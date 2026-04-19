using System;
using System.Collections.Generic;

namespace demo_exam.Models;

public partial class Order
{
    public int Id { get; set; }

    public double? Number { get; set; }

    public int? EquipmentId { get; set; }

    public double? Days { get; set; }

    public DateTime? StartDate { get; set; }

    public int? PickupPointId { get; set; }

    public int? UserId { get; set; }

    public double? Code { get; set; }

    public int? StatusId { get; set; }

    public virtual Equipment? Equipment { get; set; }

    public virtual PickupPoint? PickupPoint { get; set; }

    public virtual Status? Status { get; set; }

    public virtual User? User { get; set; }
}
