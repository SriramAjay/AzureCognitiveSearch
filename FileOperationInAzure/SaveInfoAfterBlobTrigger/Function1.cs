using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SaveInfoAfterBlobTrigger
{
    public static class Function1
    {
        [FunctionName("Blobtrigger")]
        public static void Run([BlobTrigger("dev/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
      
            string comment = "inserted";
            string responseMessage = string.Empty;

            try
            {
                // Get the connection string from app settings and use it to create a connection.
                var str = Environment.GetEnvironmentVariable("AzureSqlStorageConnectionString");
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    var text = "INSERT INTO Persons(Person_Name,Comments,Comment_Date)" +
                         "values('" + name + "','" + comment + "',getdate())";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        // Execute the command and log the # rows affected.
                        var rows =  cmd.ExecuteNonQueryAsync();
                        log.LogInformation($"{rows} rows were updated");
                    }
                }

                responseMessage = "Hi, file Inserted successfully";

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
