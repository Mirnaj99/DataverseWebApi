using ConsoleApp2.Models.Cases;
using DataverseWebApis.Helpers;
using DataverseWebApis.Models.Accounts;
using DataverseWebApis.Models.Contacts;
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
                                        HttpResponseMessage getUsers = await httpClient.GetAsync(apiUrl + "contacts?$filter=_parentcustomerid_value eq " + account.accountid);

                                        if (getUsers.StatusCode == HttpStatusCode.OK)
                                        {
                                            var usersJson = await getUsers.Content.ReadAsStringAsync();
                                            var usersJsonObject = JsonConvert.DeserializeObject<JObject>(usersJson);

                                            if (usersJsonObject != null)
                                            {
                                                var userArray = usersJsonObject["value"];
                                                if (userArray != null)
                                                {
                                                    var users = userArray.ToObject<List<UserCasesModel>>();

                                                    accountCase.accountid = account.accountid;
                                                    accountCase.name = account.name;
                                                    accountCase.userCases = new List<UserCasesModel>();

                                                    foreach (var user in users)
                                                    {
                                                        try
                                                        {
                                                            // Get cases for the user
                                                            HttpResponseMessage getCases = await httpClient.GetAsync(apiUrl + $"incidents?$filter=_primarycontactid_value eq {user.contactid}");

                                                            if (getCases.StatusCode == HttpStatusCode.OK)
                                                            {
                                                                var casesJson = await getCases.Content.ReadAsStringAsync();
                                                                var casesJsonObject = JsonConvert.DeserializeObject<JObject>(casesJson);

                                                                if (casesJsonObject != null)
                                                                {
                                                                    var caseArray = casesJsonObject["value"];
                                                                    if (caseArray != null)
                                                                    {
                                                                        var cases = caseArray.ToObject<List<GetCasesModel>>();

                                                                        UserCasesModel userCase = new UserCasesModel
                                                                        {
                                                                            contactid = user.contactid,
                                                                            fullname = user.fullname,
                                                                            cases = cases
                                                                        };

                                                                        accountCase.userCases.Add(userCase);
                                                                    }
                                                                    else
                                                                    {
                                                                        // User doesn't have cases, add to the userCases array with empty cases
                                                                        UserCasesModel userCase = new UserCasesModel
                                                                        {
                                                                            contactid = user.contactid,
                                                                            fullname = user.fullname,
                                                                            cases = new List<GetCasesModel>()
                                                                        };

                                                                        accountCase.userCases.Add(userCase);
                                                                    }
                                                                }
                                                                accountCases.Add(accountCase);
                                                            }
                                                            else
                                                            {
                                                                throw new HttpRequestException($"Failed to get cases for user {user.contactid}. Status code: {getCases.StatusCode}");
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            throw new HttpRequestException($"Error while getting cases for user {user.contactid}.", ex);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            throw new HttpRequestException($"Failed to get users for account {account.accountid}. Status code: {getUsers.StatusCode}");
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



