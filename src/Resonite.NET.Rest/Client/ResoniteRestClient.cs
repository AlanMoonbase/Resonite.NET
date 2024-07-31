using Resonite.NET.Rest.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Resonite.NET.Core.Models;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Resonite.NET.Rest.Client
{
    public class ResoniteRestClient : IResoniteRestClient
    {
        public UserSession CurrentUserSession { get; set; } = new UserSession();

        public RestClientOptions RequestOptions = new RestClientOptions("https://api.resonite.com");

        public async Task<UserSession> LoginAsync(LoginInfo loginInfo)
        {
            var client = new RestClient(RequestOptions);
            RestRequest request = new RestRequest("userSessions");
            request.AddBody(loginInfo);
            request.AddHeader("UID", GenerateSha256("GabbaGoo"));
            RestResponse response = await client.PostAsync(request);

            UserSession? userSession = null;
            if (response != null && response.IsSuccessStatusCode && response.Content != null)
                userSession = JsonDocument.Parse(response.Content).RootElement.GetProperty("entity").Deserialize<UserSession>();
            else userSession = new UserSession();

            if (userSession != null) CurrentUserSession = userSession;

            return CurrentUserSession;
        }

        public async Task<User?> GetUserAsync(string userId)
        {
            var client = new RestClient(RequestOptions);
            if (CurrentUserSession != new UserSession()) client.AddDefaultHeader("Authorization", $"res {CurrentUserSession.UserId}:{CurrentUserSession.Token}");
            User? retreivedUser = await client.GetAsync<User>($"users/{userId}");
            if (retreivedUser != null) return retreivedUser;
            else return new User(); // return empty user
        }

        private string GenerateSha256(string text)
        {
            var sb = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(text));
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
