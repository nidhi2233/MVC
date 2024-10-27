using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_Practical.BAL;
using MVC_Practical.Models;
using Npgsql;

namespace MVC_Practical.Controllers
{
    // [Route("[controller]")]
    public class internDemoController : Controller
    {

        private readonly internHelper _internHelper;
        private readonly topicHelper _topicHelper;

        public internDemoController(NpgsqlConnection connection)
        {
            _internHelper = new internHelper(connection);
            _topicHelper = new topicHelper(connection);
        }

        public IActionResult GetAllInterns()
        {
            List<internDemo> internList = _internHelper.GetInterns();
            return View(internList);
        }

        [HttpGet]
        public IActionResult AddInterns()
        {
            ViewBag.Topics = _topicHelper.GetTopics();
            return View();
        }

        [HttpPost]
        public IActionResult AddInterns(internDemo internDemo)
        {
            if (ModelState.IsValid)
            {
                bool result = _internHelper.AddIntern(internDemo);
                if (result == true)
                {
                    TempData["alertMessage"] = "Record Add Successfully";
                    return RedirectToAction("GetAllInterns");
                }
            }
            TempData["alertMessage"] = "Some thing Went Wrong.";
            ViewBag.Topics = _topicHelper.GetTopics();
            return View(internDemo);
        }

        [HttpGet]
        public IActionResult EditInterns(int id)
        {
            var internList = _internHelper.GetSelectedInterns(id);
            ViewBag.Topics = _topicHelper.GetTopics();
            return View(internList);
        }

        [HttpPost]
        public IActionResult EditInterns(internDemo internDemo)
        {
            if (ModelState.IsValid || (internDemo.imagePath == null || internDemo.imagePath != null ))
            {
                bool result = _internHelper.UpdateIntern(internDemo);
                if (result == true)
                {
                    TempData["alertMessage"] = "Record Updated Successfully";
                    return RedirectToAction("GetAllInterns");
                }
            }
            TempData["alertMessage"] = "Some thing Went Wrong.";
            ViewBag.Topics = _topicHelper.GetTopics();
            return View(internDemo);
        }

        [HttpPost]
        public IActionResult DeleteInterns(int id)
        {
            if (ModelState.IsValid)
            {
                bool result = _internHelper.DeleteIntern(id);
                if (result == true)
                {
                    TempData["alertMessage"] = "Record Deleted Successfully.";
                    return RedirectToAction("GetAllInterns");
                }
            }
            TempData["alertMessage"] = "Some Thing Went Wrong.";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}