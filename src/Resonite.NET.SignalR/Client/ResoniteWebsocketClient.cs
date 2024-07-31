using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Resonite.NET.Core.Models;
using Resonite.NET.SignalR.Schema;

namespace Resonite.NET.SignalR.Client
{
    public class ResoniteWebsocketClient : IResoniteWebsocketClient, IDisposable
    {
        public IHubConnectionBuilder ConnectionSettings = new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .AddJsonProtocol();

        public HubConnection HubConnection { get; set; }

        public UserStatus CurrentUserStatus { get; set; } = new UserStatus();

        public async Task<bool> StartAsync(UserSession userSession, string initialStatus = "Online", string appVersion = "Resonite.NET Bot")
        {
            if (HubConnection == null)
            {
                Log("Building Connection");

                // configure connection settings url and authorization header
                ConnectionSettings.WithUrl("https://api.resonite.com/hub", options =>
                {
                    options.Headers = new Dictionary<string, string>
                    {
                        { "Authorization", $"res {userSession.UserId}:token={userSession.Token}" },
                        { "UID", $"{Guid.NewGuid()}" }
                    };
                });
                // create connection
                HubConnection = ConnectionSettings.Build();

                Log("Registering Events");
                // register events
                HubConnection.Closed += HubConnection_Closed; // only doing closed for now

                Log("Starting Connection");
                // start it
                await HubConnection.StartAsync();
                Log($"Connected As {userSession.UserId}");

                Log("Setting Current User Status");
                CurrentUserStatus = new UserStatus()
                {
                    UserId = userSession.UserId,
                    OnlineStatus = initialStatus,
                    OutputDevice = "Unknown",
                    SessionType = "Bot",
                    UserSessionId = userSession.UserId,
                    IsPresent = true,
                    LastPrecenseTimeStamp = DateTime.UtcNow,
                    LastStatusChange = DateTime.UtcNow,
                    CompatibilityHash = "resonitenet",
                    AppVersion = appVersion,
                    IsMobile = false
                };

                Log("Starting 'BroadcastStatus' Loop");
                RecurringTask(() => BroadcastStatus(), 30, new CancellationTokenSource().Token);
                Log("Ready");
            }
            else throw new InvalidOperationException("Connection Already Started");

            // return connection state
            return HubConnection.State == HubConnectionState.Connected;
        }

        private Task BroadcastStatus()
        {
            // set datetime properties to now
            CurrentUserStatus.LastStatusChange = DateTime.UtcNow;
            CurrentUserStatus.LastPrecenseTimeStamp = DateTime.UtcNow;

            // set status to current user status
            HubConnection.SendAsync("BroadcastStatus", CurrentUserStatus, new UserGroup
            {
                Group = 1,
                TargetIds = null
            });

            Log("BroadcastStatus");

            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            if (HubConnection != null)
            {
                if (HubConnection.State == HubConnectionState.Connected || HubConnection.State == HubConnectionState.Connecting)
                {
                    Log("Stopping Connection");
                    // stop hub connection
                    await HubConnection.StopAsync();
                }
                else throw new InvalidOperationException("Attempted To Stop A Connection That's Already Disconnected");
            }
            else throw new InvalidOperationException("Hub Connection Not Initialized");
        }

        public async void Dispose()
        {
            if (HubConnection != null)
            {
                if (HubConnection.State == HubConnectionState.Connected || HubConnection.State == HubConnectionState.Connecting)
                {
                    Log("Stopping Connection");
                    // stop connection before disposing to ensure proper events on server side
                    await HubConnection.StopAsync();
                }

                Log("Disposing Client");

                // deregister events (probably not needed but oh well)
                HubConnection.Closed -= HubConnection_Closed;

                // dispose of the connection
                await HubConnection.DisposeAsync();
            }
            else throw new InvalidOperationException("Hub Connection Is Not Initialized Or Connection Is Not Disposable");
        }

        private void Log(string message)
        {
            Console.WriteLine($"[SignalR] [{DateTime.UtcNow}] " + message);
        }

        static void RecurringTask(Action action, int seconds, CancellationToken token)
        {
            if (action == null) return;

            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    action();
                    await Task.Delay(TimeSpan.FromSeconds(seconds), token);
                }
            });
        }

        // EVENTS

        private Task HubConnection_Closed(Exception? arg)
        {
            if(arg != null)
            {
                Log("Connection Closed With Exception - " + arg.Message);
            }

            return Task.CompletedTask;
        }
    }
}
