using Resonite.NET.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resonite.NET.SignalR.Schema
{
    public class MarkMessageReadRequest
    {
        [JsonPropertyName("senderId")]
        public string SenderId { get; set; } = string.Empty;
        [JsonPropertyName("readTime")]
        public DateTime ReadTime { get; set; }
        [JsonPropertyName("ids")]
        public string[] MessageIds { get; set; } = Array.Empty<string>();
    }
}
