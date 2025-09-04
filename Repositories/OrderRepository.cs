using GameStore.Data;
using GameStore.Interfaces;
using GameStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories
{
    public class OrderRepository : IOrder
    {
        private ApplicationContext _context;

        public OrderRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.Include(e => e.Lines).ThenInclude(e => e.Product);
        }

        public Order GetOrder(int id)
        {
            return _context.Orders.Include(e => e.Lines).FirstOrDefault(e => e.Id == id);
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public IEnumerable<ProductStatsViewModel> GetTopOrders()
        {
            var topProducts = _context.Orders
                .SelectMany(o => o.Lines)
                .GroupBy(line => line.Product)
                .Select(group => new ProductStatsViewModel
                {
                    Product = group.Key,
                    TotalQuantity = group.Sum(line => line.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantity)
                .Take(10)
                .ToList();

            return topProducts;
        }

    }
}
