using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LearnFurther.Models;
using LearnFurther.ViewModels;
using System.Security.Claims;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LearnFurther.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationContext db;
        UserManager<User> _userManager;

        public TaskController(UserManager<User> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            db = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
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
            return View("TaskList", db.Tasks.ToList());
        }
        
        [HttpGet]
        public async Task<IActionResult> TaskExecuteAsync(short id)
        {
            var task = await db.Tasks.Include(c => c.Questions).ThenInclude(d => d.Answers)
                .Include(s => s.Questions).ThenInclude(a => a.UserAnswers).FirstOrDefaultAsync(u => u.Id == id);
            ExecuteTaskViewModel model = new()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Questions = task.Questions,
                TestPerson = await _userManager.GetUserAsync(HttpContext.User)
            };
            return View("Execute", model);
        }

        [HttpPost]
        public IActionResult Check(ExecuteTaskViewModel model)
        {
            var task = model;
            return Ok();
        }
        public IActionResult TaskEditList()
        {
            if (User.IsInRole("Admin"))
            {
                return View("TaskEditList", db.Tasks.ToList());
            }
            if (User.IsInRole("Teacher"))
            {
                return View("TaskEditList", db.Tasks.Where(p => p.Author.Email == User.Identity.Name).ToList());
            }
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> EditAsync(short id)
        {
            Models.Task task = await db.Tasks.Include(p => p.Questions).ThenInclude(c => c.Answers).FirstOrDefaultAsync(a => a.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            EditTaskViewModel model = new EditTaskViewModel {Id = task.Id, Title = task.Title, Description = task.Description, Questions = task.Questions};
            return View("EditTask",model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(EditTaskViewModel model)
        {
            Models.Task task = await db.Tasks.FindAsync(model.Id);
            if (task != null)
            {
                db.Tasks.Remove(task);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("TaskEditList");
        }
    }
}