using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Client.Data
{
    internal static class Cash
    {
        internal static Guid UserId { get; set; } = Guid.Empty;
        internal static Guid CurrentInterlocutor { get; set; } = Guid.Empty;
    }
}
