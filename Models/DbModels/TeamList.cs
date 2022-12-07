using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbModels
{
    public  class TeamList : TblTeam
    {
        public new string? SportType { get; set; }  
        public string? FirstName { get; set; }
        public string? LastName { get; set;}

    }
}
