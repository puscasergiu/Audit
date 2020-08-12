
using System.Collections.Generic;
using System.Threading.Tasks;
using Audit.DAL.Models;
using Audit.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Audit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }
        // GET: api/Audit/table
        [HttpGet("{table}")]
        public async Task<IEnumerable<AuditLog>> Get(string table)
        {
            return await _auditService.GetLogsAsync(table);
        }
    }
}
