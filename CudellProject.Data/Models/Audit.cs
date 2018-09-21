using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CudellProject.Data.Models
{
    public class Audit
    {
        public int AuditID { get; set; }
        public DateTime OperationTimeStamp { get; set; }
        public string Operation { get; set; }
        public string Username { get; set; }
    }
}
