using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using System.Data.SqlClient;
using Azure.Storage.Blobs;

namespace AzureFunctions
{
    public static class Function1
    {
        [FunctionName("TestHttp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            string comment = req.Query["comment"];
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
                        var rows = await cmd.ExecuteNonQueryAsync();
                        log.LogInformation($"{rows} rows were updated");
                    }
                }

                responseMessage = "Hi, row Inserted successfully";

            }
            catch (Exception ex)
            {
                throw;
            }
           

            return new OkObjectResult(responseMessage);
        }


        [FunctionName("FileUploadtoblob")]
        public static async Task<IActionResult> FileUploadtoblob(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,

         ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = "test";
            string responseMessage = string.Empty;

         
            try
            {
                string fileconnectionstring = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                BlobServiceClient blobService = new BlobServiceClient(fileconnectionstring);
                BlobContainerClient blobContainer = blobService.GetBlobContainerClient("dev");

                BlobClient blobClient = blobContainer.GetBlobClient(name);
                var memory = new MemoryStream();
                //var file = req.Form.Files["file"];
                await req.Form.Files["file"].CopyToAsync(memory);
                memory.Position = 0;

                var result = await blobClient.UploadAsync(memory, true);
                memory.Close();

                

                responseMessage =" file uploaded successfully";

            }
            catch (Exception ex)
            {
                throw;
            }


            return new OkObjectResult(responseMessage);
        }


        [FunctionName("Blobtrigger")]
        public static void blob([BlobTrigger("dev/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
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
                        var rows = cmd.ExecuteNonQueryAsync();
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
