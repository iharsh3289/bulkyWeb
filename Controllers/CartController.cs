using System.Security.Claims;
using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.Models;
using Bulky_Web.Models.ViewModels;
using Bulky_Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Bulky_Web.Controllers;
[Authorize]
public class CartController  : Controller {
    private readonly IUnitOfWork _unitOfWork ;
    [BindProperty]
    public ShoppingCartVM ShoppingCartVM { get; set; }
    public CartController(IUnitOfWork UnitOfWork){
        _unitOfWork=UnitOfWork;
    }
    public IActionResult Index(){   
        
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        ShoppingCartVM = new()
        {
            ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
            OrderHeader = new OrderHeader()
        };

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        
        return View(ShoppingCartVM);
    }
    
    public IActionResult Summary(){   
        
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        ShoppingCartVM = new()
        {
            ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
            OrderHeader = new OrderHeader()
        };

        ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

        ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
        ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
        ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
        ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
        ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        
        return View(ShoppingCartVM);
    }
    [HttpPost]
    [ActionName("Summary")]
    public IActionResult SummaryPOST(){   
        
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
            includeProperties: "Product");

        ApplicationUser ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        ShoppingCartVM.OrderHeader.ApplicationUserId = userId;
        ShoppingCartVM.OrderHeader.OrderDate=DateTime.Now;
        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        if (ApplicationUser.CompanyId.GetValueOrDefault() == 0)
        {
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
        }
        else
        {
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
        }
        
        _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
        _unitOfWork.Save();

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            OrderDetail orderDetail = new()
            {
                ProductId = cart.ProductId,
                OrderHeaderId = ShoppingCartVM.OrderHeader.OrderHeaderId,
                Count = cart.Count,
                Price = cart.Price
            };
            _unitOfWork.OrderDetail.Add(orderDetail);
            _unitOfWork.Save();
        }

        if (ApplicationUser.CompanyId.GetValueOrDefault() == 0)
        {
            var domain = "http://localhost:5205/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain+$"/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.OrderHeaderId}",
                CancelUrl = domain+$"/cart/Index",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                var SessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price*100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                    
                };
                options.LineItems.Add(SessionLineItem);

            }
            var service = new Stripe.Checkout.SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.OrderHeaderId,session.Id,session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location",session.Url);
            return new StatusCodeResult(303);
        }
        
        
        return RedirectToAction(nameof(OrderConfirmation),new {id=ShoppingCartVM.OrderHeader.OrderHeaderId});
    }

    public IActionResult OrderConfirmation(int id)
    {
        OrderHeader orderHeader =
            _unitOfWork.OrderHeader.Get(u => u.OrderHeaderId == id, includeProperties: "ApplicationUser");
        if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
        {
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStripePaymentId(id,session.Id,session.PaymentIntentId);
                _unitOfWork.OrderHeader.UpdateStatus(id,SD.StatusApproved,SD.PaymentStatusApproved);
                _unitOfWork.Save();
            }
            
        }

        List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
            .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
        _unitOfWork.ShoppingCart.removeRange(shoppingCarts);
        _unitOfWork.Save();
        return View(id);
    }
    public IActionResult plus(int id)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ShoppingCartId == id);
        cartFromDb.Count += 1;
        _unitOfWork.ShoppingCart.Update(cartFromDb);
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }
    
    public IActionResult minus(int id)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ShoppingCartId == id);
        if (cartFromDb.Count <= 1)
        {
            _unitOfWork.ShoppingCart.Remove(cartFromDb);

        }
        else
        {
            cartFromDb.Count -= 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
        }
        
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }
    
    public IActionResult remove(int id)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ShoppingCartId == id);
        _unitOfWork.ShoppingCart.Remove(cartFromDb);
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
    {
        if (shoppingCart.Count <= 50)
        {
             return shoppingCart.Product.Price;
        }
        else
        {
            if (shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }
        
    }
}