using System;
using System.Collections.Generic;

namespace demo_exam.Models;

public partial class Equipment
{
    public int Id { get; set; }

    public string? Article { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public int? SupplierId { get; set; }

    public int? ManufacturerId { get; set; }

    public int? TypeId { get; set; }

    public double? Discount { get; set; }

    public double? Count { get; set; }

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public int? UnitId { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Supplier? Supplier { get; set; }

    public virtual EquipmentType? Type { get; set; }

    public virtual Unit? Unit { get; set; }
}
