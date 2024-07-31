using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resonite.NET.Rest.Schema
{
    public class Authentication
    {
        [JsonPropertyName("$type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
