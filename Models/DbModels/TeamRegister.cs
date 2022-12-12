using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbModels
{
    public class TeamRegister
    {
        public string? TeamName { get; set; }

        public string? City { get; set; }

        public int? SportType { get; set; }

        public string? Email { get; set; }

        public long? PhoneNum { get; set; }

        public int? UserId { get; set; }
    }
}
