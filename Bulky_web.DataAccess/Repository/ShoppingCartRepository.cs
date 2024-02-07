using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.DataAccess.Data;
using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository;

public class ShoppingCartRepository : Repository<ShoppingCart> , IShoppingCartRepository{

    private ApplicationDbContext _db ;
    public ShoppingCartRepository(ApplicationDbContext db) : base(db){
        _db=db;
    }

    public void Update(ShoppingCart obj)
    {
        _db.ShoppingCarts.Update(obj);
    }
}
