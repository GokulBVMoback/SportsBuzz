using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PaginationParams
    {
        public int Count { get; set; }
        public int Cursor { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
    }
}
