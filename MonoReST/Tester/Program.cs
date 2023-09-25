using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Emc.Documentum.Rest.Net;
using System.IO;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Emc.Documentum.Rest.Test
{
    class Program
    {
        private const string ARGUMENT_NAME_REMOTE_CONFIG_PATH = "-remoteConfigPath";

        private static string _remoteConfigPath;
        private static NameValueCollection _currentConfigProfile;
        private static RestController client;
        private static bool getFiles;
        private static bool getDelta;
        private static bool generateCSV;
        private static bool generateJSON;
        private static string saveResultsPath;
        private static string timeStampFilePath;
        private static string RestHomeUri;
        private static int itemsPerPage;
        private static string repositoryName;

        [STAThread]
        static void Main(string[] args)
        {
            AcceptArguments(args);
            ApplyConfig();

            itemsPerPage = int.Parse(_currentConfigProfile["itemsPerPage"]);
            saveResultsPath = _currentConfigProfile["exportMetadataDirectory"];
            var windowsAuthentication = Boolean.Parse(_currentConfigProfile["windowsAuthentication"]);
            var defaultUsername = _currentConfigProfile["defaultUsername"];
            var defaultPassword = _currentConfigProfile["defaultPassword"];
            var decryptedPassword = Utl.Crypt.DecryptString(defaultPassword);
            var ignoreInvalidSslCertificate = Boolean.Parse(_currentConfigProfile["ignoreInvalidSslCertificate"]);
            RestHomeUri = _currentConfigProfile["defaultRestHomeUri"];
            repositoryName = _currentConfigProfile["defaultRepositoryName"];
            generateCSV = Boolean.Parse(_currentConfigProfile["generateCSV"]);
            generateJSON = Boolean.Parse(_currentConfigProfile["generateJSON"]);
            getFiles = Boolean.Parse(_currentConfigProfile["getFiles"]);
            getDelta = Boolean.Parse(_currentConfigProfile["getDelta"]);
            timeStampFilePath = _currentConfigProfile["timeStampfilePath"];
            var docTemplateToDownloadRenditions = _currentConfigProfile["docTemplateToDownloadRenditions"];
            var saveLogFile = Boolean.Parse(_currentConfigProfile["saveLogFile"]);
            var logLevel = _currentConfigProfile["logLevel"];
            var dqlSections = _currentConfigProfile.AllKeys.Where(k => k.StartsWith("dql_"));

            PrepareResultArea();

            FileStream logFileStream = null;
            StreamWriter logStreamWriter = null;
            if (saveLogFile)
            {
                var logFile = Path.Combine(saveResultsPath, "logFile.txt");
                logFileStream = new FileStream(logFile, FileMode.Create, FileAccess.Write);
                logStreamWriter = new StreamWriter(logFileStream);
                logStreamWriter.AutoFlush = true;
                Console.SetOut(logStreamWriter);
            }

            try
            {
                SetupClient(windowsAuthentication, defaultUsername, !string.IsNullOrEmpty(decryptedPassword) ? decryptedPassword : defaultPassword, ignoreInvalidSslCertificate, logLevel);
                var useCaseTests = new UseCaseTests(client, _currentConfigProfile, RestHomeUri, repositoryName, false, false, ".\\DocRenditions", 1, 1);
                foreach (var dqlSection in dqlSections)
                {
                    Console.WriteLine("Section '{0}' is being processed...", dqlSection);
                    var documentArea = dqlSection.Replace("dql_", "");
                    var objectIds = ExportDocumentMetadata(_currentConfigProfile[dqlSection], documentArea + "_", logLevel);
                    if (getFiles) useCaseTests.Start(documentArea, !string.IsNullOrEmpty(docTemplateToDownloadRenditions) ? new Regex(docTemplateToDownloadRenditions) : null, objectIds);
                    Console.WriteLine("Section '{0}' has been processed.", dqlSection);
                }

                if (getDelta)
                {
                    string newTimeStamp = DateTime.Now.ToString("dd.MM.yyyy");
                    File.WriteAllText(timeStampFilePath, newTimeStamp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("The following fatal exception has been raised.\n{0}", ex);
            }
            finally
            {
                if (saveLogFile)
                {
                    Console.SetOut(TextWriter.Null);
                    logStreamWriter?.Close();
                    logFileStream?.Close();
                }
            }
        }
        //r_modify_date >= date('01.01.2021 00:00:00', 'dd.MM.yyyy HH:mm:ss')
        private static void SetupClient(bool windowsAuth, string username, string password, bool ignoreInvalidSslCertificate, string logLevel = null)
        {
            // unfortunately, in some cases SSL-certificate may be expired
            if (ignoreInvalidSslCertificate) ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            client = windowsAuth ? new RestController(5) : new RestController(username, password);
            JsonDotnetJsonSerializer serializer = new JsonDotnetJsonSerializer();
            serializer.PrintStreamBeforeDeserialize = true;
            client.JsonSerializer = serializer;

            if (string.IsNullOrEmpty(logLevel) || !Enum.TryParse(logLevel, true, out Utility.LogLevel parsedLogLevel)) parsedLogLevel = Utility.LogLevel.FATAL;
            client.Logger = new Utility.LoggerFacade("RestServices", "NA", "NA", username, parsedLogLevel);
        }

        private static string[] ExportDocumentMetadata(string dql, string fileNamePrefix, string logLevel)
        {
            var objectIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Console.WriteLine("Fetching documents metadata from Capital Project...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            fileNamePrefix = !string.IsNullOrWhiteSpace(fileNamePrefix) ? fileNamePrefix : "doc_";

            if (getDelta)
            {
                if (!File.Exists(timeStampFilePath))
                {
                    File.Create(timeStampFilePath);
                }
                else
                {
                    string timeStamp = (dql.Contains("where ") ? " and " : " where ") + "r_modify_date >= date('" + File.ReadAllText(timeStampFilePath) + "','dd.MM.yyyy')";
                    dql += timeStamp;
                }
            }

            List<string> fresult = DqlQueryExecute.Run(client, RestHomeUri, dql, itemsPerPage, repositoryName, true);
            if (fresult.Count < 1)
                return objectIds.ToArray();

            //List<string> jsonfileformatresults = new List<string>();
            stopwatch.Stop();
            Console.WriteLine("Time elapsed to get data from Capital Project: " + stopwatch.Elapsed);
            Console.WriteLine("Parsing query results...");

            List<string> cvsFileContent = new List<string>();
            cvsFileContent.Add("\"DocNumber\";\"AttributeName\";\"AttributeValue\"");

            // Rootobject dataSet2 = JsonConvert.DeserializeObject<Rootobject>(string.Join("",fresult), new JavaScriptDateTimeConverter());
            Console.OutputEncoding = Encoding.UTF8;

            foreach (var item in fresult)
            {
                DocClass.CP_Document rootObject = new DocClass.CP_Document();
                string fixedDate = item.Replace("new Date(\r\n      ", "\"").Replace("\r\n    ),", "\",").Replace("\r\n    )\r\n", "\"\r\n");
                try
                {
                    rootObject = JsonConvert.DeserializeObject<DocClass.CP_Document>(fixedDate);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Deserialization ERROR: \r" + fixedDate + "\r with error: \r " + e.Message);
                }

                try
                {
                    string objectName = rootObject.properties["r_object_id"];
                    if (!string.IsNullOrWhiteSpace(objectName)) objectIds.Add(objectName);

                    LogDebug(string.Format("{0}", objectName), logLevel);
                    LogDebug("Properties observed:", logLevel);

                    foreach (var prop in rootObject.properties)
                    {
                        LogDebug(string.Format("Name: {0}", prop.Key), logLevel);
                        if (prop.Value != null)
                        {
                            string propertyName = prop.Key;
                            var propertyValue = prop.Value
                                .Replace('"', ' ').Replace("\r\n", " ").Replace("\r", " ")
                                .Replace("\n", " ").Replace("\t", " ").Replace(";", "_")
                                .Replace('"', ' ').Replace(Environment.NewLine, " ").Replace("\\", " ").Trim();
                            if (propertyName.Contains("date"))
                            {
                                if (DateTime.TryParse(propertyValue, out var dateTime))
                                {
                                    propertyValue = dateTime.ToString("yyyy-MM-dd hh:mm:ss");
                                }
                                else
                                {
                                    dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                                    propertyValue = dateTime.AddMilliseconds(long.Parse(propertyValue)).ToString("yyyy-MM-dd hh:mm:ss");
                                }
                            }

                            //Due to limit value length in GK, we need to cut long descriptions
                            if (!string.IsNullOrEmpty(propertyValue) && propertyValue.Length > 254)
                            {
                                cvsFileContent.Add("\"" + objectName + "\";\"" + propertyName + "\";\"" + propertyValue.Substring(0, 250) + "..." + "\"");
                            }
                            else if (!string.IsNullOrEmpty(propertyValue))
                            {
                                cvsFileContent.Add("\"" + objectName + "\";\"" + propertyName + "\";\"" + propertyValue + "\"");
                            }
                            LogDebug(string.Format("Name: {0}, Value: {1}", propertyName, propertyValue), logLevel);
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error occurred while parsing results");
                }

            }

            SaveResultToFile(cvsFileContent, fresult, fileNamePrefix);

            return objectIds.ToArray();
        }

  //      private static void ExportLINKmetada(string dql)
  //      {
  //          Console.WriteLine("Getting link data from Capital project...");
  //          Stopwatch stopwatch = new Stopwatch();
  //          stopwatch.Start();
  //          string fileNamePrefix = "link_";
  //          if (getDelta)
  //          {
  //              if (!File.Exists(timeStampFilePath))
  //              {
  //                  File.Create(timeStampFilePath);
  //              }
  //              else
  //              {
  //                  string timeStamp = " where trm.r_modify_date>=date('" + File.ReadAllText(timeStampFilePath) + "','dd.MM.yyyy')";
  //                  dql += timeStamp;
  //              }
  //          }
  //          List<string> fresult = DqlQueryExecute.Run(client, RestHomeUri, dql, itemsPerPage, repositoryName);
  //          if (fresult.Count < 1)
  //          {
  //              return;
  //          }

  //          //List<string> jsonfileformatresults = new List<string>();
  //          stopwatch.Stop();
  //          Console.WriteLine("Time elapsed to get data from Capital Project: " + stopwatch.Elapsed);
  //          Console.WriteLine("Parsing query results...");
  //          List<string> cvsFileContent = new List<string>();
  //          if (generateCSV)
  //          {
  //              cvsFileContent.Add("\"Link_id\";\"parent_id\";\"child_id\"");
  //              foreach (var item in fresult)
  //              {
  //                  LinksClass.TRMtoDocLink rootObject = new LinksClass.TRMtoDocLink();
  //                  try
  //                  {
  //                      var options = new JsonSerializerOptions
  //                      {
  //                          WriteIndented = true,
  //                          AllowTrailingCommas = true,
  //                          PropertyNameCaseInsensitive = true

  //                      };
  //                      rootObject = System.Text.Json.JsonSerializer.Deserialize<LinksClass.TRMtoDocLink>(item, options);
  //                  }
  //                  catch (Exception e)
  //                  {
  //                      Console.WriteLine("deserializing ERROR: \r" + item + "\r with error: \r " + e.Message);
  //                  }

  //                  try
  //                  {
  //                      cvsFileContent.Add("\"" + rootObject.properties.r_object_id + "\";\"" + rootObject.properties.parent_id + "\";\"" + rootObject.properties.child_id + "\"");
  //                  }
  //                  catch (Exception)
  //                  {
  //                      Console.WriteLine("Error occured while parsing results");
  //                  }
  //              }
  //          }

  //          SaveResultToFile(cvsFileContent, fresult, fileNamePrefix);
  //    }

        public static void SaveResultToFile(List<string> csvContent, List<string> jsonContent, string fileNamePrefix)
        {
            var fileToSaveResults = Path.Combine(saveResultsPath, fileNamePrefix + "exportData.csv");
            var jsonfileToSaveResults = Path.Combine(saveResultsPath, fileNamePrefix + "exportData.json");

            if (generateCSV)
            {
                using (TextWriter tw = new StreamWriter(fileToSaveResults))
                {
                    Console.WriteLine("Saving CSV in progress...");
                    foreach (String s in csvContent.Distinct())
                        tw.WriteLine(s);
                }
            }

            if (generateJSON)
            {
                Console.WriteLine("Saving JSON in progress...");

                using (TextWriter tw2 = new StreamWriter(jsonfileToSaveResults))
                {
                    foreach (String s in jsonContent)
                        tw2.WriteLine(s);
                }
            }
        }

        public static void PrepareResultArea()
        {
            if (!Directory.Exists(saveResultsPath))
                Directory.CreateDirectory(saveResultsPath);

            var exportDataFiles = Directory.GetFiles(saveResultsPath ?? ".\\", "*exportData.*", SearchOption.TopDirectoryOnly);
            foreach (var exportDataFile in exportDataFiles)
                File.Delete(exportDataFile);
        }

        private static void LogDebug(string message, string logLevel)
        {
            if (string.Equals(logLevel, "debug", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(message);
            }
        }

        private static void AcceptArguments(string[] args)
        {
            if (args?.Length == 0) return;

            foreach (var arg in args)
            {
                var argComponents = arg?.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (argComponents?.Length != 2) throw new ConfigurationException($"The argument '{arg}' could not be accepted as it should fit the format '-argName=argValue'");

                var argName = argComponents[0];
                switch (argName)
                {
                    case ARGUMENT_NAME_REMOTE_CONFIG_PATH:
                        _remoteConfigPath = argComponents[1];
                        break;
                    default:
                        throw new NotSupportedException($"The input parameter '{argName}' is not supported.");
                }
            }
        }

        private static void ApplyConfig()
        {
            if (string.IsNullOrWhiteSpace(_remoteConfigPath))
            {
                _currentConfigProfile = ConfigurationManager.GetSection("restconfig") as NameValueCollection;
                return;
            }

            if (!File.Exists(_remoteConfigPath)) throw new FileNotFoundException($"The remote config. file '{_remoteConfigPath}' could not be found.");

            var configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = _remoteConfigPath;
            var remoteConfig = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            var settingsXml = remoteConfig.GetSection("restconfig").SectionInformation.GetRawXml();
            var settingsXmlDoc = new System.Xml.XmlDocument();
            settingsXmlDoc.Load(new StringReader(settingsXml));
            _currentConfigProfile = new NameValueSectionHandler().Create(null, null, settingsXmlDoc.DocumentElement) as NameValueCollection;
        }
	}
}

