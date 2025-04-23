using System;
using System.Collections.Generic;

namespace ProjectPRN222.Models;

public partial class Order
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
