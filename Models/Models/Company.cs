using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatacomConsole.Models.Models
{
    internal class Company
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
        
    }
}
