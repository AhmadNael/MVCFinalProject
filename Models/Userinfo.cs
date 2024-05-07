using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Userinfo
{
    public decimal Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? Gender { get; set; }

    public string? ImgPath { get; set; }

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Visa> Visas { get; set; } = new List<Visa>();
}
