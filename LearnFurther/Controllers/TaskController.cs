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
            Models.Task task = new ()
            {
                Title = model.Title,
                Description = model.Description,
                Questions = model.Questions,
                Author = await _userManager.GetUserAsync(HttpContext.User),
                Types = (Models.TaskTypes)model.Types
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
                .Include(s => s.Questions).ThenInclude(a => a.UserAnswers).Include(q => q.Users).FirstOrDefaultAsync(u => u.Id == id);
            //UsersWithAccessToTheTask usersWith = new()//добавил чтобы чисто проверить код ниже
            //{
            //    TaskId = task.Id,
            //    User = await _userManager.GetUserAsync(HttpContext.User)
            //};
            //db.UsersWithAccesses.Add(usersWith);
            //await db.SaveChangesAsync();//код между этим и верхним комментом удалить после отладки
            if (task.Types == Models.TaskTypes.TaskWithFullSolution)
            {
                if (HttpContext.User.Identity.Name == null)
                {
                    var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                    if (isAjax)
                    {
                        return PartialView("UserNotAuthenticateModal", "Чтобы приступить к выполнению задания пожалуйста пройдите авторизацию");
                    }
                    return View("UserNotAuthenticateModal", "Чтобы приступить к выполнению задания пожалуйста пройдите авторизацию");
                }
                else
                {//здесь бы добавить проверку, что пытается создаль задания его выполнять
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var exist = task.Users.Where(p => p.Id == task.Id).FirstOrDefault(e => e.User == user);
                    if (exist != null)
                    {
                        ExecuteTaskViewModel model1 = new()
                        {
                            Id = task.Id,
                            Title = task.Title,
                            Description = task.Description,
                            Questions = task.Questions,
                            TestPerson = await _userManager.GetUserAsync(HttpContext.User)
                        };
                        return View("Execute", model1);
                    }
                    else
                    {
                        var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                        if (isAjax)
                        {
                            return PartialView("UserHavenAcces'tModal", "У вас отсутствует доступ к данному заданию.");
                        }
                        return View("UserHavenAcces'tModal", "У вас отсутствует доступ к данному заданию.");
                    }
                }
            } 
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
        public IActionResult TestExecute(ExecuteTaskViewModel model)
        {
            Check check = new ();//Необходимо уменьшить связность, а именно заменить new, допустим, на использование в виде сервиса при помощи Di
            CheckViewModel model1 = check.CheckTestTask(model);
            return PartialView(model1);
        }

        public async Task<IActionResult> SaveExecutedTestTask(ExecuteTaskViewModel model)
        {
            Check check = new();//Необходимо уменьшить связность, а именно заменить new, допустим, на использование в виде сервиса при помощи Di
            var model1 = check.CheckTestTask(model);
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
            EditTaskViewModel model = new () {Id = task.Id, Title = task.Title, Description = task.Description, Questions = task.Questions};
            return View("EditTask",model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                Models.Task task = await db.Tasks.FindAsync(model.Id);
                if (task != null)
                {
                    task.Title = model.Title;
                    task.Description = model.Description;
                    task.Questions = model.Questions;
                    db.Tasks.Update(task);
                    await db.SaveChangesAsync();
                    return RedirectToAction("TaskEditList");
                }
            }
            return View("EditTask", model);
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