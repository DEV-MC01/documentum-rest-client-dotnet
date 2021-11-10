using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Emc.Documentum.Rest.Net;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Emc.Documentum.Rest.Test
{
    class Program
    {
        private static RestController client;
        private static bool getDelta;
        private static bool generateCSV;
        private static bool generateJSON;
        private static string fileToSaveResults;
        private static string jsonfileToSaveResults;
        private static string saveResultsPath;
        private static string timeStampFilePath;
        private static string RestHomeUri;
        private static int itemsPerPage;
        private static string repositoryName;
        //private static string saveResultFileName;

        [STAThread]
        static void Main(string[] args)
        {
            NameValueCollection config = ConfigurationManager.GetSection("restconfig") as NameValueCollection;

            itemsPerPage = int.Parse(config["itemsPerPage"]);
            string doc_dql = config["DQL"];
            string trm_dql = config["TRM_DQL"];
            string link_dql = config["Link_DQL"];


            saveResultsPath = config["exportFilesDirectory"];
            string defaultUsername = config["defaultUsername"];
            string defaultPassword = config["defaultPassword"];
            RestHomeUri = config["defaultRestHomeUri"];
            repositoryName = config["defaultRepositoryName"];
            generateCSV = Boolean.Parse(config["generateCSV"]);
            generateJSON = Boolean.Parse(config["generateJSON"]);
            getDelta = Boolean.Parse(config["getDelta"]);
            timeStampFilePath = config["timeStampfilePath"];
            //saveResultFileName = config["saveResultFileName"];

            setupClient(defaultUsername, defaultPassword);
            ExportDocumentMetadata(doc_dql);
            ExportTRMmetada(trm_dql);
            ExportLINKmetada(link_dql);
            //File export using REST API (doesn't work at the moment)
            //UseCaseTests useCaseTests = new UseCaseTests(client, RestHomeUri, repositoryName, false, false, ".\\DocRenditions", 1, 1);

            //useCaseTests.Start();

            if (getDelta)
            {
                string newTimeStamp = DateTime.Now.ToString("dd.MM.yyyy");
                File.WriteAllText(timeStampFilePath, newTimeStamp);
            }
        }
        //r_modify_date >= date('01.01.2021 00:00:00', 'dd.MM.yyyy HH:mm:ss')
        private static void setupClient(string username, string password)
        {
            client = new RestController(username, password);
            JsonDotnetJsonSerializer serializer = new JsonDotnetJsonSerializer();
            serializer.PrintStreamBeforeDeserialize = true;
            client.JsonSerializer = serializer;
        }
        private static void ExportDocumentMetadata(string dql)
        {
            Console.WriteLine("Fetching documents metadata from Capital project...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string fileNamePrefix = "doc_";

            if (getDelta)
            {
                if (!File.Exists(timeStampFilePath))
                {
                    File.Create(timeStampFilePath);
                }
                else
                {
                    string timeStamp = " and r_modify_date >= date('" + File.ReadAllText(timeStampFilePath) + "','dd.MM.yyyy')";
                    dql += timeStamp;
                }
            }


            List<string> fresult = DqlQueryExecute.Run(client, RestHomeUri, dql, itemsPerPage, repositoryName);
            if (fresult.Count < 1)
            {
                return;
            }
            //List<string> jsonfileformatresults = new List<string>();
            stopwatch.Stop();
            Console.WriteLine("Time elapsed to get data from Capital Project: " + stopwatch.Elapsed);
            Console.WriteLine("Parsing query results...");
            List<string> cvsFileContent = new List<string>();
            if (generateCSV)
            {
                cvsFileContent.Add("\"DocNumber\";\"AttributeName\";\"AttributeValue\"");

                // Rootobject dataSet2 = JsonConvert.DeserializeObject<Rootobject>(string.Join("",fresult), new JavaScriptDateTimeConverter());
                Console.OutputEncoding = Encoding.UTF8;

                foreach (var item in fresult)
                {
                    DocClass.CP_Document rootObject = new DocClass.CP_Document();
                    string fixedDate = item.Replace("new Date(\r\n      ", "\"").Replace("\r\n    ),", "\",").Replace("\r\n    )\r\n", "\"\r\n");
                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            AllowTrailingCommas = true,
                            PropertyNameCaseInsensitive = true

                        };
                        rootObject = System.Text.Json.JsonSerializer.Deserialize<DocClass.CP_Document>(fixedDate, options);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("deserializing ERROR: \r" + fixedDate + "\r with error: \r " + e.Message);

                    }

                    //Console.WriteLine(rootObject.properties.r_object_id);
                    try
                    {
                        foreach (PropertyInfo prop in rootObject.properties.GetType().GetProperties())
                        {

                            //Console.WriteLine(prop.Name);
                            if (prop.GetValue(rootObject.properties) != null)
                            {
                                //Console.WriteLine(prop.GetType());
                                string propertyName = prop.Name;
                                //string objectName = rootObject.properties.object_name;
                                string objectName = rootObject.properties.r_object_id;

                                var propertyValue = prop.GetValue(rootObject.properties).ToString()
                                    .Replace('"', ' ').Replace("\r\n", " ").Replace("\r", " ")
                                    .Replace("\n", " ").Replace("\t", " ").Replace(";", "_")
                                    .Replace('"', ' ').Replace(Environment.NewLine, " ").Replace("\\", " ").Trim();
                                if (propertyName.Contains("date"))
                                {
                                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                                    propertyValue = dateTime.AddMilliseconds(long.Parse(propertyValue)).ToString("yyyy-MM-dd hh:mm:ss");

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

                            }
                            //Console.WriteLine("Name: {0}, Value: {1}", prop.Name, propertyValue);
                        }
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Error occured while parsing results");
                    }

                }


            }
            SaveResultToFile(cvsFileContent, fresult, fileNamePrefix);
        }
        private static void ExportTRMmetada(string dql)
        {
            Console.WriteLine("Getting data from Capital project for TRM...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string fileNamePrefix = "trm_";
            if (getDelta)
            {
                if (!File.Exists(timeStampFilePath))
                {
                    File.Create(timeStampFilePath);
                }
                else
                {
                    string timeStamp = " and r_modify_date>=date('" + File.ReadAllText(timeStampFilePath) + "','dd.MM.yyyy')";
                    dql += timeStamp;
                }
            }


            List<string> fresult = DqlQueryExecute.Run(client, RestHomeUri, dql, itemsPerPage, repositoryName);
            if (fresult.Count < 1)
            {
                return;
            }
            //List<string> jsonfileformatresults = new List<string>();
            stopwatch.Stop();
            Console.WriteLine("Time elapsed to get data from Capital Project: " + stopwatch.Elapsed);
            Console.WriteLine("Parsing query results...");
            List<string> cvsFileContent = new List<string>();
            if (generateCSV)
            {
                cvsFileContent.Add("\"TRM_id\";\"AttributeName\";\"AttributeValue\"");

                foreach (var item in fresult)
                {
                    TRM_Class.TrmClass rootObject = new TRM_Class.TrmClass();
                    string fixedDate = item.Replace("new Date(\r\n      ", "\"").Replace("\r\n    ),", "\",").Replace("\r\n    )\r\n", "\"\r\n");
                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            AllowTrailingCommas = true,
                            PropertyNameCaseInsensitive = true

                        };
                        rootObject = System.Text.Json.JsonSerializer.Deserialize<TRM_Class.TrmClass>(fixedDate, options);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Deserializing ERROR: \r" + fixedDate + "\r with error: \r " + e.Message);

                    }

                    try
                    {
                        foreach (PropertyInfo prop in rootObject.properties.GetType().GetProperties())
                        {
                            if (prop.GetValue(rootObject.properties) != null)
                            {
                                //Console.WriteLine(prop.GetType());
                                string propertyName = prop.Name;
                                string r_objectID = rootObject.properties.r_object_id;

                                var propertyValue = prop.GetValue(rootObject.properties).ToString()
                                    .Replace('"', ' ').Replace("\r\n", " ").Replace("\r", " ")
                                    .Replace("\n", " ").Replace("\t", " ").Replace(";", "_")
                                    .Replace('"', ' ').Replace(Environment.NewLine, " ").Replace("\\", " ").Trim();
                                if (propertyName.Contains("date"))
                                {
                                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                                    propertyValue = dateTime.AddMilliseconds(long.Parse(propertyValue)).ToString("yyyy-MM-dd hh:mm:ss");

                                }
                                //Due to limit value length in GK, we need to cut long descriptions
                                if (!string.IsNullOrEmpty(propertyValue) && propertyValue.Length > 254)
                                {
                                    cvsFileContent.Add("\"" + r_objectID + "\";\"" + propertyName + "\";\"" + propertyValue.Substring(0, 250) + "..." + "\"");
                                }
                                else if (!string.IsNullOrEmpty(propertyValue))
                                {
                                    cvsFileContent.Add("\"" + r_objectID + "\";\"" + propertyName + "\";\"" + propertyValue + "\"");
                                }

                            }
                        }
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Error occured while parsing results");
                    }
                }


            }
            SaveResultToFile(cvsFileContent, fresult, fileNamePrefix);
            

        }
        private static void ExportLINKmetada(string dql)
        {
            Console.WriteLine("Getting link data from Capital project...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string fileNamePrefix = "link_";
            if (getDelta)
            {
                if (!File.Exists(timeStampFilePath))
                {
                    File.Create(timeStampFilePath);
                }
                else
                {
                    string timeStamp = " where trm.r_modify_date>=date('" + File.ReadAllText(timeStampFilePath) + "','dd.MM.yyyy')";
                    dql += timeStamp;
                }
            }
            List<string> fresult = DqlQueryExecute.Run(client, RestHomeUri, dql, itemsPerPage, repositoryName);
            if (fresult.Count < 1)
            {
                return;
            }

            //List<string> jsonfileformatresults = new List<string>();
            stopwatch.Stop();
            Console.WriteLine("Time elapsed to get data from Capital Project: " + stopwatch.Elapsed);
            Console.WriteLine("Parsing query results...");
            List<string> cvsFileContent = new List<string>();
            if (generateCSV)
            {
                cvsFileContent.Add("\"Link_id\";\"parent_id\";\"child_id\"");
                foreach (var item in fresult)
                {
                    LinksClass.TRMtoDocLink rootObject = new LinksClass.TRMtoDocLink();
                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            AllowTrailingCommas = true,
                            PropertyNameCaseInsensitive = true

                        };
                        rootObject = System.Text.Json.JsonSerializer.Deserialize<LinksClass.TRMtoDocLink>(item, options);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("deserializing ERROR: \r" + item + "\r with error: \r " + e.Message);
                    }

                    try
                    {
                        cvsFileContent.Add("\"" + rootObject.properties.r_object_id + "\";\"" + rootObject.properties.parent_id + "\";\"" + rootObject.properties.child_id +"\"");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error occured while parsing results");
                    }
                }


            }
            SaveResultToFile(cvsFileContent, fresult, fileNamePrefix);
            

        }
        public static void SaveResultToFile(List<string> csvContent, List<string> jsonContent, string _fileNamePrefix)
        {
            string csvFileName = _fileNamePrefix + "exportData.csv";
            fileToSaveResults = System.IO.Path.Combine(saveResultsPath, csvFileName);
            jsonfileToSaveResults = System.IO.Path.Combine(saveResultsPath, _fileNamePrefix + "_json.txt");

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

    }

}

