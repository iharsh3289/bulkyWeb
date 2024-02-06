using Bulky_Web.DataAccess.Data;
using System.Diagnostics;
using Bulky_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.Utility;
using Microsoft.AspNetCore.Authorization;

namespace Bulky_Web.Controllers ;

[Authorize(Roles = SD.Role_Admin)]
public class CategoryController : Controller{
    private readonly IUnitOfWork _unitOfWork ;
    public CategoryController(IUnitOfWork UnitOfWork){
        _unitOfWork=UnitOfWork;
    }
    
    public IActionResult Index(){   
        
        List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
        return View(objCategoryList);
    }

    public IActionResult Create(){
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category obj){

        if(obj.Name == obj.DisplayOrder.ToString()){
            ModelState.AddModelError("Name","Category Name Could Exact Match To Display Order");
        }
        if(obj.Name!=null && obj.Name.ToLower()=="test"){
            ModelState.AddModelError("","Test Is An Invalid Category Name");
        }
        if(ModelState.IsValid){
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["success"]="Category Created Successfully";
            return RedirectToAction("Index");
        }

        return View();
        
    }

    public IActionResult Edit(int? id){

        if(id==null || id==0) return NotFound();
        Category? CategoryFromDb = _unitOfWork.Category.Get(u=>u.CategoryId==id);
        if(CategoryFromDb == null) return NotFound();
        return View(CategoryFromDb);
    }

    [HttpPost]
    public IActionResult Edit(Category obj){

        if(ModelState.IsValid){
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["success"]="Category Updated Successfully";
            return RedirectToAction("Index");
        }

        return View();
        
    }

    public IActionResult Delete(int? id){

        if(id==null || id==0) return NotFound();
        Category? CategoryFromDb = _unitOfWork.Category.Get(u=>u.CategoryId==id);
        if(CategoryFromDb == null) return NotFound();
        return View(CategoryFromDb);
    }

    [HttpPost,ActionName("Delete")]
    public IActionResult DeletePOST(int? id){

        Category? obj = _unitOfWork.Category.Get(u=>u.CategoryId==id);
        if (obj==null) return NotFound();

        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        TempData["success"]="Category Deleted Successfully";
        return RedirectToAction("Index");
        
    }
}
