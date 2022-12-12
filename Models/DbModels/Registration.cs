using System;
using System.Collections.Generic;

namespace Entities.Models;

public class Registration
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public long? PhoneNum { get; set; }

    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }


    public int? UserRole { get; set; }
}
