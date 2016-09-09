using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DebatePlatform.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DebatePlatform.Controllers
{
    public class ArgumentsController : Controller
    {
        private DebatePlatformContext db = new DebatePlatformContext();
        public IActionResult Index()
        {
            List<Argument> rootArguments = new List<Argument>();
            rootArguments = db.Arguments
                .Where(a => a.ParentId == 0)
                .ToList();
            foreach(Argument a in rootArguments)
            {
                a.AddChildren();
            }
            return View(rootArguments);
        }
    }
}
