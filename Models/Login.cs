using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Login
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public decimal? UserId { get; set; }

    public decimal? RoleId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual Userinfo? User { get; set; }
}
