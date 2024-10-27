using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mvc.Models;
using Mvc.Repositories;

namespace Mvc.Controllers
{
    //[Route("[controller]")]
    public class ClientController : Controller
    {

        public ClientHelper client;
        public ClientController(ClientHelper helper)
        {

            client = helper;

        }

        public IActionResult chome()
        {
            List<ClientClass> data = client.showdata();



            ViewBag.datas = data;

            return View("Chome");
        }

        public IActionResult purches(purchesClass model, int id)
        {
            Console.WriteLine("id = " + id);
            Console.WriteLine("qty = " + model.c_qty);

            model.c_sid = id;

            int row = client.addorder(model);

            if (row > 0)
            {
                TempData["purches"] = "thankyou for purches";
            }

            return RedirectToAction("chome");

        }

        public IActionResult cart()
        {
            List<CartClass> data = client.orders();
            Console.WriteLine(data[data.Count - 1].grandtotal);
            TempData["grandtotal"] = data[data.Count - 1].grandtotal;
            ViewBag.carts = data;
            return View("cart");
        }

        public IActionResult cancel(int id)
        {

            Console.WriteLine("id = " + id);
            client.cancel(id);

            return RedirectToAction("cart");

        }

        public IActionResult loginform()
        {
            return View("loginform");
        }

        public IActionResult login(LoginClass model)
        {

            Console.WriteLine(model.email);

            var password = new PasswordHasher<object?>().HashPassword(null, model.password);
            var user = new PasswordHasher<object?>().VerifyHashedPassword(null, model.password, "123");

            // if(user==PasswordVerificationResult.Success){

            // }


            HttpContext.Session.SetString("email", model.email);
            Console.WriteLine(password);

            return View("loginform");

        }


        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}