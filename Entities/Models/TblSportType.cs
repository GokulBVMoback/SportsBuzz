using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TblSportType
{
    public int SportTypeId { get; set; }

    public string? SportType { get; set; }

    public virtual ICollection<TblGround> TblGrounds { get; } = new List<TblGround>();

    public virtual ICollection<TblTeam> TblTeams { get; } = new List<TblTeam>();
}
