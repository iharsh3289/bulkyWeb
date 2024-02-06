using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.DataAccess.Data;
using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository;

public class ProductRepository : Repository<Product> , IProductRepository{

    private ApplicationDbContext _db ;
    public ProductRepository(ApplicationDbContext db) : base(db){
        _db=db;
    }

    public void Update(Product obj)
    {
        var objFromDb = _db.Products.FirstOrDefault(u => u.ProductId == obj.ProductId);
        if (objFromDb != null)
        {
            objFromDb.Title = obj.Title;
            objFromDb.ISBN = obj.ISBN;
            objFromDb.Description = obj.Description;
            objFromDb.Author = obj.Author;
            objFromDb.CategoryId = obj.CategoryId;
            objFromDb.ListPrice = obj.ListPrice;
            objFromDb.Price = obj.Price;
            objFromDb.Price50 = obj.Price50;
            objFromDb.Price100 = obj.Price100;
            if (obj.ImageUrl != null) objFromDb.ImageUrl = obj.ImageUrl;

        }
    }
}
