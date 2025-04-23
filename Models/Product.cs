using System;
using System.Collections.Generic;

namespace ProjectPRN222.Models;

public partial class Product
{

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public int? ManufacturerId { get; set; }

    
    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Manufacture? Manufacturer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
