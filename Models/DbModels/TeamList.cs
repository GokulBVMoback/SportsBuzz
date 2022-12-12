using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbModels
{
    public  class TeamList
    {
        public int TeamId { get; set; }

        public string? TeamName { get; set; }
     
        public string? City { get; set; }
        
        public string? SportType { get; set; }
        
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }
        
        public string? Email { get; set; }

        public long? PhoneNum { get; set; }

        public int? UserId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? Active { get; set; }

    }
}
