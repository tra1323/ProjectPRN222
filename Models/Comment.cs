using System;
using System.Collections.Generic;

namespace ProjectPRN222.Models;

public partial class Comment
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? UserId { get; set; }

    public DateTime? CreateAt { get; set; }

    public int? ProductId { get; set; }

    public int? ParentId { get; set; }

    public virtual ICollection<Comment> InverseParent { get; set; } = new List<Comment>();

    public virtual Comment? Parent { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
