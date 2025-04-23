using System;
using System.Collections.Generic;

namespace ProjectPRN222.Models;

public partial class UserLogin
{
    public string UserId { get; set; } = null!;

    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }
}
