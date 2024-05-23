using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Contactu
{
    public decimal Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }
}
