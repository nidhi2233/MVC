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
    public class topicController : Controller
    {
        private readonly topicHelper _topics;

        public topicController(NpgsqlConnection connection)
        {
            _topics = new topicHelper(connection);
        }

        public IActionResult GetAllTopic()
        {
            List<topics> topics = _topics.GetTopics();
            return View(topics);
        }

        [HttpGet]
        public IActionResult AddTopic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTopic(topics topics)
        {
            if (ModelState.IsValid)
            {
                bool result = _topics.AddTopics(topics);
                if (result)
                {
                    return RedirectToAction("GetAllTopic");
                }
            }
            return View(topics);
        }

        [HttpGet]
        public IActionResult EditTopic(int id)
        {
            var topic = _topics.GetSelectedTopics(id);
            return View(topic);
        }

        [HttpPost]
        public IActionResult EditTopic(topics topics)
        {
            if (ModelState.IsValid)
            {
                bool result = _topics.UpdateTopics(topics);
                if (result)
                {
                    return RedirectToAction("GetAllTopic");
                }
            }
            return View(topics);
        }

        [HttpPost]
        public IActionResult DeleteTopic(int id)
        {
            if (ModelState.IsValid)
            {
                bool result = _topics.DeleteTopics(id);
                if (result)
                {
                    return RedirectToAction("GetAllTopic");
                }
            }
            return RedirectToAction("GetAllTopic");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}