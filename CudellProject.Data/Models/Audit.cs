using System;

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
