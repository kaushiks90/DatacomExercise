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
        }

        public async Task RunAsync()
        {
            var res = await GetAccessToken();
            var res1=await GetCompanyDetail();
        }

        public void CollectInput()
        {
            //ValidateInput
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
            token = tokenResponse.AccessToken;
            return tokenResponse;
        }
        public async Task<List<Company>> GetCompanyDetail()
        {
            string url = $"{_config["BaseEndpoint"]}{_apiEndPointConfig.CompaniesUrl}";
            var response = await _restUtility.GetAsync<Company>(url, token);
            return response;
        }

        public void GetPayRuns()
        {

        }

        public void GetTmeSheets()
        {

        }

        public void CreateCSV()
        {

        }
    }
}
