using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TblTeam
{
    public int TeamId { get; set; }

    public string? TeamName { get; set; }

    public string? City { get; set; }

    public int? SportType { get; set; }

    public string? Email { get; set; }

    public long? PhoneNum { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? Active { get; set; }

    public virtual TblSportType? SportTypeNavigation { get; set; }

    public virtual ICollection<TblTeamMember> TblTeamMembers { get; } = new List<TblTeamMember>();

    public virtual TblUser? User { get; set; }
}
