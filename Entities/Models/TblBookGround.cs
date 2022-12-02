using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TblBookGround
{
    public int BookedId { get; set; }

    public int? TeamId { get; set; }

    public int? SessionId { get; set; }

    public DateTime? Date { get; set; }

    public int? GroundId { get; set; }

    public virtual TblGround? Ground { get; set; }

    public virtual TblSession? Session { get; set; }

    public virtual TblTeam? Team { get; set; }
}
