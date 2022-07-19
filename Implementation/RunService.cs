using DatacomConsole.Models.Appsettings;
using DatacomConsole.Models.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DatacomConsole
{
    class RunService : IRunService
    {
        private readonly ApiEndPoint _apiEndPointConfig;
        private readonly AccessToken _accessTokenEndPointConfig;
        private readonly IRestUtility _restUtility;
        private readonly ILogger<RunService> _logger;
        private readonly IConfiguration _config;
        private string token;
        Input inputModel = null;

        public RunService(IRestUtility restUtility,
            IConfiguration config,
                                  ILogger<RunService> logger,
                                  ApiEndPoint apiEndPointConfig,
                                  AccessToken accessTokenEndPointConfig)
        {
            _restUtility = restUtility;
            _logger = logger;
            _apiEndPointConfig = apiEndPointConfig;
            _accessTokenEndPointConfig = accessTokenEndPointConfig;
            _config = config;
            inputModel = new Input();
        }

        public async Task RunAsync()
        {
            var accessToken = await GetAccessToken();
            token = accessToken.AccessToken;

            var getCompanies = await GetCompanyDetail();


            var res2 = await GetPayRuns();

            var res3 = await GetTmeSheets();
        }

        public void CollectInput()
        {

            Console.WriteLine("Enter the CompanyCode");
            inputModel.CompanyCode = Console.ReadLine();
            Console.WriteLine("Enter the Pay Period start date- YYYY-MM-DD");
            inputModel.PayPeriodStartDate = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Enter the Pay Period end date- YYYY-MM-DD");
            inputModel.PayPeriodEndDate = Convert.ToDateTime(Console.ReadLine());
        }

        public async Task<Token> GetAccessToken()
        {
            string baseAddress = _config["AccessTokenEndpoint"]; ;
            var form = new Dictionary<string, string>
                {
                    {"grant_type", _accessTokenEndPointConfig.GrantType},
                    {"client_id", _accessTokenEndPointConfig.ClientId},
                    {"client_secret", _accessTokenEndPointConfig.SecretKey},
                };
            var tokenResponse = await _restUtility.PostRequestAsync<Token>(new FormUrlEncodedContent(form), baseAddress);
            return tokenResponse;
        }
        public async Task<List<Company>> GetCompanyDetail()
        {
            string url = $"{_config["BaseEndpoint"]}{_apiEndPointConfig.CompaniesUrl}";
            var response = await _restUtility.GetAsync<Company>(url, token);
            return response;
        }

        public async Task<List<PayRun>> GetPayRuns()
        {
            string url = $"{_config["BaseEndpoint"]}{_apiEndPointConfig.PayRunsUrl}";
            var response = await _restUtility.GetAsync<PayRun>(url, token);
            return response;
        }

        public async Task<List<Timesheet>> GetTmeSheets()
        {
            string url = $"{_config["BaseEndpoint"]}{_apiEndPointConfig.TimesheetsUrl}";
            var response = await _restUtility.GetAsync<Timesheet>(url, token);
            return response;
        }

        public void CreateCSV()
        {

        }
    }
}
