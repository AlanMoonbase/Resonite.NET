using Resonite.NET.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resonite.NET.SignalR.Events
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public Message Message { get; set; }
    }
}
