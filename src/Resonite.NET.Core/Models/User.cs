using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resonite.NET.Core.Models
{
    public class User
    {
        /// <summary>
        /// The ID Of The User
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The Username Of The User
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The Normalized Username Of The User
        /// </summary>
        [JsonPropertyName("normalizedUsername")]
        public string NormalizedUsername { get; set; } = string.Empty;

        /// <summary>
        /// The Email Of The User (Only Populated If User Is Same As Logged In User)
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; } = string.Empty;

        /// <summary>
        /// The Registration Date Of The User
        /// </summary>
        [JsonPropertyName("registrationDate")]
        public DateTime RegistrationDate { get; set; } = new DateTime();

        /// <summary>
        /// Indicates Whether The User Is Verified
        /// </summary>
        [JsonPropertyName("isVerified")]
        public bool IsVerified { get; set; }

        /// <summary>
        /// Indicates Whether The User Is Locked
        /// </summary>
        [JsonPropertyName("isLocked")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// Indicates Whether The User Has Ban Evasion Supression
        /// </summary>
        [JsonPropertyName("supressBanEvasion")]
        public bool SupressBanEvasion { get; set; }

        /// <summary>
        /// Whether Two Factor Authentication Is Enabled On The User
        /// </summary>
        [JsonPropertyName("2fa_login")]
        public bool TwoFALogin { get; set; }

        /// <summary>
        /// The Profile Data Of The User
        /// </summary>
        [JsonPropertyName("profile")]
        public ProfileData Profile { get; set; } = new ProfileData();
    }
}
