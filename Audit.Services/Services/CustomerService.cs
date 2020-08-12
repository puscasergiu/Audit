using System.Collections.Generic;
using System.Threading.Tasks;

using Audit.DAL;
using Audit.DAL.Models;

using Microsoft.EntityFrameworkCore;

namespace Audit.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AuditDBContext _auditDBContext;

        public CustomerService(AuditDBContext auditDBContext)
        {
            _auditDBContext = auditDBContext;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _auditDBContext.Customer.ToListAsync();
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            return await _auditDBContext.Customer.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            if (!(await ValidateCustomer(customer)))
            {
                throw new System.Exception("A customer with the same email already exists");
            }

            _auditDBContext.Customer.Add(customer);

            await _auditDBContext.SaveChangesAsync();

            return customer;
        }

        private async Task<bool> ValidateCustomer(Customer customer)
        {
            Customer existingCustomer = await _auditDBContext.Customer.FirstOrDefaultAsync(c => c.Email == customer.Email);

            return existingCustomer == null;
        }

        public async Task EditCustomerAsync(int customerId, Customer customer)
        {
            Customer existingCustomer = await _auditDBContext.Customer.FirstOrDefaultAsync(c => c.CustomerId == customerId);

            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.BirthDate = customer.BirthDate;

            await _auditDBContext.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            Customer existingCustommer = await _auditDBContext.Customer.FirstOrDefaultAsync(c => c.CustomerId == customerId);

            _auditDBContext.Customer.Remove(existingCustommer);

            await _auditDBContext.SaveChangesAsync();
        }

    }
}
