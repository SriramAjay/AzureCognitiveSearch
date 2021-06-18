using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Blobtrigger
{
    public static class Function1
    {
        [FunctionName("SavedatatoSqlAfterupload")]
        public static async Task<IActionResult> Run([BlobTrigger("dev/{name}", Connection = "")]Stream myBlob, string name, string BlobTrigger, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            string responseMessage = string.Empty;

            try
            {
                // Get the connection string from app settings and use it to create a connection.
                var str = Environment.GetEnvironmentVariable("AzureSqlStorageConnectionString");
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    var text = "INSERT INTO Persons(Person_Name,Comments,Comment_Date)" +
                         "values('" + name + "','" + BlobTrigger + "',getdate())";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        // Execute the command and log the # rows affected.
                        var rows =  cmd.ExecuteNonQueryAsync();
                        log.LogInformation($"{rows} rows were updated");
                    }
                }

                responseMessage = "Hi, row Inserted successfully";
                return new OkObjectResult(responseMessage);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
