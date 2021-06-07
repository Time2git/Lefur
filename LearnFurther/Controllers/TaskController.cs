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
                .Include(s => s.Questions).Include(q => q.Users).FirstOrDefaultAsync(u => u.Id == id);
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
                {//здесь бы добавить проверку, что пытается создатель задания его выполнять
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var exist = task.Users.Where(p => p.TaskId == task.Id).FirstOrDefault(e => e.User == user);
                    if (exist != null)
                    {
                        if (exist.HaveAccess)//если пользователь в списке и имеет доступ, то збс
                        {
                            ExecuteTaskViewModel model1 = new()
                            {
                                Id = task.Id,
                                Title = task.Title,
                                Description = task.Description,
                                Questions = task.Questions,
                                TestPerson = HttpContext.User.Identity.Name
                            };
                            return View("ExecuteTaskWithFullSolution", model1);
                        }//надо добавать проверку иначе ...
                    }
                    else
                    {
                        var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                        RequestAccessToTaskViewModel model2 = new()
                        {
                            TaskId = task.Id,
                            TestPerson = user.UserName,
                            Context = "У вас отсутствует доступ к данному заданию."
                        };
                        if (isAjax)
                        {
                            return PartialView("UserHavenAcces'tModal", model2);
                        }
                        return View("UserHavenAcces'tModal", model2);
                    }
                }
            } 
            ExecuteTaskViewModel model = new()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Questions = task.Questions,
                TestPerson = HttpContext.User.Identity.Name
            };
            return View("Execute", model);
        }

        [HttpPost]
        public async Task<IActionResult> TestExecuteAsync(ExecuteTaskViewModel model)
        {
            Check check = new();//Необходимо уменьшить связность, а именно заменить new, допустим, на использование в виде сервиса при помощи Di
            Models.Task task = await db.Tasks.Include(c => c.Questions).ThenInclude(d => d.Answers).FirstOrDefaultAsync(u => u.Id == model.Id);
            CheckViewModel model1 = check.CheckTestTask(model, task);
            return PartialView("TestExecutionComplete", model1);
        }
        [HttpPost]
        public async Task<IActionResult> TaksWithFullSolutionExecute(ExecuteTaskViewModel model)
        {
            var task = await db.Tasks.Include(c => c.Questions).ThenInclude(s => s.UserAnswers).Include(q => q.Users).FirstOrDefaultAsync(u => u.Id == model.Id);
            User user = await _userManager.FindByNameAsync(model.TestPerson);
            for (int i = 0; i<task.Questions.Count; i++)
            {
                if(task.Questions[i].UserAnswers.FirstOrDefault(s => s.UserId == user.Id) != null)
                {
                    task.Questions[i].UserAnswers.FirstOrDefault(s => s.UserId == user.Id).Content = model.Questions[i].UserAnswers[0].Content;
                }
                else
                {
                    var answer = model.Questions[i].UserAnswers[0];
                    answer.User = user;
                    answer.UserId = user.Id;
                    task.Questions[i].UserAnswers.Add(answer);
                }
            }
            db.Tasks.Update(task);
            db.SaveChanges();
            return RedirectToAction("TaskList");
        }

        public async Task<IActionResult> SaveExecutedTestTask(ExecuteTaskViewModel model)
        {
            Check check = new();//Необходимо уменьшить связность, а именно заменить new, допустим, на использование в виде сервиса при помощи Di
            Models.Task task = await db.Tasks.Include(c => c.Questions).ThenInclude(d => d.Answers).FirstOrDefaultAsync(u => u.Id == model.Id);
            CheckViewModel model1 = check.CheckTestTask(model, task);
            Models.User user = db.Users.FirstOrDefault(s => s.UserName == model.TestPerson);
            User user1 = await _userManager.FindByNameAsync(model.TestPerson);
            Result result = new()
            {
                Grade = model1.Grade,
                Task = task,
                User = user1
            };
            if ((db.Results.Where(u => u.Task == task).FirstOrDefault(p => p.User == user)) != null)
            {
                db.Results.Update(result);
            }
            await db.Results.AddAsync(result);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public IActionResult AddUserToWaitingList(RequestAccessToTaskViewModel model)
        {
            UsersWithAccessToTheTask user = db.UsersWithAccesses.Where(u => u.TaskId == model.TaskId).FirstOrDefault(p => p.User.UserName == model.TestPerson);
            if (user != null)
            {
                return PartialView("UserAlreadyRequestAccessToTheTaskModal", "Пожалуйста, подождите ответа от составителя задания");
            }
            else
            {
                User userr = db.Users.FirstOrDefault(u => u.UserName == model.TestPerson);
                var task = db.Tasks.FirstOrDefault(a => a.Id == model.TaskId);
                UsersWithAccessToTheTask listOf = new()
                {
                    TaskId = model.TaskId,
                    Task = task,
                    User = userr,
                    AccessRequested = true
                };
                db.UsersWithAccesses.Add(listOf);
                db.SaveChangesAsync();
            }
            return PartialView("UserAlreadyRequestAccessToTheTaskModal", "Запрос на доступ к заданию отправлен составителю.");
        }

        public IActionResult ShowUserWaitingList()
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            var task = db.Tasks.Where(s => s.Author == user);
            var list = db.UsersWithAccesses.Include(p => p.Task).Include(s => s.User).ToList();
            var l = list.Where(s => s.Task.Author == user).ToList();
            //ListOfUsersRequestingAccess list = await db.ListOfUsersRequestingAccesses.Where(u => u.TaskId == task[1].Where(s => s.Author == user));
            //var list = db.ListOfUsersRequestingAccesses.FirstOrDefault());
            return View(l);
        }

        [HttpPost]
        public async Task<IActionResult> ShowUserWaitingListAsync(short id)
        {
            UsersWithAccessToTheTask user = db.UsersWithAccesses.Include(u => u.User).FirstOrDefault(p => p.Id == id);
            UsersWithAccessToTheTask withAccess = new()
            {
                TaskId = user.TaskId,
                User = user.User,
            };
            user.HaveAccess = true;
            user.AccessRequested = false;
            db.UsersWithAccesses.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowUserWaitingList");
        }

        [HttpPost]
        public async Task<IActionResult> AccessDeniedAsync(short id)
        {
            UsersWithAccessToTheTask user = db.UsersWithAccesses.Include(u => u.User).FirstOrDefault(p => p.Id == id);
            db.UsersWithAccesses.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowUserWaitingList");
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