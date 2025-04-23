using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ProjectPRN222.Models;

public partial class User : IdentityUser
{
    public string? FullName { get; set; } = null!;

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Notification> NotificationCreateByNavigations { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationSendToNavigations { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
