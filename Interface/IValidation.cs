using DatacomConsole.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatacomConsole.Interface
{
    public interface IValidation
    {
        Task<Company> GetCompany(Input input, List<Company> companies);
        Task<List<Paygroup>> GetPaygroups(Company company, List<Paygroup> paygroups);
        Task<List<PayRun>> GetPayRuns(Input input, List<Paygroup> paygroups, List<PayRun> payruns);
        Task<List<Timesheet>> GetTimesheets(List<Timesheet> timesheets, List<PayRun> payruns);
        Task<List<Output>> GenerateOutputModel(DateTime startTime, List<Timesheet> timesheets);
    }
}
