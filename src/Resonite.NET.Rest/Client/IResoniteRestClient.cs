using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resonite.NET.Core.Models;
using Resonite.NET.Rest.Schema;

namespace Resonite.NET.Rest.Client
{
    public interface IResoniteRestClient
    {
        public UserSession CurrentUserSession { get; set; }

        /// <summary>
        /// Logs Into The Resonite API
        /// </summary>
        /// <param name="username">The Username Of The User You Wish To Sign In As</param>
        /// <param name="password">The Password Of The User You Wish To Sign In As</param>
        /// <returns>Completed Task With The Current User Session</returns>
        public Task<UserSession> LoginAsync(string username, string password);

        /// <summary>
        /// Get The User With The Provided UserID.
        /// </summary>
        /// <param name="userId">The UserID Of The User You Want To Get. (Example - "U-{ID}")</param>
        /// <returns>The Completed Task Containing The Retreived User (User Has Empty Params If Not Found).</returns>
        public Task<User?> GetUserAsync(string userId);
    }
}
