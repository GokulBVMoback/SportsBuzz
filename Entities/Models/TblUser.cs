using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public long? PhoneNum { get; set; }

    public string? Password { get; set; }

    public int? UserRole { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? Active { get; set; }

    public virtual TblUserRole? UserRoleNavigation { get; set; }
}
