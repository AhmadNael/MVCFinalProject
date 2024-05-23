using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Testimonial
{
    public string? Content { get; set; }

    public decimal? UserId { get; set; }

    public decimal? StatusId { get; set; }

    public DateTimeOffset? CreationDate { get; set; }

    public decimal TestId { get; set; }

    public virtual Status? Status { get; set; }

    public virtual Userinfo? User { get; set; }
}
