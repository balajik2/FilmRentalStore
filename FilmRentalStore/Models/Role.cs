using System;
using System.Collections.Generic;

namespace FilmRentalStore.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Ruser> Rusers { get; set; } = new List<Ruser>();


    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

}
