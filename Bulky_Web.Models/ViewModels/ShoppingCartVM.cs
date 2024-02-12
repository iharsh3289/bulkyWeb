using System.Collections;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky_Web.Models.ViewModels;

public class ShoppingCartVM{
    
    public IEnumerable<ShoppingCart> ShoppingCartList {get;set;}
    public OrderHeader OrderHeader { get; set; }
}
