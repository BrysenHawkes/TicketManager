﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket_Manager.Models;

namespace Ticket_Manager.ViewModels
{
    public class TicketIndexViewModel
    {
        public IEnumerable<ListTicketViewModel> Tickets { get; set; }
    }
}
