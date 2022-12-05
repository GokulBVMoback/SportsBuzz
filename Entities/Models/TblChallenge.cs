using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TblChallenge
{
    public int ChallengeId { get; set; }

    public int? TeamId1 { get; set; }

    public int? SessionId { get; set; }

    public DateTime? Date { get; set; }

    public int? TeamId2 { get; set; }

    public int? SportType { get; set; }

    public bool? Status { get; set; }

    public virtual TblSession? Session { get; set; }

    public virtual TblSportType? SportTypeNavigation { get; set; }

    public virtual TblTeam? TeamId1Navigation { get; set; }

    public virtual TblTeam? TeamId2Navigation { get; set; }
}
