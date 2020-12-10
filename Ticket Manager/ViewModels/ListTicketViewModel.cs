using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket_Manager.ViewModels
{
    public class ListTicketViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public string Reported { get; set; }
        public string Assigned { get; set; }
        public string Due { get; set; }
        public string Status { get; set; }

    }
}
