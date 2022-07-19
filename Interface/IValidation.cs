using DatacomConsole.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatacomConsole.Interface
{
    interface IValidation
    {
        Task<Company> GetCompany(Input input,List<Company> companies);
        Task<List<PayRun>> GetPayRuns(Input input, List<PayRun> payruns);
        Task<List<Timesheet>> GetTimesheets(List<Timesheet> timesheets,List<PayRun> payruns);
    }
}
