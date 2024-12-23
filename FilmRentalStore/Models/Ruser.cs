using System;
using System.Collections.Generic;

namespace FilmRentalStore.Models;

public partial class Ruser
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }
}
