using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ProjectPRN222.Models;

public partial class User : IdentityUser
{
    public string? FullName { get; set; } = null!;
}
