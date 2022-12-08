using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TeamMemberView
{
    public int MemberId { get; set; }

    public string? TeamName { get; set; }

    public string? PlayerFirstName { get; set; }

    public string? PlayerLastName { get; set; }

    public int? Age { get; set; }

    public int? JerseyNo { get; set; }

    public string? State { get; set; }
}
