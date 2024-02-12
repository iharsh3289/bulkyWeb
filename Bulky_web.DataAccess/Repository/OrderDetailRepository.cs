using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.DataAccess.Data;
using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository;

public class OrderDetailRepository : Repository<OrderDetail> , IOrderDetailRepository{

    private ApplicationDbContext _db ;
    public OrderDetailRepository(ApplicationDbContext db) : base(db){
        _db=db;
    }

    public void Update(OrderDetail obj)
    {
        _db.OrderDetails.Update(obj);
    }
}
