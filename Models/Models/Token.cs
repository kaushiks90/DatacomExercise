﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatacomConsole.Models.Models
{
    internal class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
