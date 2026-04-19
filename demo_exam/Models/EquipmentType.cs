using System;
using System.Collections.Generic;

namespace demo_exam.Models;

public partial class EquipmentType
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    public override string ToString()
    {
        return $"{Type}";
    }
}
