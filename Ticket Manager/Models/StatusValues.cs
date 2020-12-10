using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket_Manager.Models
{
    public enum StatusValues
    {
        Unnassigned,
        Assigned,
        In_Progress,
        Resolved,
        Closed,
        Cancelled
    }
}
