using System;
using System.Collections.Generic;

namespace MVCFinalProject.Models;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string IngredientName { get; set; } = null!;

    public string? MeasuringTool { get; set; }

    public decimal? Amount { get; set; }
}
