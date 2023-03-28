using System;
using System.Collections.Generic;

namespace online_store.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public int? AddressId { get; set; }

    public string? Role { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Product> Products { get; } = new List<Product>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();
}
