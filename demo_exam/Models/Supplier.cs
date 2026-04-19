using System;
using System.Collections.Generic;

namespace demo_exam.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
    public override string ToString()
    {
        return $"{Name}";
    }
}
