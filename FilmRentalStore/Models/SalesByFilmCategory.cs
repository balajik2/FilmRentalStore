﻿using System;
using System.Collections.Generic;

namespace FilmRentalStore.Models;

public partial class SalesByFilmCategory
{
    public string Category { get; set; } = null!;

    public decimal? TotalSales { get; set; }
}
