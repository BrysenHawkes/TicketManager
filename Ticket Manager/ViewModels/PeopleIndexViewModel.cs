using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket_Manager.ViewModels
{
    public class PeopleIndexViewModel
    {
        public IEnumerable<ListPeopleViewModel> PeopleInProject;
        public string JoinId;
    }
}
