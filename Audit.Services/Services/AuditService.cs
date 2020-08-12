using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Audit.DAL;
using Audit.DAL.Models;

using Microsoft.EntityFrameworkCore;

namespace Audit.Services.Services
{
    public class AuditService : IAuditService
    {
        private readonly AuditDBContext _auditDBContext;

        public AuditService(AuditDBContext auditDBContext)
        {
            _auditDBContext = auditDBContext;
        }

        public async Task<IEnumerable<AuditLog>> GetLogsAsync(string table)
        {
            return await _auditDBContext.AuditLog.Where(log => log.TableName.ToLower() == table.ToLower()).ToListAsync();
        }
    }
}
