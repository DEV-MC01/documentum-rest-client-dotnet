using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace DQLinterpreter.Tests
{
	[TestClass]
	public class ProgramTests
	{
		[TestMethod]
		public void ProgramFragment_DeserializePropertiesAsDictionary_Succeeded()
		{
			string[] fresult = new[] { @"
{
            ""definition"": ""https://s001rp-stdo1/dctm-rest/repositories/kvip/types/eifx_deliverable_doc"",
            ""json-root"": ""query-result"",
            ""properties"":
                {
                ""eif_acceptance_code"": ""B - \u041d\u0435\u0437\u043d\u0430\u0447\u0438\u0442\u0435\u043b\u044c\u043d\u044b\u0435 \u0437\u0430\u043c\u0435\u0447\u0430\u043d\u0438\u044f. \u0423\u0447\u0435\u0441\u0442\u044c \u0432 \u0441\u043b\u0443\u0447\u0430\u0435 \u0432\u044b\u043f\u0443\u0441\u043a\u0430 \u043d\u043e\u0432\u043e\u0439 \u0440\u0435\u0432\u0438\u0437\u0438\u0438"",
                ""eif_alt_doc_number"": ""PDH2.12953-123-TX.DS-4071"",
                ""eif_contract_number"": ""12953_WSN_PDH2_\u041f\u0440\u043e\u0435\u043a\u0442\u0438\u0440\u043e\u0432\u0430\u043d\u0438\u0435, \u043f\u043e\u0441\u0442\u0430\u0432\u043a\u0430 \u043e\u0431\u043e\u0440\u0443\u0434\u043e\u0432\u0430\u043d\u0438\u044f \u0438 \u043c\u0430\u0442\u0435\u0440\u0438\u0430\u043b\u043e\u0432 \u0438 \u0443\u0441\u043b\u0443\u0433\u0438 \u043d\u0430 \u043f\u043b\u043e\u0449\u0430\u0434\u043a\u0435 \u0432 \u0447\u0430\u0441\u0442\u0438 \u0443\u0441\u0442\u0430\u043d\u043e\u0432\u043e\u043a \u0414\u0413\u041f \u0438 \u041f\u041f"",
                ""eif_historic_state"": ""Latest"",
                ""eif_issue_reason"": ""IFD \u2013 \u0412\u044b\u043f\u0443\u0449\u0435\u043d\u043e \u0434\u043b\u044f \u043f\u0440\u043e\u0435\u043a\u0442\u0438\u0440\u043e\u0432\u0430\u043d\u0438\u044f"",
                ""eif_originator"": """",
                ""eif_project_ref"": ""PDH2"",
                ""eif_revision"": ""1"",
                ""eif_type_of_doc"": """",
                ""object_name"": ""PDH2.12953-123-TX.DS-4071"",
                ""r_creation_date"": ""2023-05-22T12:46:52.000+00:00"",
                ""r_modify_date"": ""2023-05-30T19:21:50.000+00:00"",
                ""r_object_id"": ""0903383f8125fc83"",
                ""sib_change_number"": """",
                ""sib_customer"": """",
                ""sib_doc_type_code"": """",
                ""sib_document_title"": ""\u0422\u0435\u0445\u043d\u043e\u043b\u043e\u0433\u0438\u0447\u0435\u0441\u043a\u0430\u044f \u0441\u043f\u0435\u0446\u0438\u0444\u0438\u043a\u0430\u0446\u0438\u044f \u0434\u043b\u044f \u043f\u043e\u0432\u0435\u0440\u0445\u043d\u043e\u0441\u0442\u043d\u043e\u0433\u043e \u043a\u043e\u043d\u0434\u0435\u043d\u0441\u0430\u0442\u043e\u0440\u0430 \u0442\u0435\u043f\u043b\u043e\u0432\u043e\u0433\u043e \u043d\u0430\u0441\u043e\u0441\u0430 (E-12371)"",
                ""sib_document_title_eng"": ""Process Datasheet for Heat Pump Surface Condenser (E-12371)"",
                ""sib_language"": """",
                ""sib_purchase_claim_number"": """",
                ""sib_restriction_level"": """",
                ""sib_revision_date"": ""2023-05-18T09:00:00.000+00:00"",
                ""sib_unit_title_code"": ""1200 - \u0423\u0441\u0442\u0430\u043d\u043e\u0432\u043a\u0430 \u0414\u0413\u041f (\u0432\u043a\u043b. \u0413\u041f) \\ Propane dehydration unit (inc. Unit Plot Plan)"",
                ""sib_vendor"": """"
                }
            }
" };

			var objectIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			foreach (var item in fresult)
			{
				Emc.Documentum.Rest.DocClass.CP_Document rootObject = new Emc.Documentum.Rest.DocClass.CP_Document();
				string fixedDate = item.Replace("new Date(\r\n      ", "\"").Replace("\r\n    ),", "\",").Replace("\r\n    )\r\n", "\"\r\n");
				try
				{
					var options = new JsonSerializerOptions
					{
						WriteIndented = true,
						AllowTrailingCommas = true,
						PropertyNameCaseInsensitive = true

					};
					rootObject = JsonSerializer.Deserialize<Emc.Documentum.Rest.DocClass.CP_Document>(fixedDate, options);
				}
				catch (Exception e)
				{
					Console.WriteLine("Deserialization ERROR: \r" + fixedDate + "\r with error: \r " + e.Message);
				}

				//Console.WriteLine(rootObject.properties.r_object_id);
				try
				{
					string objectName = rootObject.properties["r_object_id"];
					if (!string.IsNullOrWhiteSpace(objectName)) objectIds.Add(objectName);

					foreach (var prop in rootObject.properties)
					{
						Debug.WriteLine(prop.Key);
						if (prop.Value != null)
						{
							//Console.WriteLine(prop.GetType());
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
								Debug.WriteLine("\"" + objectName + "\";\"" + propertyName + "\";\"" + propertyValue.Substring(0, 250) + "..." + "\"");
								//cvsFileContent.Add("\"" + objectName + "\";\"" + propertyName + "\";\"" + propertyValue.Substring(0, 250) + "..." + "\"");
							}
							else if (!string.IsNullOrEmpty(propertyValue))
							{
								Debug.WriteLine("\"" + objectName + "\";\"" + propertyName + "\";\"" + propertyValue + "\"");
								//cvsFileContent.Add("\"" + objectName + "\";\"" + propertyName + "\";\"" + propertyValue + "\"");
							}
							Debug.WriteLine("Name: {0}, Value: {1}", propertyName, propertyValue);
						}
					}
				}
				catch (Exception)
				{
					throw;
				}
			}

			Assert.AreEqual(1, objectIds.Count);
			Assert.AreEqual("0903383f8125fc83", objectIds.First());
		}
	}
}
