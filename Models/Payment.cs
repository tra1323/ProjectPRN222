using System;
using System.Collections.Generic;

namespace ProjectPRN222.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string? Content { get; set; }

    public decimal? Price { get; set; }

    public string? TransactionCode { get; set; }

    public string? Status { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }
}
