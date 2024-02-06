using Bulky_Web.DataAccess.Data;
using System.Diagnostics;
using Bulky_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Bulky_web.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky_Web.Models.ViewModels;

namespace Bulky_Web.Controllers ;
public class CompanyController : Controller{
    private readonly IUnitOfWork _unitOfWork ;
    public CompanyController(IUnitOfWork UnitOfWork){
        _unitOfWork=UnitOfWork;
    }
    public IActionResult Index(){   
        
        List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
        return View(objCompanyList);
    }

    public IActionResult Upsert(int? id){

        // ViewBag.CategoryList=CategoryList;
        // ViewData["CategoryList"]=CategoryList;
        if (id==null || id==0) return View(new Company());
        else{
            Company Companyobj = _unitOfWork.Company.Get(u=>u.CompanyId==id);
            return View(Companyobj);
        };
            
    }

    [HttpPost]
    public IActionResult Upsert(Company obj){

        // if(obj.Name == obj.DisplayOrder.ToString()){
        //     ModelState.AddModelError("Name","Company Name Could Exact Match To Display Order");
        // }
        // if(obj.Name!=null && obj.Name.ToLower()=="test"){
        //     ModelState.AddModelError("","Test Is An Invalid Company Name");
        // }
        

        if(ModelState.IsValid){
            if(obj.CompanyId==0) _unitOfWork.Company.Add(obj);
            else _unitOfWork.Company.Update(obj);
            _unitOfWork.Save();
            TempData["success"]="Company Created Successfully";
            return RedirectToAction("Index");
        }
        else{
            return View(obj);
        }   
    }

    #region API Calls
    
    [HttpDelete]
    public IActionResult Delete(int? id){

        Company? obj = _unitOfWork.Company.Get(u=>u.CompanyId==id);
        if (obj == null) return Json(new { success = false, message = "Error While Deleting" });
        
        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Deleted Successfully" });
        
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
        return Json(new { data = objCompanyList });
    }

    #endregion
}
