﻿using System;
using System.Collections.Generic;

namespace ProjectPRN222.Models;

public partial class UserClaim
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }
}
