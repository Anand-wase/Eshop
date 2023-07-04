using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopping.WebApi.Models;
public class UpdateProductDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }

    [StringLength(50)]
    [Required]
    public string ProductName { get; set; }
    [Required]
    public double Price { get; set; }
    [StringLength(300)]
    public string? ProductDescription { get; set; } = null!;
    [Required]
    public int AvailableQuantity { get; set; }
    [Required]
    public int CategoryId { get; set; }

    [StringLength(100)]
    public string? ImageUrl { get; set; }
}
