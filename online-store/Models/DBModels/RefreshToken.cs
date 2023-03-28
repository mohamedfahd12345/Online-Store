using System;
using System.Collections.Generic;

namespace online_store.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken1 { get; set; }

    public bool? IsExpired { get; set; }

    public bool? IsUsed { get; set; }

    public bool? IsRevoked { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual User? User { get; set; }
}
