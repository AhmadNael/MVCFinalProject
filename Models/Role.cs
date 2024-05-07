using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Role
{
    public decimal RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();
}
