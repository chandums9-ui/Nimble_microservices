using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

public class Program
{
    static async Task Main()
    {
        // Define connection strings
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, true);
        IConfiguration configuration = builder.Build();

        //tragetDB connection
        string bankFeedbConnection = configuration.GetSection("ConnectionStrings:BankFeedDataSharing").Value;

        string ClientDBs = configuration.GetSection("ClientDBs:DBs").Value;
        string[] commaSepartedDBs = null;

        if (!string.IsNullOrEmpty(ClientDBs))
            commaSepartedDBs = ClientDBs.Split(',');
        try
        {
            if (commaSepartedDBs != null && commaSepartedDBs.Count() > 0)
            {
                foreach (var db in commaSepartedDBs)
                {
                    string dbName = db.Trim();

                    string coreDBConnection = configuration.GetConnectionString(dbName);


                    // Define the query to select data from the source database
                    string selectQuery = "SELECT * FROM BankFeedRules where status <>3";

                    string selectQuery1 = "SELECT * FROM BankFeedRuleDetails where ruleid in(select Id from BankFeedRules where status <>3)";

                    // Create connections
                    using (SqlConnection sourceConnection = new SqlConnection(coreDBConnection))
                    using (SqlConnection targetConnection = new SqlConnection(bankFeedbConnection))
                    {
                        try
                        {

                            // Open connections
                            sourceConnection.Open();
                            targetConnection.Open();

                            // Retrieve data from the source database
                            DataTable dataTable = new DataTable();
                            DataTable dataTable1 = new DataTable();
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, sourceConnection);
                            dataAdapter.Fill(dataTable);

                            dataAdapter = new SqlDataAdapter(selectQuery1, sourceConnection);
                            dataAdapter.Fill(dataTable1);

                            // Insert data into the target database
                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(targetConnection))
                            {
                                long parentMaxID = 0;

                                string checkParentQuery = "select max(Id) from FeedRule";
                                using (SqlCommand checkCommand = new SqlCommand(checkParentQuery, targetConnection))
                                {
                                    object result = checkCommand.ExecuteScalar();

                                    if (result != null)
                                    {
                                        // Parent exists, return its ID
                                        parentMaxID = Convert.ToInt64(result);
                                    }
                                }

                                using (SqlBulkCopy bulkCopy1 = new SqlBulkCopy(targetConnection))
                                {
                                    // Set the destination table name
                                    bulkCopy1.DestinationTableName = "FeedRule";


                                    // Map columns if necessary
                                    bulkCopy1.ColumnMappings.Add("CorporationID", "CorporationID");
                                    bulkCopy1.ColumnMappings.Add("AccountID", "BankOrCreditAccountID");
                                    bulkCopy1.ColumnMappings.Add("SetupFor", "SetUpFor");
                                    bulkCopy1.ColumnMappings.Add("AutoApply", "AutoApplyEnable");
                                    bulkCopy1.ColumnMappings.Add("priority", "RulePriority");
                                    bulkCopy1.ColumnMappings.Add("Status", "Status");
                                    bulkCopy1.ColumnMappings.Add("ActualTransactionType", "FeedTransType");

                                    // Write data to the target database
                                    bulkCopy1.WriteToServer(dataTable);
                                }


                                using (SqlBulkCopy bulkCopy2 = new SqlBulkCopy(targetConnection))
                                {
                                    bulkCopy2.DestinationTableName = "FeedRuleMapping";

                                    dataTable.Columns.Add("ruleID", typeof(long));
                                    long i = parentMaxID + 1;
                                    foreach (DataRow row in dataTable.Rows)
                                    {

                                        row["ruleID"] = i++.ToString();
                                    }

                                    bulkCopy2.ColumnMappings.Add("ruleID", "ID");
                                    bulkCopy2.ColumnMappings.Add("CorporationID", "CorpID");
                                    bulkCopy2.ColumnMappings.Add("MappedAccountID", "AccountID");
                                    bulkCopy2.ColumnMappings.Add("PayeeID", "NameID");
                                    bulkCopy2.ColumnMappings.Add("SourceType", "NameType");
                                    bulkCopy2.ColumnMappings.Add("Type", "ActTransType");
                                    //bulkCopy.ColumnMappings.


                                    bulkCopy2.WriteToServer(dataTable);

                                }

                                using (SqlBulkCopy bulkCopy3 = new SqlBulkCopy(targetConnection))
                                {
                                    bulkCopy3.DestinationTableName = "FeedRuleDetails";

                                    dataTable1.Columns.Add("FeedRuleID", typeof(long));
                                    long j = parentMaxID + 1;
                                    long PrevRuleId = 0;
                                    foreach (DataRow row in dataTable1.Rows)
                                    {

                                        long prenRuleid = Convert.ToInt64(row["RuleID"].ToString());

                                        if (PrevRuleId == prenRuleid)
                                        {
                                            j = j - 1;
                                            row["FeedRuleID"] = j.ToString();

                                            j++;
                                        }
                                        else
                                        {
                                            row["FeedRuleID"] = j++.ToString();

                                        }
                                        PrevRuleId = prenRuleid;
                                    }


                                    bulkCopy3.ColumnMappings.Add("FeedRuleID", "FeedRuleID");
                                    bulkCopy3.ColumnMappings.Add("DescriptionAmount", "RuleType");
                                    bulkCopy3.ColumnMappings.Add("SourceType", "FilterType");
                                    bulkCopy3.ColumnMappings.Add("Value", "Val");


                                    bulkCopy3.WriteToServer(dataTable1);
                                }
                            }

                            Console.WriteLine("Data transfer complete.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred: " + ex.Message);
                        }

                        // Close connections
                        sourceConnection.Close();
                        targetConnection.Close();
                    }

                }
            }
        }
        catch(Exception ex)
        {
            throw;
        }
    }


}