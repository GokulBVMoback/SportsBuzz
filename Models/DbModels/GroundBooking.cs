using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbModels
{
    public class GroundBooking
    {
        public int? TeamId { get; set; }

        public int? SessionId { get; set; }

        public DateTime? Date { get; set; }

        public int? GroundId { get; set; }
    }
}
