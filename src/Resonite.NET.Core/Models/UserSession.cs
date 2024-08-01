using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resonite.NET.Core.Models
{
    public class UserSession
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = string.Empty;
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        public string MachineId {  get; set; } = string.Empty;
        public string UID { get; set; } = string.Empty;
    }
}
