using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Status
{
    public decimal Id { get; set; }

    public string? StatusType { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
