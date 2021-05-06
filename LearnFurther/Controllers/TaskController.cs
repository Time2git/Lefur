using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LearnFurther.Models;
using LearnFurther.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationContext db;

        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        public /*async Task<*/IActionResult/*>*/ AddTask(AddTaskViewModel model,IFormFile uploadedFile)
        {

            //if (uploadedFile != null)
            //{
            //    IQueryable<Models.Task> task = db.Tasks;
            //    IQueryable<Question> questions = db.Questions;
            //    IQueryable<Answer> answers = db.Answers;
            //    IQueryable<UserAnswer> userAnswers = db.UserAnswers;
            //    using (StreamReader sr = new StreamReader(uploadedFile.OpenReadStream()))
            //    {
            //        string line;
            //        while ((line = sr.ReadLine()) != null)
            //        {
            //            Console.WriteLine(line);
            //        }
            //    }
            //}
            return Content(model.Description.ToString());
        }
    }
}