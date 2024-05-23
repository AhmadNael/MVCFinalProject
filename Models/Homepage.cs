using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Homepage
{
    public decimal Id { get; set; }

    public string? WebsiteName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? HeroImgTitle { get; set; }

    public string? HeroImgContent { get; set; }

    public string? Email { get; set; }
}
