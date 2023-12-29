using ConsoleApp2.Models.Cases;

namespace DataverseWebApis.Models.Accounts
{
    public class AccountCasesModel
    {
        public string accountid { get; set; }

        public string name { get; set; }

        public List<GetCasesModel> casesModel { get; set; }
    }
}
