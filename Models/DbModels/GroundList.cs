﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbModels
{
    public class GroundList:TblGround
    {
        public new string? SportType { get; set; }
    }
}
