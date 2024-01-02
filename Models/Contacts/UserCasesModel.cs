using ConsoleApp2.Models.Cases;

namespace DataverseWebApis.Models.Contacts
{
    public class UserCasesModel
    {
        public string contactid {  get; set; }

        public string fullname { get; set; }

        public List<GetCasesModel> cases { get; set; }
    }
}
