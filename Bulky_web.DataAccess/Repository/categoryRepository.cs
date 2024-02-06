using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.DataAccess.Data;
using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository;

public class categoryRepository : Repository<Category> , ICategoryRepository{

    private ApplicationDbContext _db ;
    public categoryRepository(ApplicationDbContext db) : base(db){
        _db=db;
    }

    public void Update(Category obj)
    {
        _db.Categories.Update(obj);
    }
}
