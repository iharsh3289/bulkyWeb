using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.DataAccess.Data;
using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository;

public class ApplicationUserRepository : Repository<ApplicationUser> , IApplicationUserRepository{

    private ApplicationDbContext _db ;
    public ApplicationUserRepository(ApplicationDbContext db) : base(db){
        _db=db;
    }
    
}

