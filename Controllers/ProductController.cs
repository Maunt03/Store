using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public JsonResult GetAllProductsInfo()
        {
            using var context = new AppDbContext();
            return Json(context.Products.ToList());
        }

        [HttpGet]
        public JsonResult GetProductInfo()
        {
            int productId = Convert.ToInt32(HttpContext.Request.Query["productId"]);
            using var context = new AppDbContext();

            var record = from product in context.Products
                         where product.Id == productId
                         select product;

            return Json(record.First());
        }

        [HttpPost]
        public IActionResult PostNewProduct()
        {
            string productName = HttpContext.Request.Form["productName"];
            string productCategory = HttpContext.Request.Form["productCategory"];
            string productDescription = HttpContext.Request.Form["productDescription"];
            string game = HttpContext.Request.Form["game"];
            decimal productPrice = Convert.ToDecimal(HttpContext.Request.Form["productPrice"]);
            int productCount = Convert.ToInt32(HttpContext.Request.Form["productCount"]);

            var product = new Product()
            {
                Name = productName,
                Category = productCategory,
                Description = productDescription,
                Game = game,
                Price = productPrice,
                Count = productCount
            };
            using var context = new AppDbContext();
            context.Products.Add(product);
            context.SaveChanges();
            return StatusCode(200);
        }

        [HttpPatch]
        public IActionResult ChangeProductCount()
        {
            int newCount = Convert.ToInt32(HttpContext.Request.Form["count"]);
            string productName = HttpContext.Request.Form["productName"];
            if (newCount < 0) return StatusCode(500);

            using var context = new AppDbContext();
            var record = from product in context.Products
                         where product.Name == productName
                         select product;

            if (record.Count() != 1) return StatusCode(500);
            record.First().Count = newCount;
            return StatusCode(200);
        }

        [HttpGet]
        public FileContentResult GetImage()
        {
            int productId = Convert.ToInt32(HttpContext.Request.Query["productId"]);

            using var context = new AppDbContext();
            var record = from image in context.Images
                         where image.ProductId == productId
                         select image;

            string name = record.First().Name;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Images", name);
            var data = System.IO.File.ReadAllBytes(path);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = name
            };
            return result;
        }
    }
}
