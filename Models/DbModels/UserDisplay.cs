using Entities.Models;
using System;
using System.Collections.Generic;

namespace Models.DbModels;

public class UserDisplay
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public long? PhoneNum { get; set; }

    public string? UserRole { get; set; }

}
