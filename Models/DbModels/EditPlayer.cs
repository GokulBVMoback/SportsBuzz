using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbModels
{
    public class EditPlayer
    {
        public int MemberId { get; set; }

        public string? PlayerFirstName { get; set; }

        public string? PlayerLastName { get; set; }

        public int? Age { get; set; }

        public int? JerseyNo { get; set; }
    }
}
