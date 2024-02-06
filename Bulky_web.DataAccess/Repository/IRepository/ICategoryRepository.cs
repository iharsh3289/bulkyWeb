using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository.IRepository;

public interface ICategoryRepository : IRepository<Category>{

    void Update(Category obj);

}
