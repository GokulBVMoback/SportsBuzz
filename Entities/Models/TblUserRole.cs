using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TblUserRole
{
    public int UserRoleId { get; set; }

    public string? UserRole { get; set; }

    public virtual ICollection<TblUser> TblUsers { get; } = new List<TblUser>();
}
