using Entities.Models;
using System;
using System.Collections.Generic;

namespace Models.DbModels;

public class UserDisplay:TblUser
{
    public new string? UserRole { get; set; }


}
