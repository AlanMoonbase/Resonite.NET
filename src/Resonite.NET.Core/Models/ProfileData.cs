using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resonite.NET.Core.Models
{
    public class ProfileData
    {
        /// <summary>
        /// The URL Of The Profile's Icon
        /// </summary>
        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; } = string.Empty;

        /// <summary>
        /// List Of Token Outputs
        /// </summary>
        public List<string> TokenOutOut = new List<string>();

        /// <summary>
        /// List Of Display Badges (Record Ids)
        /// </summary>
        [JsonPropertyName("displayBadges")]
        public List<RecordId> DisplayBadges { get; set; } = new List<RecordId>();
    }
}
