using DatacomConsole.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatacomConsole
{
    interface IRunService
    {
        Task RunAsync();
        Task<Token> GetAccessToken();
        Task<List<Company>> GetCompanyDetail();
        Task<List<Paygroup>> GetPayGroupsDetail();
        Task<List<PayRun>> GetPayRuns();
        Task<List<Timesheet>> GetTmeSheets();
        void GenerateCSV(List<Output> outputs);
        
    }
}
