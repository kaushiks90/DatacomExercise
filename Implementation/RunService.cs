using CsvHelper;
using DatacomConsole.Interface;
using DatacomConsole.Models.Appsettings;
using DatacomConsole.Models.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DatacomConsole
{
    class RunService : IRunService
    {
        private readonly ApiEndPoint _apiEndPointConfig;
        private readonly AccessToken _accessTokenEndPointConfig;
        private readonly IRestUtility _restUtility;
        private readonly ILogger<RunService> _logger;
        private readonly IValidation _validation;
        private readonly IConfiguration _config;
        private string token;
        Input inputModel = null;

        public RunService(IRestUtility restUtility,
            IConfiguration config,
                                  ILogger<RunService> logger,
                                  ApiEndPoint apiEndPointConfig,
                                  AccessToken accessTokenEndPointConfig,
                                   IValidation validation)
        {
            _restUtility = restUtility;
            _logger = logger;
            _apiEndPointConfig = apiEndPointConfig;
            _accessTokenEndPointConfig = accessTokenEndPointConfig;
            _config = config;
            _validation = validation;
            inputModel = new Input();
        }

        public async Task RunAsync()
        {
            CollectInput();
            List<Output> outputResults = await GetData();
            GenerateCSV(outputResults);
        }

        private async Task<List<Output>> GetData()
        {
            var accessToken = await GetAccessToken();
            token = accessToken.AccessToken;

            var companies = await GetCompanyDetail();
            var company = await _validation.GetCompany(inputModel, companies);

            var paygroups = await GetPayGroupsDetail();
            var filteredPayGroups = await _validation.GetPaygroups(inputModel, company, paygroups);

            var payRuns = await GetPayRuns();
            var filteredpayRuns = await _validation.GetPayRuns(inputModel, filteredPayGroups, payRuns);

            var timeSheets = await GetTmeSheets();
            var filteredTimesheets = await _validation.GetTimesheets(timeSheets, filteredpayRuns);

            var outputResults = await _validation.GenerateOutputModel(inputModel.PayPeriodStartDate, filteredTimesheets);
            return outputResults;
        }

        private void CollectInput()
        {

            Console.WriteLine("Enter the CompanyCode");
            inputModel.CompanyCode = Console.ReadLine();
            Console.WriteLine("Enter the Pay Period start date- YYYY-MM-DD");
            var startDate = Console.ReadLine().Split('-');
            inputModel.PayPeriodStartDate = new DateTime(Convert.ToInt32(startDate[0]), Convert.ToInt32(startDate[1]), Convert.ToInt32(startDate[2]));
            Console.WriteLine("Enter the Pay Period end date- YYYY-MM-DD");
            var endDate = Console.ReadLine().Split('-');
            inputModel.PayPeriodEndDate = new DateTime(Convert.ToInt32(endDate[0]), Convert.ToInt32(endDate[1]), Convert.ToInt32(endDate[2]));
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

        public async Task<List<Paygroup>> GetPayGroupsDetail()
        {
            string url = $"{_config["BaseEndpoint"]}{_apiEndPointConfig.PayGroupsUrl}";
            var response = await _restUtility.GetAsync<Paygroup>(url, token);
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

        public void GenerateCSV(List<Output> outputs)
        {
            string folderPath = _config["CSVFilePath"];
            string fileName = $"Timesheetdetails{Regex.Replace(DateTimeOffset.Now.ToString().Split('+')[0].Trim(), @"[^0-9a-zA-Z]+", "_")}";
            using (var writer = new StreamWriter($"{folderPath}{fileName}.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(outputs);
            }
        }

    }
}
