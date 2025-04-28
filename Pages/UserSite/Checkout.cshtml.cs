using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using ProjectPRN222.Models;
using ProjectPRN222.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectPRN222.Pages.UserSite
{
    public class CheckoutModel : PageModel
    {
        private readonly Prn222projectContext _context;
        private readonly UserManager<User> _userManager;
        private readonly BankSettings _bankSettings;

        public CheckoutModel(Prn222projectContext context, UserManager<User> userManager, BankSettings bankSettings)
        {
            _context = context;
            _userManager = userManager;
            _bankSettings = bankSettings;
        }

        [BindProperty]
        public List<int> SelectedIds { get; set; }

        public List<Card> SelectedProducts { get; set; } = new List<Card>();

        public decimal TotalAmount { get; set; } = 0;
        public string TransferInfo { get; set; }
        public string BankId { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public Order NewOrder { get; set; }

        public User CurrentUser { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SelectedIds == null || !SelectedIds.Any())
            {
                return RedirectToPage("/UserSite/Card");
            }

            CurrentUser = await _userManager.GetUserAsync(User);

            SelectedProducts = await _context.Cards
               .Where(c => SelectedIds.Contains(c.Id) && c.UserId == CurrentUser.Id)
               .Include(c => c.Product)
               .ToListAsync();

            TotalAmount = SelectedProducts?.Sum(p => ((p.Product?.Price ?? 0) * (p.Quantity ?? 0))) ?? 0;

            NewOrder = new Order
            {
                UserId = CurrentUser.Id,
                Status = OrderStatus.PendingPayment,
                CreateAt = DateTime.Now,
            };
            _context.Orders.Add(NewOrder);
            await _context.SaveChangesAsync();
            if (SelectedIds != null && SelectedIds.Any())
            {
                foreach (int id in SelectedIds)
                {
                    Card item = await _context.Cards
                        .Include(c => c.Product)
                        .FirstOrDefaultAsync(c => c.Id == id);

                    Product product = item?.Product;
                    if (product != null)
                    {
                        product.Stock = product.Stock - item.Quantity;
                    }

                    _context.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = NewOrder.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = _context.Products.Find(item.Product.Id).Price
                    });
                    _context.Cards.Remove(_context.Cards.Find(id));
                }
            }
            BankId = _bankSettings.Bank_id;
            AccountNo = _bankSettings.AccountNo;
            AccountName = _bankSettings.BankName;
            TransferInfo = $"PAY{CurrentUser.Id}{NewOrder.Id}".Replace("-", "");
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostPaymentCompleteAsync(string totalAmount, string content, string order_id)
        {
            Payment payment = new Payment
            {
                OrderId = int.Parse(order_id),
                Content = content,
                Price = decimal.Parse(totalAmount)
            };
            _context.Payments.Add(payment);
            var order = _context.Orders.Find(int.Parse(order_id));
            if (order != null)
            {
                order.Status = OrderStatus.AwaitingConfirmation;
                _context.Orders.Update(order);
            }
            _context.SaveChanges();
            return RedirectToPage("/UserSite/Order");
        }
    }
}

