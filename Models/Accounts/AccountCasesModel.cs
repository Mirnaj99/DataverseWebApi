using ConsoleApp2.Models.Cases;
using DataverseWebApis.Models.Contacts;

namespace DataverseWebApis.Models.Accounts
{
    public class AccountCasesModel
    {
        public string accountid { get; set; }

        public string name { get; set; }

        public List<UserCasesModel> userCases { get; set; }

    }
}
