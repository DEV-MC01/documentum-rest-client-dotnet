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

            string fileToSaveResults = System.IO.Path.Combine(saveResultsPath, "export_DocumentsMetadata.csv");
            string jsonfileToSaveResults = System.IO.Path.Combine(saveResultsPath, "_json.txt");

            setupClient(defaultUsername, defaultPassword);

            List<string> fresult = DqlQueryTest.Run(client, RestHomeUri, dql, itemsPerPage, repositoryName);
            List<string> jsonfileformatresults = new List<string>();

            List<string> trmToDocRefs = new List<string>();
            trmToDocRefs.Add("\"DocNumber\";\"AttributeName\";\"AttributeValue\"");
            foreach (var item in fresult)
            {

                Rootobject dataSet = JsonConvert.DeserializeObject<Rootobject>(item);
                //Console.OutputEncoding = System.Text.Encoding.UTF8;

                foreach (var entry in dataSet.entries)
                {
                    //Console.OutputEncoding = System.Text.Encoding.UTF8;

                    Properties properties = entry.content.properties;
                    foreach (var prop in properties.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        string propertyName = prop.Name;
                        string propertyValue = prop.GetValue(properties, null) as string;

                        if (!string.IsNullOrEmpty(propertyValue) && propertyValue.Length > 254)
                        {
                            trmToDocRefs.Add('"' + entry.content.properties.object_name + '"' 
                                + ";" + '"' + propertyName + '"' + ";" 
                                + '"' + propertyValue.Replace('"', ' ')
                                .Replace("\r\n", " ").Replace(";", "_")
                                .Replace('"', ' ').Replace("\t", "")
                                .Substring(1, 254) + '"');
                        }
                        else if (!string.IsNullOrEmpty(propertyValue))
                        {
                            trmToDocRefs.Add('"' + entry.content.properties.object_name + '"' 
                                + ";" + '"' + propertyName + '"' + ";" 
                                + '"' + propertyValue.Replace('"', ' ')
                                .Replace("\r\n", " ").Replace(";", "_")
                                .Replace('"', ' ').Replace("\t", "") + '"');
                        }
                        //Console.WriteLine("Name: {0}, Value: {1}", prop.Name, prop.GetValue(properties, null));
                    }
                }
            }
            using (TextWriter tw = new StreamWriter(fileToSaveResults))
            {
                foreach (String s in trmToDocRefs)
                    tw.WriteLine(s);
            }
            using (TextWriter tw2 = new StreamWriter(jsonfileToSaveResults))
            {
                foreach (String s in fresult)
                    tw2.WriteLine(s);
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

