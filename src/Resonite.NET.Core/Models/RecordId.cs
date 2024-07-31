using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resonite.NET.Core.Models
{
    public class RecordId
    {
        /// <summary>
        /// The ID Of The Record (Example: "R-{UUID}")
        /// </summary>
        public string Id { get; } = string.Empty;

        /// <summary>
        /// The Owner ID Of The Record (Example: "R-{UUID}")
        /// </summary>
        public string OwnerId { get; } = string.Empty;
    }
}
