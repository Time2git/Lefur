using System;
using System.Collections.Generic;
using LearnFurther.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnFurther.Controllers
{
    public class NotificationSender : Controller
    {
        private ApplicationContext db;
        UserManager<User> _userManager;
        public NotificationSender(UserManager<User> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            db = context;
        }
        public async Task<int> GetNotificationsAsync()
        {
            if (HttpContext.User.Identity.Name != null)
            {
                User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var count = db.Notifications.Where(s => s.User == user).ToList();
                return count.Where(s => s.HasBeenRead == false).Count();
            }
            else
            {
                return 0;
            }
        }
    }
}