using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN222.Services;
using ProjectPRN222.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ProjectPRN222.Pages.AdminSite
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public DashboardModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }

        public DashboardDto DashboardData { get; set; }

        public List<RevenueByDateDto> FilteredRevenueByDate { get; set; }

        public void OnGet()
        {
            DashboardData = _dashboardService.GetDashboardData();

            // Áp d?ng l?c th?i gian cho doanh thu theo ngày
            var from = FromDate ?? DateTime.MinValue;
            var to = ToDate ?? DateTime.MaxValue;

            FilteredRevenueByDate = DashboardData.RevenueByDate
                .Where(r => r.Date >= from && r.Date <= to)
                .OrderBy(r => r.Date)
                .ToList();
        }
    }
}
