using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket_Manager.Models;

namespace Ticket_Manager.ViewModels
{
    public class TicketIndexViewModel
    {
        public List<ProjectViewModel> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
