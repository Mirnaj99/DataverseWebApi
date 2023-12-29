namespace DataverseWebApis.Models.Customers
{
    public class CustomersListModel
    {
        public string ODataContext { get; set; }
        public List<CustomerObjectModel> Value { get; set; }
    }
}
