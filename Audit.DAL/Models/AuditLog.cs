using System;

namespace Audit.DAL.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }

        public int UserId { get; set; }
        public string TableName { get; set; }
        public DateTime Date { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
    }
}
