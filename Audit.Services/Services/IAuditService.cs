using System.Collections.Generic;
using System.Threading.Tasks;
using Audit.DAL.Models;

namespace Audit.Services.Services
{
    public interface IAuditService
    {
        Task<IEnumerable<AuditLog>> GetLogsAsync(string table);
    }
}