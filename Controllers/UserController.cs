using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Store.Models;
using System.Data.Entity;

namespace Store.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {

            string userName = HttpContext.Request.Query["userName"];
            string userPassword = HttpContext.Request.Query["userPassword"];
            if (userName == null || userPassword == null || userName == "" || userPassword == "") return StatusCode(500);
            using var context = new AppDbContext();
            var record = from user in context.Users
                         where user.Login == userName && user.Password == userPassword
                         select new { user.Id, user.Login, user.Email };

            if (record.ToList().Count == 1) return Json(record.First());
            return StatusCode(500);
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            using var context = new AppDbContext();
            var records = from user in context.Users
                          select user;
            return Json(records.ToList());
        }



        [HttpPost]
        public IActionResult Registration()
        {
            string userName = HttpContext.Request.Form["userName"];
            string userPassword = HttpContext.Request.Form["userPassword"];
            string userEmail = HttpContext.Request.Form["userEmail"];

            if (userName == null || userPassword == null || userEmail == null) return StatusCode(500);

            using var context = new AppDbContext();
            var userCheck = from user in context.Users
                            where user.Login == userName
                            select user.Login;

            if (userCheck.ToList().Count != 0) return StatusCode(500);
            var newUser = new User()
            {
                Login = userName,
                Password = userPassword,
                Email = userEmail
            };
            context.Users.Add(newUser);
            context.SaveChanges();

            var newWallet = new Wallet()
            {
                UserId = newUser.Id,
                Count = 0
            };
            context.Wallets.Add(newWallet);
            context.SaveChanges();

            return StatusCode(200);
        }

        [HttpPatch]
        public IActionResult UpdateUserName()
        {
            string userName = HttpContext.Request.Form["userName"];
            if (userName == null || userName == "") return StatusCode(500);

            using var context = new AppDbContext();
            var record = from user in context.Users
                         where user.Login == userName
                         select user;

            if (record.ToList().Count != 1) return StatusCode(500);


            foreach (var item in record)
            {
                item.Login = userName;
            }
            context.SaveChanges();
            return StatusCode(200);
        }

        [HttpPatch]
        public IActionResult UpdateUserPassword()
        {
            string userName = HttpContext.Request.Query["userName"];
            string newPassword = HttpContext.Request.Form["newPassword"];
            if (userName == null || newPassword == null || userName == "" || newPassword == "") return StatusCode(500);

            using var context = new AppDbContext();
            var record = from user in context.Users
                         where user.Login == userName
                         select user;
            if (record.ToList().Count != 1) return StatusCode(500);

            foreach (var item in record)
            {
                item.Password = newPassword;
            }
            context.SaveChanges();
            return StatusCode(200);
        }

        [HttpPut]
        public IActionResult Update()
        {
            string userName = HttpContext.Request.Form["userName"];
            string userPassword = HttpContext.Request.Form["password"];
            if (userName == null || userPassword == null || userName == "" || userPassword == "") return StatusCode(500);

            using var context = new AppDbContext();
            var record = from user in context.Users
                         where user.Login == userName
                         select user;
            if (record.ToList().Count != 1) return StatusCode(500);

            foreach (var item in record)
            {
                item.Login = userName;
                item.Password = userPassword;
            }
            context.SaveChanges();
            return StatusCode(200);
        }

        [HttpDelete]
        public IActionResult DeleteAccount()
        {
            string userName = HttpContext.Request.Query["userName"];
            using var context = new AppDbContext();

            var record = from user in context.Users
                         where user.Login == userName
                         select user;
            if (record.ToList().Count != 1) return StatusCode(500);

            foreach (var item in record)
            {
                context.Users.Remove(item);
            }
            context.SaveChanges();
            return StatusCode(200);
        }

        [HttpGet]
        public IActionResult PasswordRecovery()
        {
            string userName = HttpContext.Request.Form["userName"];
            string userEmail = HttpContext.Request.Form["userEmail"];
            if (userName == null || userEmail == null || userName == "" || userEmail == "") return StatusCode(500);
            using var context = new AppDbContext();
            var record = from user in context.Users
                         where user.Login == userName && user.Email == userEmail
                         select user;

            if (record.ToList().Count == 1) return StatusCode(200);
            return StatusCode(500);
        }
    }
}
