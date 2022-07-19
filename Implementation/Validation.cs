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

        public async Task<List<Paygroup>> GetPaygroups(Input input,Company company, List<Paygroup> paygroups)
        {
            var paygroupResult = paygroups.Where(payGroup => payGroup.CompanyId == company.Id).ToList();
            if (paygroupResult == null)
            {
                _logger.LogInformation($"No Paygroups is found for the company {company.Name}");
                Console.WriteLine($"No Paygroups with is found for the company {company.Name}");
            }
            return paygroupResult;
        }

        public async Task<List<PayRun>> GetPayRuns(Input input, List<Paygroup> paygroups,List<PayRun> payruns)
        {
            List<PayRun> newpayruns = new List<PayRun>();
            foreach (var payRun in payruns)
            {
                foreach (var paygroup in paygroups)
                {
                    if (payRun.PayGroupId == paygroup.Id)
                    {
                        newpayruns.Add(payRun);
                    }
                }
            }
            if (newpayruns == null)
            {
                _logger.LogInformation($"No PayRuns with startDate {input.PayPeriodStartDate} and with endDate {input.PayPeriodEndDate} is found");
                Console.WriteLine($"No PayRuns with startDate {input.PayPeriodStartDate} and with endDate {input.PayPeriodEndDate} is found");
            }
            return newpayruns;
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

        public async Task<List<Output>> GenerateOutputModel(DateTime startTime, List<Timesheet> timesheets)
        {
            List<Output> outputs = new List<Output>();
            foreach (var timesheet in timesheets)
            {
                double Sum = 0;
                foreach (var value in timesheet.Values)
                {
                    Sum += value.Value;
                }
                outputs.Add(new Output() { PayRunId = timesheet.PayRunId, StartTime = startTime, SumValue = Sum });
            }
            return outputs;
        }
    }
}
