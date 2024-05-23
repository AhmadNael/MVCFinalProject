using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class UserRequest
{
    public decimal UserRequestId { get; set; }

    public decimal? UserId { get; set; }

    public decimal? RequestId { get; set; }

    public virtual Request? Request { get; set; }

    public virtual Userinfo? User { get; set; }
}
