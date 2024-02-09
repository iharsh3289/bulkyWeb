using System.Diagnostics;
using System.Security.Claims;
using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulky_Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork ;

    public HomeController(ILogger<HomeController> logger,IUnitOfWork UnitOfWork)
    {
        _logger = logger;
        _unitOfWork=UnitOfWork;
    }

    public IActionResult Index(){   
        
        List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
        return View(objProductList);
    }
    
    public IActionResult Details(int id)
    {
        ShoppingCart cart = new()
        {
            Product = _unitOfWork.Product.Get(u => u.ProductId == id, includeProperties: "Category"),
            Count = 1,
            ProductId = id
        };
        return View(cart);
    }
    
    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart ShoppingCart)
    {

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        ShoppingCart.ApplicationUserId = userId;

        ShoppingCart cartFromdb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
                                                                    u.ProductId == ShoppingCart.ProductId);
        if (cartFromdb != null)
        {
            cartFromdb.Count += ShoppingCart.Count;
            _unitOfWork.ShoppingCart.Update(cartFromdb);
        }
        else
        {
            _unitOfWork.ShoppingCart.Add(ShoppingCart);
        }
        _unitOfWork.Save();
        
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
