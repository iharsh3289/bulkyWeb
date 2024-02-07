using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository.IRepository;

public interface IShoppingCartRepository : IRepository<ShoppingCart>{

    void Update(ShoppingCart obj);

}
