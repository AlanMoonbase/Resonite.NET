using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resonite.NET.SignalR.Schema
{
    public class UserStatus
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = string.Empty;
        [JsonPropertyName("onlineStatus")]
        public string OnlineStatus { get; set; } = string.Empty;
        [JsonPropertyName("outputDevice")]
        public string OutputDevice { get; set; } = string.Empty;
        [JsonPropertyName("sessionType")]
        public string SessionType { get; set; } = string.Empty;
        [JsonPropertyName("userSessionId")]
        public string UserSessionId { get; set; } = string.Empty;
        [JsonPropertyName("isPresent")]
        public bool IsPresent { get; set; }
        [JsonPropertyName("lastPresenceTimestamp")]
        public DateTime LastPrecenseTimeStamp { get; set; } = new DateTime();
        [JsonPropertyName("lastStatusChange")]
        public DateTime LastStatusChange { get; set; } = new DateTime();
        [JsonPropertyName("compatibilityHash")]
        public string CompatibilityHash {  get; set; } = string.Empty;
        [JsonPropertyName("appVersion")]
        public string AppVersion { get; set; } = string.Empty;
        [JsonPropertyName("isMobile")]
        public bool IsMobile { get; set; }
    }
}
