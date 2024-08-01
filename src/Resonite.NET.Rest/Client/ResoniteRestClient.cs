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
            Log("Resonite.NET V0.0.1");
            Log("Generating Machine ID");

            string machineId = GenerateRandomMachineId();

            Log($"Generated Machine ID '{machineId}'");
            Log("Generating UID");

            string uid = GenerateUID(machineId);

            Log($"Generated UID '{uid}'");

            var client = new RestClient(RequestOptions);
            RestRequest request = new RestRequest("userSessions");
            request.AddBody(loginInfo);
            request.AddHeader("UID", GenerateUID(machineId));
            RestResponse response = await client.PostAsync(request);

            UserSession? userSession = null;
            if (response != null && response.IsSuccessStatusCode && response.Content != null)
                userSession = JsonDocument.Parse(response.Content).RootElement.GetProperty("entity").Deserialize<UserSession>();
            else userSession = new UserSession();

            if (userSession != null) CurrentUserSession = userSession;

            CurrentUserSession.MachineId = machineId;
            CurrentUserSession.UID = uid;

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

        private static string GenerateRandomMachineId()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
            var random = new Random();
            var result = new char[128];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = characters[random.Next(characters.Length)];
            }
            return new string(result);
        }

        private static string GenerateUID(string machineId)
        {
            using (var sha256 = SHA256.Create())
            {
                var data = Encoding.UTF8.GetBytes("ResoniteNETApp-" + machineId);
                var hash = sha256.ComputeHash(data);
                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }

        private void Log(string message)
        {
            Console.WriteLine($"[REST] [{DateTime.UtcNow}] " + message);
        }
    }
}
