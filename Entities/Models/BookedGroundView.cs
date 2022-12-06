using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class BookedGroundView
{
    public int BookedId { get; set; }

    public string? TeamName { get; set; }

    public string? Venue { get; set; }

    public string? City { get; set; }

    public long? PhoneNum { get; set; }

    public DateTime? Date { get; set; }

    public int SportTypeId { get; set; }

    public string? SportType { get; set; }

    public int? SessionId { get; set; }

    public TimeSpan? Session { get; set; }
}
