using System.Collections.Generic;
using System.Threading.Tasks;
using Audit.DAL.Models;

namespace Audit.Services.Services
{
    public interface ICustomerService
    {
        Task DeleteCustomerAsync(int customerId);
        Task EditCustomerAsync(int customerId, Customer customer);
        Task<Customer> GetCustomerAsync(int customerId);
        Task<IEnumerable<Customer>> GetCustomersAsync();

        Task<Customer> AddCustomerAsync(Customer customer);
    }
}