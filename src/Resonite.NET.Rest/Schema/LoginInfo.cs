using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resonite.NET.Rest.Schema
{
    public class LoginInfo
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        [JsonPropertyName("authentication")]
        public Authentication Authentication { get; set; } = new Authentication();
        [JsonPropertyName("secretMachineId")]
        public string SecretMachineId { get; set; } = string.Empty;
        [JsonPropertyName("rememberMe")]
        public bool RememberMe { get; set; }
    }
}
