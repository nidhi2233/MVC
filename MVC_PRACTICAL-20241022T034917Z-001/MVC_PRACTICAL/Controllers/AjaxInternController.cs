using Microsoft.AspNetCore.Mvc;
using MVC_PRACTICAL.BAL;
using MVC_PRACTICAL.Models;
using Npgsql;
using System.Collections.Generic;


namespace MVC_PRACTICAL.Controllers
{
    public class AjaxInternController : Controller
    {
        private readonly InternHelper _internHelper;
        private readonly TopicHelper _topicHelper; 

        public AjaxInternController(NpgsqlConnection connection)
        {
            _internHelper = new InternHelper(connection);
            _topicHelper = new TopicHelper(connection);
        }


        // Action to load the Index page with the HTML structure
        public IActionResult Index()
        {
            // Fetch topics and interns, and pass them to the view
            ViewBag.Topics = _internHelper.FetchAllTopics(); // Assuming this method returns a list of topics
                                                             // var interns = _internHelper.FetchAllInterns(); // Fetch all interns
            return View(); // Pass the list of interns to the view
        }
       


        // GET: Fetch and return all interns as JSON
        [HttpGet]
        public JsonResult GetAllInterns()
        {
            List<InternClass> interns = _internHelper.FetchAllInterns();


            return Json(interns);
        }


        // GET: Fetch and return the details of a single intern
        [HttpGet]
        public JsonResult GetInternDetails(int id)
        {
            var intern = _internHelper.FetchInternDetails(id);
            if (intern == null)
            {
                return Json(new { success = false, message = "Intern not found." });
            }
            return Json(intern);
        }


        // POST: Add a new intern




    //    [HttpPost]
    //     public JsonResult AddIntern( InternClass intern)
    //     {
    //         if (ModelState.IsValid)
    //         {
    //             _internHelper.AddNewIntern(intern); // Image is saved in this method
    //             return Json(new { success = true, message = "Intern added successfully.", intern });
    //         }
    //         return Json(new { success = false, message = "Invalid intern data." });
    //     }

    [HttpPost]
        public JsonResult AddIntern([FromForm] InternClass intern)
        {
            if (ModelState.IsValid)
            {
                // Ensure any file for TopicImageFile is saved properly
                if (intern.TopicImageFile != null)
                {
                    // Assuming a helper method to handle the image upload
                    string uploadedImage = _internHelper.SaveImage(intern.TopicImageFile);
                    intern.TopicImage = uploadedImage;  // Save the filename in the TopicImage field
                }

                bool isAdded = _internHelper.AddNewIntern(intern);
                if (isAdded)
                {
                    return Json(new { success = true, message = "Intern added successfully.", intern });
                }
            }
            return Json(new { success = false, message = "Invalid intern data." });
        }


        [HttpPost]
        public JsonResult UpdateIntern([FromForm] InternClass intern)
        {
            if (ModelState.IsValid)
            {
                _internHelper.UpdateIntern(intern); // This handles image update or keeping the old image
                return Json(new { success = true, message = "Intern updated successfully." });
            }
            return Json(new { success = false, message = "Invalid intern data." });
        }


        // POST: Delete an intern
        [HttpPost]
        public JsonResult DeleteIntern(int id)
        {
            _internHelper.DeleteIntern(id);
            return Json(new { success = true, message = "Intern deleted successfully." });
        }
    }
}
