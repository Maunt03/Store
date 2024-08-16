using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet]
        public JsonResult GetWalletCount()
        {
            string userName = HttpContext.Request.Query["userName"];
            int userId = Convert.ToInt32(GetId.GetUserId(userName));

            using var context = new AppDbContext();
            var record = from wallet in context.Wallets
                         where wallet.UserId == userId
                         select wallet.Count;
            return Json(record.First());
        }

        [HttpPatch]
        public IActionResult Payment()
        {
            string userName = HttpContext.Request.Query["userName"];
            int userId = Convert.ToInt32(GetId.GetUserId);
            if (userId == -1) return StatusCode(500);
            string productName = HttpContext.Request.Query["productName"];
            int productId = Convert.ToInt32(GetId.GetProductId);
            if (productId == -1) return StatusCode(500);
            int productQuantityInOrder = Convert.ToInt32(HttpContext.Request.Query["productQuantityInOrder"]);

            using var context = new AppDbContext();
            var userRecord = from wallet in context.Wallets
                              where wallet.UserId == userId
                              select wallet;
            decimal userWalletCount = userRecord.First().Count;

            var productRecord = from product in context.Products
                                where product.Id == productId
                                select product;
            decimal price = productRecord.First().Price;
            int productCount = productRecord.First().Count;

            if (userWalletCount >= price && productCount > 0 && productQuantityInOrder > 0 && productCount - productQuantityInOrder > 0)
            {
                userRecord.First().Count -= price;
                productRecord.First().Count -= productQuantityInOrder;
                var order = new Order()
                {
                    ProductId = productId,
                    UserId = userId
                };
                context.Orders.Add(order);
                context.SaveChanges();
                return StatusCode(200);
            }

            return StatusCode(500);
        }

        [HttpPatch]
        public IActionResult AddMoney()
        {
            string userName = HttpContext.Request.Query["userName"];
            int userId = GetId.GetUserId(userName);
            if (userId == -1) return StatusCode(500);
            int count = Convert.ToInt32(HttpContext.Request.Query["count"]);

            using var context = new AppDbContext();
            var record = from wallet in context.Wallets
                         where wallet.UserId == userId
                         select wallet;

            record.First().Count += count;
            context.SaveChanges();
            return StatusCode(200);

        }
    }
}
