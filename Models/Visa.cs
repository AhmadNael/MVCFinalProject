using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Visa
{
    public long CardNumber { get; set; }

    public byte Cvc { get; set; }

    public string CardName { get; set; } = null!;

    public string BillingAdrress { get; set; } = null!;

    public DateTime? ExpDate { get; set; }

    public decimal? Amount { get; set; }

    public decimal? UserId { get; set; }

    public virtual Userinfo? User { get; set; }
}
