namespace ProjectPRN222.DTO
{
    public class DashboardDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProductsSold { get; set; }
        public List<TopProductDto> TopSellingProducts { get; set; }
        public List<RevenueByDateDto> RevenueByDate { get; set; }
    }
}
