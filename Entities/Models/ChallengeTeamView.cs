using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class ChallengeTeamView
{
    public int ChallengeId { get; set; }

    public string? Team1 { get; set; }

    public long? PhoneNumT1 { get; set; }

    public DateTime? Date { get; set; }

    public int? SessionId { get; set; }

    public TimeSpan? Session { get; set; }

    public int SportTypeId { get; set; }

    public string? SportType { get; set; }

    public string? Team2 { get; set; }

    public long? PhoneNumT2 { get; set; }

    public bool? Status { get; set; }
}
