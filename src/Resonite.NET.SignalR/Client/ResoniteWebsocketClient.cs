using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Resonite.NET.Core.Models;
using Resonite.NET.SignalR.Events;
using Resonite.NET.SignalR.Schema;

namespace Resonite.NET.SignalR.Client
{
    public class ResoniteWebsocketClient : IResoniteWebsocketClient, IDisposable
    {
        public IHubConnectionBuilder ConnectionSettings = new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .ConfigureLogging(logging =>
            {
                logging.AddDebug();
                logging.SetMinimumLevel(LogLevel.Error);
            })
            .AddJsonProtocol();

        public HubConnection HubConnection { get; set; }

        public UserStatus CurrentUserStatus { get; set; } = new UserStatus();

        public async Task<bool> StartAsync(UserSession userSession, string initialStatus = UserStatusType.Online, string appVersion = "Resonite.NET 0.0.1")
        {
            Log($"Resonite.NET SignalR Client V0.0.1 Is Starting Up");

            if (HubConnection == null)
            {
                Log("Building Connection");

                // configure connection settings url and authorization header
                ConnectionSettings.WithUrl("https://api.resonite.com/hub", options =>
                {
                    options.Headers = new Dictionary<string, string>
                    {
                        { "Authorization", $"res {userSession.UserId}:{userSession.Token}" },
                        { "UID", userSession.UID }
                    };
                });
                // create connection
                HubConnection = ConnectionSettings.Build();

                Log("Registering Events");
                // register events
                HubConnection.Closed += HubConnection_Closed; // only doing closed for now

                HubConnection.On<string>("Debug", (dbgmsg) => Debug.WriteLine(dbgmsg));
                HubConnection.On<Message>("ReceiveMessage", async (msg) =>
                {
                    // always mark message as read
                    await HubConnection.InvokeAsync("MarkMessagesRead", new MarkMessageReadRequest
                    {
                        SenderId = msg.SenderId,
                        ReadTime = DateTime.UtcNow,
                        MessageIds = new string[] { msg.Id }
                    });

                    OnMessageReceived(new MessageReceivedEventArgs { Message = msg });
                });
                HubConnection.On<SessionInfo>("ReceiveSessionUpdate", (session) =>
                {
                    OnSessionUpdateReceived(new SessionUpdateReceivedEventArgs { Session = session });
                });

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

                RecurringTask(() => BroadcastStatus(), 30, new CancellationTokenSource().Token);
                Log("Ready");
            }
            else throw new InvalidOperationException("Connection Already Started");

            // return connection state
            return HubConnection.State == HubConnectionState.Connected;
        }

        public async Task SetUserStatus(string statusType)
        {
            // set current user stautus type
            CurrentUserStatus.OnlineStatus = statusType;

            // force status change
            await BroadcastStatus();
        }

        public async Task SendMessage(UserSession ownerSession, string userId, string content, string contentType)
        {
            // construct a message
            Message message = new Message
            {
                Id = "MSG-" + Guid.NewGuid().ToString(),
                Content = content,
                SenderId = ownerSession.UserId,
                RecipientId = userId,
                SendTime = DateTime.UtcNow,
                LastUpdateTime = DateTime.UtcNow,
                MessageType = contentType
            };
            
            // invoke "SendMessage" on server
            if (HubConnection != null && HubConnection.State == HubConnectionState.Connected)
            {
                await HubConnection.InvokeAsync("SendMessage", message);
            }
            else throw new InvalidOperationException("Hub Connection Not Initialized");
        }

        public async Task StopAsync()
        {
            if (HubConnection != null)
            {
                if (HubConnection.State == HubConnectionState.Connected || HubConnection.State == HubConnectionState.Connecting)
                {
                    // set user to offline
                    CurrentUserStatus.OnlineStatus = UserStatusType.Offline;
                    await BroadcastStatus();

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
                    // set user to offline
                    CurrentUserStatus.OnlineStatus = UserStatusType.Offline;
                    await BroadcastStatus();

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

        private Task BroadcastStatus()
        {
            // set datetime properties to now
            CurrentUserStatus.LastStatusChange = DateTime.UtcNow;
            CurrentUserStatus.LastPrecenseTimeStamp = DateTime.UtcNow;

            // set status to current user status
            HubConnection.InvokeAsync("BroadcastStatus", CurrentUserStatus, new UserGroup
            {
                Group = 1,
                TargetIds = null
            });

            return Task.CompletedTask;
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

        protected virtual void OnMessageReceived(MessageReceivedEventArgs msg)
        {
            EventHandler<MessageReceivedEventArgs> handler = MessageReceived;
            if (handler != null)
            {
                handler(this, msg);
            }
        }
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;


        protected virtual void OnSessionUpdateReceived(SessionUpdateReceivedEventArgs sessionUpdateReceivedEventArgs)
        {
            EventHandler<SessionUpdateReceivedEventArgs> handler = SessionUpdateReceived;
            if(handler != null)
            {
                handler(this, sessionUpdateReceivedEventArgs);
            }
        }
        public event EventHandler<SessionUpdateReceivedEventArgs> SessionUpdateReceived;
    }
}
