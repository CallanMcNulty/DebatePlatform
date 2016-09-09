using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DebatePlatform.Models;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Create(int id)
        {
            Argument thisArgument = db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            if (id == 0)
            {
                thisArgument = new Argument();
                thisArgument.ArgumentId = 0;
                thisArgument.Text = "";
            }
            return View(thisArgument);
        }
        [HttpPost]
        public IActionResult Create(string text, string affirmative, int p_id)
        {
            Argument argument = new Argument();
            argument.Text = text;
            argument.IsAffirmative = bool.Parse(affirmative);
            argument.Strength = 1;
            argument.ParentId = p_id;
            argument.UserId = 1; //For now
            db.Arguments.Add(argument);
            db.SaveChanges();
            if(argument.ParentId == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId });
        }

        [HttpPost]
        public IActionResult Vote(int id)
        {
            Argument argument = db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            argument.Strength = argument.Strength + 1;
            db.Entry(argument).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId });
        }

        public IActionResult Edit(int id)
        {
            var thisArgument = db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            return View(thisArgument);
        }
        [HttpPost]
        public IActionResult Edit(string text, string affirmative, int id)
        {
            Argument argument = db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            argument.Text = text;
            argument.IsAffirmative = bool.Parse(affirmative);
            db.Entry(argument).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId});
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var argument = db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            argument.RemoveChildren();
            db.Arguments.Remove(argument);
            db.SaveChanges();
            return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId });
        }
    }
}