using Resonite.NET.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resonite.NET.SignalR.Events
{
    public class SessionUpdateReceivedEventArgs : EventArgs
    {
        public SessionInfo Session {  get; set; }
    }
}
