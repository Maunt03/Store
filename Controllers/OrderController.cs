using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Controllers
{
    public class OrderController : Controller
    {
        [HttpPost]
        public IActionResult CreateNewOrder()
        {
            string userName = HttpContext.Request.Query["userName"];
            int userId = GetId.GetUserId(userName);
            if (userId == -1) return StatusCode(500);
            string productName = HttpContext.Request.Query["productName"];
            int productId = GetId.GetProductId(productName);
            if (productId == -1) return StatusCode(500);


            var order = new Order()
            {
                UserId = userId,
                ProductId = productId
            };

            using var context = new AppDbContext();
            context.Orders.Add(order);
            context.SaveChanges();
            return StatusCode(200);
        }

        [HttpGet]
        public JsonResult GetUserOrdersHistory()
        {
            string userName = HttpContext.Request.Query["userName"];
            int userId = GetId.GetUserId(userName);

            using var context = new AppDbContext();
            var records = from order in context.Orders
                          where order.UserId == userId
                          select order;
            return Json(records.ToList());
        }

        [HttpGet]
        public JsonResult GetAllOrders()
        {
            using var context = new AppDbContext();
            return Json(context.Orders.ToList());
        }

        [HttpGet]
        public JsonResult GetOrder()
        {
            int orderId = Convert.ToInt32(HttpContext.Request.Query["orderId"]);
            using var context = new AppDbContext();

            var record = from order in context.Orders
                         where order.Id == orderId
                         select order;

            return Json(record.ToList());
        }
    }
}
