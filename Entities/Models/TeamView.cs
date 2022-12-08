using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TeamView
{
    public int TeamId { get; set; }

    public string? TeamName { get; set; }

    public string? City { get; set; }

    public string? SportType { get; set; }

    public string? Email { get; set; }

    public long? PhoneNum { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? Active { get; set; }
}
