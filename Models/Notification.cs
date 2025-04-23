using System;
using System.Collections.Generic;

namespace ProjectPRN222.Models;

public partial class Notification
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? SendTo { get; set; }

    public virtual User? CreateByNavigation { get; set; }

    public virtual User? SendToNavigation { get; set; }
}
