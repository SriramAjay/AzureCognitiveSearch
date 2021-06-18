using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Storage;
using Azure.Storage.Blobs;

namespace FileUploading
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
           //[Blob("dev/{name}",FileAccess.Write,Connection ="AzureWebJobsStorage")] Stream blob,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            var formdata = await req.ReadFormAsync();
            string connectionstring = "";
            string container = "dev";

            var file = req.Form.Files["file"];

            var blobcontainer = new BlobContainerClient(connectionstring, container);
            //blob.

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;



            return new OkObjectResult($"file name is {file.FileName} \n length of the file is {file.Length} \n content of the file is ");
        }
    }
}
