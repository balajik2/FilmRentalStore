using System;
using System.Collections.Generic;

namespace FilmRentalStore.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int AddressId { get; set; }

<<<<<<< HEAD
<<<<<<< HEAD
    public byte[]? Picture { get; set; }

=======
>>>>>>> c9f394a110474532bb4f8cbdea417adb7c59f041
=======
>>>>>>> origin/FilmRentalStore-3
    public string? Email { get; set; }

    public int StoreId { get; set; }

    public bool Active { get; set; }

    public DateTime LastUpdate { get; set; }

<<<<<<< HEAD
<<<<<<< HEAD
=======
    public string? UrlPath { get; set; }

    public int? RoleId { get; set; }

>>>>>>> c9f394a110474532bb4f8cbdea417adb7c59f041
=======
    public string? UrlPath { get; set; }

>>>>>>> origin/FilmRentalStore-3
    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

<<<<<<< HEAD
=======
    public virtual Role? Role { get; set; }

>>>>>>> c9f394a110474532bb4f8cbdea417adb7c59f041
    public virtual Store Store { get; set; } = null!;

    public virtual Store? StoreNavigation { get; set; }
}
