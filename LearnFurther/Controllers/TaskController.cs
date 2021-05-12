using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LearnFurther.Models;
using LearnFurther.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearnFurther.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationContext db;
        private readonly UserManager<User> _userManager;

        public TaskController(UserManager<User> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            db = context;
        }

        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTask(AddTaskViewModel model)
        {
            Models.Task task = new Models.Task
            {
                Title = model.Title,
                Description = model.Description,
                Questions = model.Questions,
                Author = await _userManager.GetUserAsync(HttpContext.User)
            };
            db.Tasks.Add(task);
            await db.SaveChangesAsync();
            return RedirectToAction("TaskList");
        }
        [HttpGet]
        public IActionResult TaskList()
        {
            var task = db.Tasks.ToList();
            return View("TaskList", task);
        }
        
        [HttpGet]
        public async Task<IActionResult> TaskExecuteAsync(short id)
        {
            var task = await db.Tasks.Include(c => c.Questions).ThenInclude(d => d.Answers)
                .Include(s => s.Questions).ThenInclude(a => a.UserAnswers).FirstOrDefaultAsync(u => u.TaskId == id);
            ShowTaskViewModel model = new()
            {
                Id = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                Questions = task.Questions,
                TestPerson = await _userManager.GetUserAsync(HttpContext.User)
            };
            return View("Show", model);
        }

        [HttpPost]
        public IActionResult Check(ShowTaskViewModel model)
        {
            var task = model;
            return Ok();
        }
    }
}