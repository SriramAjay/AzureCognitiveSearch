using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileOperationInAzure.DAL
{
    public class FILE_OPERAION_INFO
    {
        public int ID { get; set; }
        public string FILE_PATH { get; set; }
        public string FILE_NAME { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
    }
}
