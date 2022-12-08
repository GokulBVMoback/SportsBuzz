using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbModels
{
    public class GroundList
    {
        public int GroundId { get; set; }

        public string? CompanyName { get; set; }

        public string? Venue { get; set; }

        public string? City { get; set; }

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }

        public string? SportType { get; set; }

        public int? UserId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? Active { get; set; }
    }
}
