using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository.IRepository;

public interface ICompanyRepository : IRepository<Company>{

    void Update(Company obj);

}
