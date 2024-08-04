using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resonite.NET.Core.Models
{
    public class SessionInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("correspondingWorldId")]
        public RecordId CorrespondingWorldId { get; set; } = new RecordId();

        [JsonPropertyName("tags")]
        public HashSet<string> Tags { get; set; } = new HashSet<string>();

        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("normalizedSessionId")]
        public string NormalizedSessionId {  get; set; } = string.Empty;

        [JsonPropertyName("hostUserId")]
        public string HostUserId { get; set; } = string.Empty;

        [JsonPropertyName("hostUserSessionId")]
        public string HostUserSessionId { get; set; } = string.Empty;

        [JsonPropertyName("hostMachineId")]
        public string HostMachineId { get; set; } = string.Empty;

        [JsonPropertyName("hostUsername")]
        public string HostUsername { get; set; } = string.Empty;

        [JsonPropertyName("compatibilityHash")]
        public string CompatibilityHash { get; set; } = string.Empty;

        [JsonPropertyName("systemCompatibilityHash")]
        public string SystemCompatibilityHash { get; set; } = string.Empty;

        [JsonPropertyName("universeId")]
        public string UniverseId { get; set; } = string.Empty;

        [JsonPropertyName("appVersion")]
        public string AppVersion { get; set; } = string.Empty;

        [JsonPropertyName("headlessHost")]
        public bool HeadlessHost { get; set; }

        [JsonPropertyName("sessionURLs")]
        public List<string> SessionURLs { get; set; } = new List<string>();

        [JsonPropertyName("parentSessionIds")]
        public List<string> ParentSessionIds { get; set; } = new List<string>();

        [JsonPropertyName("nestedSessionIds")]
        public List<string> NestedSessionIds { get; set; } = new List<string>();

        [JsonPropertyName("sessionUsers")]
        public List<SessionUser> SessionUsers { get; set; } = new List<SessionUser>();
    }
}
