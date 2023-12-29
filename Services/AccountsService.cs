using ConsoleApp2.Models.Cases;
using DataverseWebApis.Helpers;
using DataverseWebApis.Models.Accounts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace DataverseWebApis.Services
{
    public class AccountsService : IAccountService
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string authority;
        private readonly string resource;
        private readonly string apiUrl;
        private readonly IConfiguration configuration;
        public AccountsService()
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

        public async Task<List<AccountCasesModel>> GetAccountsCases()
        {
            try
            {
                List<AccountCasesModel> accountCases = new List<AccountCasesModel>();
                DataverseAuthentication dataverseAuth = new DataverseAuthentication(clientId, clientSecret, authority, resource);
                String accessToken = await dataverseAuth.GetAccessToken();

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "accounts");

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var jsonObject = JsonConvert.DeserializeObject<JObject>(json);
                        if (jsonObject != null)
                        {
                            var valueArray = jsonObject["value"];
                            if (valueArray != null)
                            {
                                var accounts = valueArray.ToObject<List<AccountModel>>();

                                foreach (AccountModel account in accounts)
                                {
                                    AccountCasesModel accountCase = new AccountCasesModel();

                                    try
                                    {
                                        HttpResponseMessage getCases = await httpClient.GetAsync(apiUrl + "incidents?$filter=_customerid_value eq " + account.accountid);

                                        if (getCases.StatusCode == HttpStatusCode.OK)
                                        {
                                            var casesJson = await getCases.Content.ReadAsStringAsync();
                                            var casesjsonObject = JsonConvert.DeserializeObject<JObject>(casesJson);

                                            if (casesjsonObject != null)
                                            {
                                                var caseArray = casesjsonObject["value"];
                                                if (caseArray != null)
                                                {
                                                    var cases = caseArray.ToObject<List<GetCasesModel>>();
                                                    accountCase.accountid = account.accountid;
                                                    accountCase.name = account.name;
                                                    accountCase.casesModel = cases;
                                                }
                                            }

                                            accountCases.Add(accountCase);
                                        }
                                        else
                                        {
                                            throw new HttpRequestException($"Failed to get cases for account {account.accountid}. Status code: {getCases.StatusCode}");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new HttpRequestException($"Error while getting cases for account {account.accountid}.", ex);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new HttpRequestException($"Failed to get accounts. Status code: {response.StatusCode}");
                    }
                }

                return accountCases;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, "err000", ex.InnerException);
            }
        }


    }
}

