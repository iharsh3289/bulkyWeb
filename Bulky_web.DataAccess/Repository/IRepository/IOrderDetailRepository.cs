using Bulky_Web.Models;

namespace Bulky_web.DataAccess.Repository.IRepository;

public interface IOrderDetailRepository : IRepository<OrderDetail>{

    void Update(OrderDetail obj);

}
