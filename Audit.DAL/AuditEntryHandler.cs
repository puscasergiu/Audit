using System.Collections.Generic;
using System.Threading.Tasks;
using Audit.DAL.Infrastructure;
using Audit.DAL.Models;

namespace Audit.DAL
{
    public class AuditEntryHandler
    {
        public async Task HandleAsync(List<AuditEntry> entries, AuditDBContext dBContext)
        {
            List<AuditLog> logs = new List<AuditLog>();

            foreach (AuditEntry entry in entries)
            {
                logs.Add(entry.ToAuditLog());
            }

            dBContext.AuditLog.AddRange(logs);

            await dBContext.SaveChangesAsync();
        }
    }
}
