using Bulky_web.DataAccess.Repository.IRepository;
using Bulky_Web.DataAccess.Data;
using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository;

public class OrderHeaderRepository : Repository<OrderHeader> , IOrderHeaderRepository{

    private ApplicationDbContext _db ;
    public OrderHeaderRepository(ApplicationDbContext db) : base(db){
        _db=db;
    }

    public void Update(OrderHeader obj)
    {
        _db.OrderHeaders.Update(obj);
    }
    
    public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null) {
        var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.OrderHeaderId == id);
        if (orderFromDb != null) {
            orderFromDb.OrderStatus = orderStatus;
            if (!string.IsNullOrEmpty(paymentStatus)) {
                orderFromDb.PaymentStatus = paymentStatus;
            }
        }
    }

    public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId) {
        var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.OrderHeaderId == id);
        if (!string.IsNullOrEmpty(sessionId)) {
            orderFromDb.SessionId= sessionId;
        }
        if (!string.IsNullOrEmpty(paymentIntentId)) {
            orderFromDb.PaymentIntentId= paymentIntentId;
            orderFromDb.PaymentDate = DateTime.Now;
        }
    }
}
