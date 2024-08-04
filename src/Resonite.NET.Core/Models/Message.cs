using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resonite.NET.Core.Models
{
    public class Message
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("ownerId")]
        public string OwnerId { get; set; } = string.Empty;

        [JsonPropertyName("recipientId")]
        public string RecipientId { get; set; } = string.Empty;

        [JsonPropertyName("senderId")]
        public string SenderId { get; set; } = string.Empty;

        [JsonPropertyName("senderUserSessionId")]
        public string SenderUserSessionId { get; set; } = string.Empty;

        [JsonPropertyName("messageType")]
        public string MessageType { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("sendTime")]
        public DateTime SendTime { get; set; }

        [JsonPropertyName("lastUpdateTime")]
        public DateTime LastUpdateTime { get; set; }

        [JsonPropertyName("readTime")]
        public DateTime? ReadTime { get; set; }

        [JsonPropertyName("isMigrated")]
        public bool IsMigrated { get; set; }
    }
}
