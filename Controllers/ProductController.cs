using Bulky_Web.DataAccess.Data;
using System.Diagnostics;
using Bulky_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Bulky_web.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky_Web.Models.ViewModels;

namespace Bulky_Web.Controllers ;
public class ProductController : Controller{
    private readonly IUnitOfWork _unitOfWork ;
    private readonly IWebHostEnvironment _webHostEnvironment ;
    public ProductController(IUnitOfWork UnitOfWork , IWebHostEnvironment webHostEnvironment){
        _unitOfWork=UnitOfWork;
        _webHostEnvironment=webHostEnvironment;
    }
    public IActionResult Index(){   
        
        List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
        return View(objProductList);
    }

    public IActionResult Upsert(int? id){

        // ViewBag.CategoryList=CategoryList;
        // ViewData["CategoryList"]=CategoryList;
        ProductVM productVM = new(){
            CategoryList= _unitOfWork.Category.GetAll().Select(u=>new SelectListItem
            {   
                Text=u.Name,
                Value=u.CategoryId.ToString()
            
            }),
            Product = new Product()
        };
        if (id==null || id==0) return View(productVM);
        else{
            productVM.Product = _unitOfWork.Product.Get(u=>u.ProductId==id);
            return View(productVM);
        };
            
    }

    [HttpPost]
    public IActionResult Upsert(ProductVM obj , IFormFile? file){

        // if(obj.Name == obj.DisplayOrder.ToString()){
        //     ModelState.AddModelError("Name","Product Name Could Exact Match To Display Order");
        // }
        // if(obj.Name!=null && obj.Name.ToLower()=="test"){
        //     ModelState.AddModelError("","Test Is An Invalid Product Name");
        // }
        string wwwRootPath=_webHostEnvironment.WebRootPath;
        if(file!=null){
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath,@"images/product");
            if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
            {
                var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart(('/')));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            using(var FileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create)){
                file.CopyTo(FileStream);
            }
            obj.Product.ImageUrl = Path.Combine(@"/images/product",fileName);
        }

        if(ModelState.IsValid){
            if(obj.Product.ProductId==0) _unitOfWork.Product.Add(obj.Product);
            else _unitOfWork.Product.Update(obj.Product);
            _unitOfWork.Save();
            TempData["success"]="Product Created Successfully";
            return RedirectToAction("Index");
        }
        else{
            ProductVM productVM = new(){
                CategoryList= _unitOfWork.Category.GetAll().Select(u=>new SelectListItem
                {   
                    Text=u.Name,
                    Value=u.CategoryId.ToString()
                
                }),
                Product = new Product()
             };
            return View(productVM);
        }   
    }

    #region API Calls
    
    [HttpDelete]
    public IActionResult Delete(int? id){

        Product? obj = _unitOfWork.Product.Get(u=>u.ProductId==id);
        if (obj == null) return Json(new { success = false, message = "Error While Deleting" });
        string wwwRootPath=_webHostEnvironment.WebRootPath;
        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart(('/')));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Deleted Successfully" });
        
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
        return Json(new { data = objProductList });
    }

    #endregion
}
