using System;

namespace Audit.DAL.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public int ChangeType { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string PrimaryKey { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
