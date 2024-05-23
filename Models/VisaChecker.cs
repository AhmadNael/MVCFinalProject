using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class VisaChecker
{
    public string? CardHolderName { get; set; }

    public byte? Cvc { get; set; }

    public string? CardNumber { get; set; }

    public decimal? Balance { get; set; }
}
