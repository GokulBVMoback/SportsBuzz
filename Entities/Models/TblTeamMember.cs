using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TblTeamMember
{
    public int MemberId { get; set; }

    public string? PlayerFirstName { get; set; }

    public string? PlayerLastName { get; set; }

    public int? Age { get; set; }

    public int? JerseyNo { get; set; }

    public string? State { get; set; }

    public int? TeamId { get; set; }

    public virtual TblTeam? Team { get; set; }
}
