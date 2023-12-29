using DataverseWebApis.Helpers;
using DataverseWebApis.Models.Customers;
using DataverseWebApis.Models.Errors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace DataverseWebApis.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string authority;
        private readonly string resource;
        private readonly string apiUrl;
        private readonly IConfiguration configuration;
        public CustomersService()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appSettings.json")
                .Build();
            this.clientId = configuration["ClientId"];
            this.clientSecret = configuration["ClientSecret"];
            this.authority = configuration["Authority"];
            this.resource = configuration["Resource"];
            this.apiUrl = configuration["ApiUrl"];
        }
        public async Task PostCustomers(AddCustomerModel model)
        {
            try
            {
                DataverseAuthentication dataverseAuth = new DataverseAuthentication(clientId, clientSecret, authority, resource);
                String accessToken = await dataverseAuth.GetAccessToken();

                Console.WriteLine($"Access Token: {accessToken}");
               
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Example: Retrieve records from a Dataverse entity
               
                    var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    HttpResponseMessage postresponse = await httpClient.PostAsync(apiUrl + "m365_customerses", content);

                    if (!postresponse.IsSuccessStatusCode)
                    {
                        var json = await postresponse.Content.ReadAsStringAsync();

                        try
                        {
                            var errorObject = JObject.Parse(json);
                            var err = errorObject.ToObject<ErrorModel>();

                            Console.WriteLine(err.error.message);
                        

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                        }

                    }
                    else
                    {
                        Console.WriteLine("Success!");
                    }

                }
            }

            catch (Exception ex)
            {
                throw new AppException(ex.Message, "err000", ex.InnerException);

            }


        }

        public async Task<List<CustomerObjectModel>> GetCustomers()
        {
            try
            {
                DataverseAuthentication dataverseAuth = new DataverseAuthentication(clientId, clientSecret, authority, resource);
                String accessToken = await dataverseAuth.GetAccessToken();

                Console.WriteLine($"Access Token: {accessToken}");
                Console.WriteLine($"\n");
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Example: Retrieve records from a Dataverse entity


                    HttpResponseMessage getresponse = await httpClient.GetAsync(apiUrl + "accounts");
                    var json = await getresponse.Content.ReadAsStringAsync();
                    if (!getresponse.IsSuccessStatusCode)
                    {
                        try
                        {
                            var errorObject = JObject.Parse(json);
                            var err = errorObject.ToObject<ErrorModel>();

                            Console.WriteLine(err.error.message);
                           

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                        }
                        return null;
                    }
                    else
                    {

                        Console.WriteLine(json);
                        //var customerObject = JObject.Parse(json);
                        //var customersList = customerObject["value"].ToObject<List<CustomerObjectModel>>();

                        //foreach (var customer in customersList)
                        //{
                        //    // Access properties of each GetCustomersModel object
                        //    Console.WriteLine($"Customer Name: {customer.m365_name}");
                        //    Console.WriteLine($"Customer ID: {customer.m365_customersid}");
                        //    Console.WriteLine($"Customer TenantID: {customer.m365_tenantid}");
                        //    Console.WriteLine($"\n");

                        //    // Add other properties as needed
                        //}


                        return null;

                    }

                }
            }

            catch (Exception ex)
            {
                throw new AppException(ex.Message, "err000", ex.InnerException);

            }


        }
    }
}
