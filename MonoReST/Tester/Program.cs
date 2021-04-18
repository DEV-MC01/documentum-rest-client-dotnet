using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Emc.Documentum.Rest.DataModel;
using Emc.Documentum.Rest.Net;
using Emc.Documentum.Rest.Http.Utility;
using System.IO;
using System.Collections.Specialized;
using System.Windows.Forms;
using Emc.Documentum.Rest.DataModel.D2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Reflection;

namespace Emc.Documentum.Rest.Test
{
    class Program
    {
        private static RestController client;

        [STAThread]
        static void Main(string[] args)
        {
            NameValueCollection config = ConfigurationManager.GetSection("restconfig") as NameValueCollection;

            int itemsPerPage = int.Parse(config["itemsPerPage"]);
            string dql = config["DQL"];
            string saveResultsPath = config["exportFilesDirectory"];
            string defaultUsername = config["defaultUsername"];
            string defaultPassword = config["defaultPassword"];
            string RestHomeUri = config["defaultRestHomeUri"];
            string repositoryName = config["defaultRepositoryName"];
            bool generateCSV = Boolean.Parse(config["generateCSV"]);
            bool generateJSON = Boolean.Parse(config["generateJSON"]);
            bool getDelta = Boolean.Parse(config["getDelta"]);
            string timeStampdirPath = config["timeStampfilePath"];
            string date = "";

            if (getDelta)
            {
                if (!Directory.Exists(timeStampdirPath))
                {
                    Directory.CreateDirectory(timeStampdirPath);
                }

                //Path.Combine();
                string timeStampFilePath = Path.Combine(timeStampdirPath, "timestamp.txt");
                if (File.Exists(timeStampFilePath))
                {
                    date = File.ReadAllText(timeStampFilePath);
                    //and doc.r_modify_date > date ('13.04.2021','dd.mm.yyyy') 
                }
                if (string.IsNullOrEmpty(date))
                {
                    date = DateTime.Today.Date.ToString("dd.MM.yyyy");
                }

                dql += "and doc.r_modify_date >= date ('" + date + "','dd.mm.yyyy')";
                File.WriteAllText(timeStampFilePath, DateTime.Today.Date.ToString("dd.MM.yyyy"), Encoding.UTF8);
            }

            string fileToSaveResults = System.IO.Path.Combine(saveResultsPath, "export_DocumentsMetadata.csv");
            string jsonfileToSaveResults = System.IO.Path.Combine(saveResultsPath, "_json.txt");

            setupClient(defaultUsername, defaultPassword);

            List<string> fresult = DqlQueryTest.Run(client, RestHomeUri, dql, itemsPerPage, repositoryName);
            List<string> jsonfileformatresults = new List<string>();

            List<string> trmToDocRefs = new List<string>();
            if (generateCSV)
            {
                trmToDocRefs.Add("\"DocNumber\";\"AttributeName\";\"AttributeValue\"");
                foreach (var item in fresult)
                {
                    Rootobject dataSet = JsonConvert.DeserializeObject<Rootobject>(item);

                    foreach (var entry in dataSet.entries)
                    {
                        //Console.OutputEncoding = System.Text.Encoding.UTF8;

                        Properties properties = entry.content.properties;
                        //if (!string.IsNullOrEmpty(properties.ctrm_number) | !string.IsNullOrEmpty(properties.itrm_number))
                        //{
                            //Properties properties = JsonConvert.DeserializeObject<Properties>(item);
                            foreach (var prop in properties.GetType().GetProperties())
                            {


                                string propertyName = prop.Name;
                                string propertyValue = (string)prop.GetValue(properties, null);
                                //Console.WriteLine(properties.object_name);
                                if (!string.IsNullOrEmpty(propertyValue) && propertyValue.Length > 254)
                                {
                                    trmToDocRefs.Add('"' + properties.object_name + '"'
                                        + ";" + '"' + propertyName + '"' + ";"
                                        + '"' + propertyValue.Replace('"', ' ')
                                        .Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace(";", "_")
                                        .Replace('"', ' ').Replace(Environment.NewLine, " ")
                                        .Substring(1, 254) + '"');
                                }
                                else if (!string.IsNullOrEmpty(propertyValue))
                                {
                                    trmToDocRefs.Add('"' + properties.object_name + '"'
                                        + ";" + '"' + propertyName + '"' + ";"
                                        + '"' + propertyValue.Replace('"', ' ')
                                        .Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace(";", "_")
                                        .Replace('"', ' ').Replace(Environment.NewLine, " ") + '"');
                                }
                                //Console.WriteLine("Name: {0}, Value: {1}", prop.Name, (string)prop.GetValue(properties, null));
                            }

                        //}
                    }
                }
                using (TextWriter tw = new StreamWriter(fileToSaveResults))
                {
                    foreach (String s in trmToDocRefs.Distinct())
                        tw.WriteLine(s);
                }
            }
            if (generateJSON)
            {
                using (TextWriter tw2 = new StreamWriter(jsonfileToSaveResults))
                {
                    foreach (String s in fresult)
                        tw2.WriteLine(s);
                }
            }


        }

        private static void setupClient(string username, string password)
        {
            client = String.IsNullOrEmpty(password) ? new RestController(null, null) : new RestController(username, password);
            JsonDotnetJsonSerializer serializer = new JsonDotnetJsonSerializer();
            serializer.PrintStreamBeforeDeserialize = true;
            client.JsonSerializer = serializer;
        }


    }

}

