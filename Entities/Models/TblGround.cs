using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TblGround
{
    public int GroundId { get; set; }

    public string? CompanyName { get; set; }

    public string? Venue { get; set; }

    public string? City { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public int? SportType { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? Active { get; set; }

    public virtual TblSportType? SportTypeNavigation { get; set; }

    public virtual ICollection<TblBookGround> TblBookGrounds { get; } = new List<TblBookGround>();

    public virtual TblUser? User { get; set; }
}
