using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket_Manager.Models
{
    public class UserProject
    {
        public UserProject(int projectID, string userID)
        {
            this.ProjectId = projectID;
            this.UserId = userID;
        }
        public UserProject() { }
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
