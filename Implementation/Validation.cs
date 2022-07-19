using DatacomConsole.Interface;
using DatacomConsole.Models.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatacomConsole.Implementation
{
    class Validation : IValidation
    {
        private readonly ILogger<Validation> _logger;
        public Validation(ILogger<Validation> logger)
        {
            _logger = logger;
        }
        public async Task<Company> GetCompany(Input input, List<Company> companies)
        {
            var companyResult = companies.Where(comp => comp.Name.ToLower() == input.CompanyCode.ToLower()).FirstOrDefault();
            if (companyResult == null)
            {
                _logger.LogInformation($"Entered Company code {input.CompanyCode} is not found");
                Console.WriteLine($"Entered Company code {input.CompanyCode} is not found");
            }
            return companyResult;
        }

        public async Task<List<PayRun>> GetPayRuns(Input input, List<PayRun> payruns)
        {
            var payRunResult = payruns.Where(payRun => payRun.PayPeriod.StartDate == input.PayPeriodStartDate && payRun.PayPeriod.EndDate == input.PayPeriodEndDate).ToList();
            if (payRunResult == null)
            {
                _logger.LogInformation($"No PayRuns with startDate {input.PayPeriodStartDate} and with endDate {input.PayPeriodEndDate} is found");
                Console.WriteLine($"No PayRuns with startDate {input.PayPeriodStartDate} and with endDate {input.PayPeriodEndDate} is found");
            }
            return payRunResult;
        }

        public async Task<List<Timesheet>> GetTimesheets(List<Timesheet> timesheets, List<PayRun> payruns)
        {
            List<Timesheet> newtimesheets = new List<Timesheet>();
            foreach (var payRun in payruns)
            {
                foreach (var timeSheet in timesheets)
                {
                    if(timeSheet.PayRunId==payRun.Id && timeSheet.Type.ToUpper() == "API")
                    {
                        newtimesheets.Add(timeSheet);
                    }
                }
            }
            if (newtimesheets.Count() == 0)
            {
                _logger.LogInformation($"No timesheets found");
                Console.WriteLine($"No timesheets found");
            }
            return newtimesheets;
        }
    }
}
