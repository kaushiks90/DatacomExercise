using System;
using System.Collections.Generic;
using System.Text;

namespace DatacomConsole.Models.Models
{
    class Timesheet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int PayRunId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public List<TimesheetValue> Values { get; set; }
    }
}
