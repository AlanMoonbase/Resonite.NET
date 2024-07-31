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
        /// Log Into A Resonite User With The Provided LoginInfo Object. This Will Set The "CurrentAuthToken" Property.
        /// </summary>
        /// <param name="info"></param>
        /// <returns>
        /// A Completed Task Containing The Current User Session.
        /// </returns>
        public Task<UserSession> LoginAsync(LoginInfo info);

        /// <summary>
        /// Get The User With The Provided UserID.
        /// </summary>
        /// <param name="userId">The UserID Of The User You Want To Get. (Example - "U-{ID}")</param>
        /// <returns>The Completed Task Containing The Retreived User (User Has Empty Params If Not Found).</returns>
        public Task<User?> GetUserAsync(string userId);
    }
}
