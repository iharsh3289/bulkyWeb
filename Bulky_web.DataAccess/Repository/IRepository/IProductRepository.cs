using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository.IRepository;

public interface IProductRepository : IRepository<Product>{

    void Update(Product obj);

}
