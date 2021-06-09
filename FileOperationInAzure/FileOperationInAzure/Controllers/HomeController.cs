using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using System.IO;
using FileOperationInAzure.DAL;

namespace FileOperationInAzure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly MyAttachmentsContext _context;

        public HomeController(IConfiguration configuration, MyAttachmentsContext context)
        {
            _configuration = configuration;
             _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetfileName() {
            string fileconnectionstring = _configuration["Azure:FileConnectionString:connectionstring"];

            BlobServiceClient blobService = new BlobServiceClient(fileconnectionstring);
            BlobContainerClient blobContainer = blobService.GetBlobContainerClient("myfolder");
           
            return Ok($" Your folder exist in this container : {blobContainer.AccountName}");
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFileIntoAzureContainer([FromForm] IFormFile file) {
           
            string fileconnectionstring = _configuration["Azure:FileConnectionString:connectionstring"];
            BlobServiceClient blobService = new BlobServiceClient(fileconnectionstring);
            BlobContainerClient blobContainer = blobService.GetBlobContainerClient("myfolder");

            BlobClient blobClient = blobContainer.GetBlobClient(file.FileName);
            var memory = new MemoryStream();
            await file.CopyToAsync(memory);
            memory.Position = 0;

            var result = await blobClient.UploadAsync(memory, true);
            memory.Close();

            FILE_OPERAION_INFO document = new FILE_OPERAION_INFO()
            {
                UPDATE_DATE = DateTime.Now,
                FILE_NAME = file.FileName,
                FILE_PATH = blobClient.Uri.AbsoluteUri
            };

            try
            {
                _context.FILE_OPERAION_INFO.Add(document);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
           

            return Ok(document.ID);
           
        }
        [HttpDelete("DeleteFile/{filename}")]
        public async Task<IActionResult> DeleteFileFromAzureContainer(string filename) {
           
            string fileconnectionstring = _configuration["Azure:FileConnectionString:connectionstring"];
            BlobServiceClient blobService = new BlobServiceClient(fileconnectionstring);
            BlobContainerClient blobContainer = blobService.GetBlobContainerClient("myfolder");

            var result = blobContainer.DeleteBlobIfExists(filename);

         

            //result
            return Ok();
        }
    }
}
