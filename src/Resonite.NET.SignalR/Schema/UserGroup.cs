using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resonite.NET.SignalR.Schema
{
    public class UserGroup
    {
        [JsonPropertyName("group")]
        public int Group { get; set; }
        [JsonPropertyName("targetIds")]
        public int[]? TargetIds { get; set; } // ???
    }
}
