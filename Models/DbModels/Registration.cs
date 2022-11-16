using System;
using System.Collections.Generic;

namespace Entities.Models;

public class Registration:TblUser
{
    public string? ConfirmPassword { get; set; }
}
