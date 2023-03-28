using System;
using System.Collections.Generic;

namespace online_store.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string? StreetNumber { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public string? HomeNumber { get; set; }

    public string? ApartmentNumber { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
