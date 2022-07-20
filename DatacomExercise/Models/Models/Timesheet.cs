using System.Collections.Generic;


namespace DatacomConsole.Models.Models
{
    public class Timesheet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int PayRunId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public List<TimesheetValue> Values { get; set; }
    }
}
