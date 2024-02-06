using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.DataAccess.Data;
using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository;

public class CompanyRepository : Repository<Company> , ICompanyRepository{

    private ApplicationDbContext _db ;
    public CompanyRepository(ApplicationDbContext db) : base(db){
        _db=db;
    }

    public void Update(Company obj)
    {
        _db.Companies.Update(obj);
    }
}
