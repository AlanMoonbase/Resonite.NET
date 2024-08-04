using System.Text.Json.Serialization;

namespace Resonite.NET.Core.Models
{
    public class SessionUser
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("userID")]
        public string UserID { get; set; } = string.Empty;

        [JsonPropertyName("userSessionId")]
        public string UserSessionId { get; set; } = string.Empty;

        [JsonPropertyName("isPresent")]
        public bool IsPresent { get; set; }

        [JsonPropertyName("outputDevice")]
        public OutputDevice? OutputDevice { get; set; }
    }
}