using System;
using System.Collections.Generic;

namespace demo_exam.Models;

public partial class User
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public string? FullName { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual UserRole? Role { get; set; }
}
