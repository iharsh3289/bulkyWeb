using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky_Web.Models;

public class ShoppingCart
{
    public int ShoppingCartId { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; }
    [Range(1,1000,ErrorMessage = "Please Enter a Value Between 1 to 1000")]
    public int Count { get; set; }
    
    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
    
    [NotMapped]
    public double Price { get; set; }
      
}