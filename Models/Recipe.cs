using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCFinalProject.Models;

public partial class Recipe
{
    public decimal RecipeId { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? RecipeImg { get; set; }

    public string? Name { get; set; }

    public decimal? CategoryId { get; set; }

    public decimal? UserId { get; set; }

    public decimal? StatusId { get; set; }

    public string? RecipeInfo { get; set; }

    [NotMapped]
    public IFormFile? imgFile { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Status? Status { get; set; }

    public virtual Userinfo? User { get; set; }
}
