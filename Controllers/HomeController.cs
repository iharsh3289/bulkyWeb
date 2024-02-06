using System.Diagnostics;
using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.Models;
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
    
    public IActionResult Details(int id){   
        
        Product obj = _unitOfWork.Product.Get( u=>u.ProductId==id ,  includeProperties:"Category");
        return View(obj);
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
