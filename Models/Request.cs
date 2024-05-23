using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Request
{
    public decimal RequestId { get; set; }

    public DateTime? RequsetDate { get; set; }

    public decimal? RequestTax { get; set; }

    public decimal? RecipeId { get; set; }

    public decimal? UserId { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual Userinfo? User { get; set; }
}
