using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>{

    void Update(OrderHeader obj);
    void UpdateStatus(int id, string Orderstatus, string? PaymentStatus=null);
    void UpdateStripePaymentId(int id, string SessionId, string? PaymentIntentId=null);

}
