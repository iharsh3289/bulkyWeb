using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.DataAccess.Data;

namespace Bulky_web.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public ICategoryRepository Category {get; private set;}
    public IProductRepository Product {get; private set;}
    public ICompanyRepository Company {get; private set;}

    public UnitOfWork(ApplicationDbContext db){
        _db=db;
        Category = new categoryRepository(_db);
        Product = new ProductRepository(_db);
        Company = new CompanyRepository(_db);

    }

    public void Save()
    {
        _db.SaveChanges();
    }
}