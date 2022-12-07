using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class GroundView
{
    public int GroundId { get; set; }

    public string? CompanyName { get; set; }

    public string? Venue { get; set; }

    public string? City { get; set; }

    public string? SportType { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? Active { get; set; }
}
