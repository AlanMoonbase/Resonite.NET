using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Resonite.NET.Core.Models;
using Resonite.NET.SignalR.Schema;

namespace Resonite.NET.SignalR.Client
{
    public interface IResoniteWebsocketClient
    {
        /// <summary>
        /// Current SignalR Hub Connection
        /// </summary>
        public HubConnection HubConnection { get; set; }

        /// <summary>
        /// Current Users Status
        /// </summary>
        public UserStatus CurrentUserStatus { get; set; }

        /// <summary>
        /// Start Connection To Resonites SignalR Hub Using Microsofts SignalR Client And An Existing User Session
        /// </summary>
        /// <returns>A ``bool`` Indicating Whether Or Not The Client Is Connected</returns>
        public Task<bool> StartAsync(UserSession userSession, string initialStatus = "", string appVersion = "");

        /// <summary>
        /// Set The User Status To The Specified Type (Refer To UserStatusType Class For Type Constants)
        /// </summary>
        /// <param name="statusType">The Type Of Status To Set The User To</param>
        /// <returns>Completed Task</returns>
        public Task SetUserStatus(string statusType);

        /// <summary>
        /// Stop The Connection To The SignalR Hub If Client Is Connected
        /// </summary>
        public Task StopAsync();
    }
}
