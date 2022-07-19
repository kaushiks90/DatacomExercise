using System;
using System.Collections.Generic;
using System.Text;

namespace DatacomConsole.Models.Appsettings
{
    public class LoggingConfig
    {
        public string LogPath { get; set; }
        public string EnableLog { get; set; }
    }

    public class ApiEndPoint
    {
        public string CompaniesUrl { get; set; }
        public string PayRunsUrl { get; set; }
        public string TimesheetsUrl { get; set; }
    }

    public class AccessToken
    {
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
    }
}
