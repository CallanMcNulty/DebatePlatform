using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DebatePlatform.Models;
using System;

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
        public IActionResult Tree(int id)
        {
            Argument thisArgument = db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            thisArgument.AddChildrenRecursive();
            return View(thisArgument);
        }
        [HttpPost]
        public IActionResult Create(string text, string affirmative, string p_id)
        {
            Argument argument = new Argument();
            argument.Text = text;
            argument.IsAffirmative = bool.Parse(affirmative);
            argument.Strength = 1;
            argument.ParentId = Int32.Parse(p_id);
            db.Arguments.Add(argument);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
