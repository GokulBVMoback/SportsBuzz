using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbModels
{
    public class GroundRegister
    {
        public string? CompanyName { get; set; }

        public string? Venue { get; set; }

        public string? City { get; set; }

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }

        public int? SportType { get; set; }

        public int? UserId { get; set; }
    }
}
