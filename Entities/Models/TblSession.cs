using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TblSession
{
    public int SessionId { get; set; }

    public TimeSpan? Session { get; set; }

    public virtual ICollection<TblBookGround> TblBookGrounds { get; } = new List<TblBookGround>();
}
