using Emc.Documentum.Rest.DataModel;
using Emc.Documentum.Rest.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emc.Documentum.Rest.Test
{
    public class DqlQueryExecute
    {
        public static List<string> Run(RestController client, string RestHomeUri, string query, int itemsPerPage, string repositoryName, bool includeTotal = false)
        {
            List<string> results = new List<string>();
            HomeDocument home = client.Get<HomeDocument>(RestHomeUri, null);
            Feed<Repository> repositories = home.GetRepositories<Repository>(new FeedGetOptions { Inline = true, Links = true });
            Repository repository = repositories.FindInlineEntry(repositoryName);

            //Console.WriteLine(string.Format("Running DQL query '{0}' on repository '{1}', with page size {2}", query, repository.Name, itemsPerPage));

            // REST call to get the 1st page of the dql query
            Feed<PersistentObject> queryResult = repository.ExecuteDQL<PersistentObject>(query, new FeedGetOptions() { ItemsPerPage = itemsPerPage, Raw = true, Recursive = false, Links = false, IncludeTotal = includeTotal });

            if (queryResult != null)
            {
                var pageCount = queryResult.PageCount;
                var currentPage = 1;
                while (true)
                {
                    Console.WriteLine("Page {0}{1}", currentPage, includeTotal && pageCount > 0 ? $" of {pageCount}" : string.Empty);
                    currentPage++;
                    foreach (var obj in queryResult.Entries)
                    {
                        try
                        {
                            results.Add(obj.Content.ToString());
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Error in results add");
                        }
                        //docProcessed++;
                    }
                    try
                    {
                        queryResult = queryResult.NextPage();
                    }
                    catch (Exception)
                    {
                        
                        Console.WriteLine("All pages received");
                        break;
                    }

                }
                //for (int i = 0; i < totalPages && queryResult != null; i++)
                //{
                //    //        //Console.WriteLine(queryResult.ToString());
                //    //        //Console.WriteLine(queryResult.Entries.ToString());
                //    //        //Console.WriteLine(queryResult.Entries[i].Content.ToString());
                //    //        //Console.WriteLine("**************************** PAGE " + (i + 1) + " *******************************");
                //    foreach (var obj in queryResult.Entries)
                //    {
                //        try
                //        {
                //            results.Add(obj.Content.ToString());
                //        }
                //        catch (Exception)
                //        {

                //            Console.WriteLine("Error in results add");
                //        }
                //        docProcessed++;
                //    }
                //    //try
                //    //{
                //    //    //results.Add(obj.Content.ToString());
                //    //    Console.WriteLine(i.ToString());
                //    //    results.Add(queryResult.ToString());
                //    //}
                //    //catch (Exception)
                //    //{

                //    //    Console.WriteLine("Error in results add");
                //    //}
                //    //        //REST call to get next page of the dql query
                //    try
                //    {
                //        queryResult = queryResult.NextPage();
                //    }
                //    catch (Exception)
                //    {

                //        Console.WriteLine("Error in NextPage");
                //        //Console.WriteLine(Exception);
                //    }
                //    //        //Console.WriteLine("*******************************************************************");
                //    //        //Console.WriteLine("Page:" + (i + 1) + " Results: " + docProcessed + " out of " + totalResults + " Processed");
                //    //        //Console.WriteLine("*******************************************************************");
                //    //        //Console.WriteLine("\n\n");
                //    //        //if (false)
                //    //        //{
                //    //        //    Console.WriteLine("Press 'q' to quit, 'g' to run to end, or any other key to run next page..");
                //    //        //    ConsoleKeyInfo next = Console.ReadKey();
                //    //        //    if (next.KeyChar.Equals('q'))
                //    //        //    {
                //    //        //        return;
                //    //        //    }
                //    //        //    if (next.KeyChar.Equals('g'))
                //    //        //    {
                //    //        //        pauseBetweenPages = false;
                //    //        //    }
                //    //        //}
                //}
                //    Console.WriteLine("Data Recieved");
            }
            else
            {
                //Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("#### No data returned from query! ####");
                //Console.ForegroundColor = ConsoleColor.White;
            }
            //if (printResult) Console.WriteLine(queryResult == null ? "NULL" : queryResult.ToString());
            // results.Add(queryResult.ToString());
            return results;
        }

        //private static string GetAttr(PersistentObject po, string[] attrs)
        //{
        //    foreach (string attr in attrs)
        //    {
        //        var v = po.GetPropertyValue(attr);
        //        if (v != null)
        //        {
        //            return v.ToString();
        //        }
        //    }
        //    return "UNDEFINED";
        //}

    }
}
