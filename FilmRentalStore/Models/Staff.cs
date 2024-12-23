using System;
using System.Collections.Generic;

namespace FilmRentalStore.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int AddressId { get; set; }

    public string? Email { get; set; }

    public int StoreId { get; set; }

    public bool Active { get; set; }

    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public DateTime LastUpdate { get; set; }

    public string? UrlPath { get; set; }

<<<<<<< HEAD
    public int? RoleId { get; set; }

=======
>>>>>>> origin/FilmRentalStore-4
    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public virtual Role? Role { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual Store? StoreNavigation { get; set; }
}
