using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileOperationInAzure.DAL
{
    public partial class MyAttachmentsContext: DbContext
    {

        public MyAttachmentsContext()
        {
        }

        public MyAttachmentsContext(DbContextOptions<MyAttachmentsContext> options)
            : base(options)
        {
        }

        public DbSet<FILE_OPERAION_INFO> FILE_OPERAION_INFO { get; set; }
    }
}
