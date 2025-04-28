using ProjectPRN222.DTO;
using ProjectPRN222.Models;
using Microsoft.EntityFrameworkCore;
namespace ProjectPRN222.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly Prn222projectContext _context;

        public DashboardService(Prn222projectContext context)
        {
            _context = context;
        }

        public DashboardDto GetDashboardData()
        {
            var orders = _context.Orders
                .Where(o => o.Status == "Completed")
                .Include(o => o.OrderDetails)
                .ToList();

            var totalRevenue = orders
                .SelectMany(o => o.OrderDetails)
                .Sum(od => od.Price * od.Quantity) ?? 0;

            var totalOrders = orders.Count;

            var totalProductsSold = orders
                .SelectMany(o => o.OrderDetails)
                .Sum(od => od.Quantity) ?? 0;

            var topSellingProducts = orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => od.Product.Name)
                .Select(g => new TopProductDto
                {
                    ProductName = g.Key,
                    QuantitySold = g.Sum(x => x.Quantity ?? 0),
                    TotalRevenue = g.Sum(x => (x.Price ?? 0) * (x.Quantity ?? 0))
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToList();

            var revenueByDate = orders
                .GroupBy(o => o.CreateAt.Value.Date)
                .Select(g => new RevenueByDateDto
                {
                    Date = g.Key,
                    Revenue = g.SelectMany(o => o.OrderDetails)
                               .Sum(od => (od.Price ?? 0) * (od.Quantity ?? 0))
                })
                .OrderBy(x => x.Date)
                .ToList();

            return new DashboardDto
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                TotalProductsSold = totalProductsSold,
                TopSellingProducts = topSellingProducts,
                RevenueByDate = revenueByDate
            };
        }
    }

}
