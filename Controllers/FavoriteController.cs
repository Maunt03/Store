using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Controllers
{
    public class FavoriteController : Controller
    {
        [HttpGet]
        public JsonResult GetUserFavorites()
        {
            string userName = HttpContext.Request.Query["userName"];
            int userId = GetId.GetUserId(userName);

            using var context = new AppDbContext();
            var records = from favorite in context.Favorites
                          where favorite.UserId == userId
                          select favorite;

            return Json(records.ToList());
        }

        [HttpPost]
        public IActionResult AddNewFavorite()
        {
            string userName = HttpContext.Request.Query["userName"];
            int userId = GetId.GetUserId(userName);
            if (userId == -1) return StatusCode(500);
            string productName = HttpContext.Request.Query["productName"];
            int productId = GetId.GetProductId(productName);
            if (productId == -1) return StatusCode(500);

            var favorite = new Favorite()
            {
                UserId = userId,
                ProductId = productId
            };

            using var context = new AppDbContext();
            context.Favorites.Add(favorite);
            context.SaveChanges();
            return StatusCode(200);
        }

        [HttpGet]
        public IActionResult RemoveFavorite()
        {
            string userName = HttpContext.Request.Query["userName"];
            int userId = GetId.GetUserId(userName);
            if (userId == -1) return StatusCode(500);
            string productName = HttpContext.Request.Query["productName"];
            int productId = GetId.GetProductId(productName);
            if (productId == -1) return StatusCode(500);

            using var context = new AppDbContext();
            var record = from favorite in context.Favorites
                         where favorite.UserId == userId && favorite.ProductId == productId
                         select favorite;
            if (record.Count() < 1) return StatusCode(500);
            context.Favorites.Remove(record.First());
            context.SaveChanges();
            return StatusCode(200);
        }
    }
}
