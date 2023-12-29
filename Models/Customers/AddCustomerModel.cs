namespace DataverseWebApis.Models.Customers
{
    public class AddCustomerModel
    {
        public string m365_clientid { get; set; }
        public string m365_tenantid { get; set; }
        public string m365_name { get; set; }
        public string m365_clientsecret { get; set; }
    }
}
