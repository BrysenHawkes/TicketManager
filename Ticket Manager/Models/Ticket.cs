using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket_Manager.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName ("Reported")]
        public DateTime ReportedDate { get; set; }

        [DisplayName ("Reporter")]
        public string ReportedBy { get; set; }

        [DisplayName("Due")]
        public DateTime DueDate { get; set; }

        [DisplayName ("Assignment")]
        public string AssignedTo { get; set; }

        [Required]
        [DisplayName("Priority")]
        public string Priority { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
        public int ProjectID { get; set; }
    }
}
