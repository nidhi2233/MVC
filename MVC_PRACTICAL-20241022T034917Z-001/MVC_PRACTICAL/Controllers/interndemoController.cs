using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_PRACTICAL.BAL;
using MVC_PRACTICAL.Models;


namespace MVC_PRACTICAL.Controllers
{
    //[Route("[controller]")]
    public class interndemoController : Controller
    {
        private readonly InternHelper _internHelper;
        private readonly TopicHelper _topicHelper;

        public interndemoController(InternHelper internHelper,TopicHelper topicHelper)
        {
            _internHelper = internHelper;
            _topicHelper = topicHelper;
        }
        
        [HttpGet]
        public IActionResult GetAllInterns()
        {
            var intern = _internHelper.FetchAllInterns();
            return View(intern);
        }
        
        [Route("interndemo/Getdetail/{id}")]
        public IActionResult GetDetail(int id)
        {
            InternClass intern = _internHelper.FetchInternDetails(id);


            if (intern == null)
            {
                return NotFound(); 
            }


            return View(intern); 
        }


        [HttpGet]
        public IActionResult Insert()
        {
            ViewBag.Topics = _internHelper.FetchAllTopics();
            return View();
        }

        [HttpPost]
        public IActionResult Insert(InternClass intern)
        {
            if(ModelState.IsValid)
            {
                bool result = _internHelper.AddNewIntern(intern);
                if(result == true)
                {
                    TempData["alertmessage"] = "Record Add Successfully";
                    return RedirectToAction("GetAllInterns");
                } 
                
            }
            TempData["alertMessage"] = "Something went wrong";
            ViewBag.Topics = _internHelper.FetchAllTopics();
            return View(intern);
        }

        [HttpGet("interndemo/EditIntern/{id}")]
        public IActionResult EditInterns(int id)
        {
            var internList = _internHelper.EditSelectedInterns(id);
            ViewBag.Topics = _topicHelper.GetTopics();
            return View(internList);
        }

        [HttpPost("interndemo/EditIntern/{id}")]
        public IActionResult EditInterns(InternClass internDemo)
        {
            if (ModelState.IsValid || (internDemo.TopicImageFile == null || internDemo.TopicImageFile != null ))
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


        // private readonly ILogger<interndemoController> _logger;

        // public interndemoController(ILogger<interndemoController> logger)
        // {
        //     _logger = logger;
        // }

        [HttpPost]
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