using System.Collections.Generic;
using System.Threading.Tasks;

using Audit.DAL.Models;
using Audit.Services.Services;

using Microsoft.AspNetCore.Mvc;

namespace Audit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _customerService.GetCustomersAsync();
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<Customer> Get(int id)
        {
            return await _customerService.GetCustomerAsync(id);
        }

        [HttpPost]
        public async Task<Customer> Post([FromBody] Customer customer)
        {
            return await _customerService.AddCustomerAsync(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
        {
            await _customerService.EditCustomerAsync(id, customer);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.DeleteCustomerAsync(id);

            return Ok();
        }
    }
}
