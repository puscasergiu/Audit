using System;
using Audit.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Audit.DAL.Infrastructure
{
    public static class AuditEntryExtensions
    {
        public static AuditLog ToAuditLog(this AuditEntry auditEntry)
        {
            AuditLog audit = new AuditLog()
            {
                TableName = auditEntry.Entry.Metadata.GetTableName(),
                Date = DateTime.UtcNow,
                KeyValues = JsonConvert.SerializeObject(auditEntry.KeyValues),
                NewValues = auditEntry.NewValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.NewValues),
                OldValues = auditEntry.OldValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.OldValues),
            };

            return audit;
        }
    }
}
