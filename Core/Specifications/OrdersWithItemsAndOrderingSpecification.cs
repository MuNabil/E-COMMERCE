using System.Linq.Expressions;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(string buyerEmail)
            : base(x => x.BuyerEmail == buyerEmail)
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.OrderItems);

            AddOrderByDescending(x => x.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecification(int id, string buyerEmail)
            : base(x => x.Id == id && x.BuyerEmail == buyerEmail)
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.OrderItems);
        }
    }
}