using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Testimonial
{
    public short TestId { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? Content { get; set; }

    public decimal? UserId { get; set; }

    public virtual Userinfo? User { get; set; }
}
