using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCFinalProject.Models;

public partial class Category
{
    public decimal CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryImg { get; set; }

    [NotMapped]
    public IFormFile? imgFile { get; set; }
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
