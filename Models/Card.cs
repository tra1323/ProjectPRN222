﻿using System;
using System.Collections.Generic;

namespace ProjectPRN222.Models;

public partial class Card
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
