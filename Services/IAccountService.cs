using DataverseWebApis.Models.Accounts;

namespace DataverseWebApis.Services
{
    public interface IAccountService
    {
        Task<List<AccountCasesModel>> GetAccountsCases();
    }
}
