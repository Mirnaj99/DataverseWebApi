using DataverseWebApis.Models.Customers;

namespace DataverseWebApis.Services
{
    public interface ICustomersService
    {
        Task<List<CustomerObjectModel>> GetCustomers();

        Task PostCustomers(AddCustomerModel model);
    }
}
