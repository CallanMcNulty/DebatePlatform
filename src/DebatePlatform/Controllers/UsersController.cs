using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DebatePlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace DebatePlatform.Controllers
{
    public class UsersController : Controller
    {
        private DebatePlatformContext db = new DebatePlatformContext();
        public IActionResult Profile(int id)
        {
            User thisUser = db.Users
                .Include(u => u.Arguments)
                .FirstOrDefault(u => u.UserId == id);
            //temporary
            if(thisUser == null)
            {
                thisUser = new User();
                thisUser.Username = "Callan";
                db.Users.Add(thisUser);
                db.SaveChanges();
            }
            //end-temporary
            return View(thisUser);
        }
    }
}