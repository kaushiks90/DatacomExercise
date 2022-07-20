using System;


namespace DatacomConsole.Models.Models
{
    public class TimesheetValue
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string PayComponentCode { get; set; }
        public double Value { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CostBoxId { get; set; }
        public string Task { get; set; }
    }
}
