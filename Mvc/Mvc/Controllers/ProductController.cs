using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mvc.Models;
using Mvc.Repositories;

namespace Mvc.Controllers
{
    //[Route("[controller]")]
    public class ProductController : Controller
    {
        // private readonly ILogger<ProductController> _logger;

        // public ProductController(ILogger<ProductController> logger)
        // {
        //     _logger = logger;
        // }

        public ProductHelper product;
        public ProductController(ProductHelper helper)
        {

            product = helper;

        }

        public IActionResult Home()
        {
            List<ProductClass> data = product.getalldata();

            foreach (var item in data)
            {
                Console.WriteLine("item = " + item.c_bid);
            }
            return View("Home", data);
        }


        public IActionResult addshoes()
        {

            List<BrandClass> brands = product.getbrand();
            ViewBag.brand = brands;
            return View("Addshoes");
        }

        public async Task<IActionResult> insertdata(AddShoesClass model)
        {
            ModelState.Clear();
            Console.WriteLine("img= " + model.img);
            Console.WriteLine("img= " + model.c_date);
            Console.WriteLine("chek= " + string.Join(",", model.color));

            if (model.color != null && model.color.Count != 0)
            {
                foreach (var item in model.color)
                {
                    Console.WriteLine(item);
                }

                if (ModelState.IsValid)
                {
                    Console.WriteLine("mode is valid");
                    model.c_color = string.Join(',', model.color);
                    int row = await product.addshoesdata(model);
                    if (row > 0)
                    {
                        Console.WriteLine("book is add . ");
                        return RedirectToAction("addshoes");
                    }
                    else
                    {
                        Console.WriteLine("book is not add . ");
                        return RedirectToAction("addshoes");
                    }

                }
                else
                {
                    List<BrandClass> brands = product.getbrand();
                    ViewBag.brand = brands;
                    Console.WriteLine("mode not valid");
                    return View("Addshoes");
                }


            }
            else
            {
                ModelState.AddModelError("color", "Atlest one Color is select");
                List<BrandClass> brands = product.getbrand();
                ViewBag.brand = brands;
                Console.WriteLine("mode not valid");
                return View("Addshoes");
            }


        }


        public IActionResult deletedata(int id)
        {

            try
            {
                int row = product.deletedata(id);

                if (row > 0)
                {
                    Console.WriteLine("data is delete . ");
                    TempData["delete"] = "data is delete";
                    return RedirectToAction("Home");

                }
                else
                {
                    Console.WriteLine("data is not delete . ");
                    TempData["delete"] = "data is not delete";
                    return RedirectToAction("Home");

                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        public IActionResult updatefatch(int id)
        {
            try
            {
                List<BrandClass> brands = product.getbrand();
                ViewBag.brand = brands;

                AddShoesClass addShoes = product.fatch_update_data(id).FirstOrDefault();

                ViewBag.id=id;

                return View("Updatedata", addShoes);
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {

            }


        }

        public async Task<IActionResult> dataupdate(AddShoesClass model,int id)
        {

            try
            {
                Console.WriteLine("iddsss="+id);
                int row =  await product.update_data(model,id);
                Console.WriteLine("update row"+row);
                if (row > 0)
                {
                    Console.WriteLine("data is update . ");
                    TempData["update"] = "data is update";
                    return RedirectToAction("Home");
                }

            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {

            }

            return RedirectToAction("Home");

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