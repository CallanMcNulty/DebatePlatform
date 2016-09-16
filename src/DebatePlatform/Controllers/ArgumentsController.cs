using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DebatePlatform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace DebatePlatform.Controllers
{
    public class ArgumentsController : Controller
    {
        private readonly DebatePlatformContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ArgumentsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, DebatePlatformContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        public async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _userManager.FindByIdAsync(userId);
        }
        public IActionResult Index()
        {
            List<Argument> rootArguments = new List<Argument>();
            rootArguments = _db.Arguments
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
            Argument thisArgument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            thisArgument.AddChildrenRecursive();
            thisArgument.AddParent();
            return View(thisArgument);
        }

        public IActionResult Create(int id)
        {
            Argument thisArgument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            if (id == 0)
            {
                thisArgument = new Argument();
                thisArgument.ArgumentId = 0;
                thisArgument.Text = "";
            }
            return View(thisArgument);
        }
        [HttpPost]
        public async Task<IActionResult> Create(string text, string affirmative, int p_id)
        {
            Argument argument = new Argument();
            argument.Text = text;
            argument.IsAffirmative = bool.Parse(affirmative);
            argument.Strength = 1;
            argument.ParentId = p_id;
            ApplicationUser user = await GetCurrentUser();
            argument.UserId = user.Id;
            _db.Arguments.Add(argument);
            _db.SaveChanges();
            if (argument.ParentId == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId });
        }

        [HttpPost]
        public async Task<IActionResult> Vote(int id)
        {
            ApplicationUser current = await GetCurrentUser();
            Argument argument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            Vote existingVote = _db.Votes.FirstOrDefault(v => v.UserId==current.Id && v.ArgumentId==argument.ArgumentId);
            if (existingVote==null)
            {
                argument.Strength = argument.Strength + 1;
                Vote vote = new Vote();
                vote.ArgumentId = argument.ArgumentId;
                vote.UserId = current.Id;
                _db.Votes.Add(vote);
            }
            else
            {
                argument.Strength = argument.Strength - 1;
                _db.Votes.Remove(existingVote);
            }
            _db.Entry(argument).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId });
        }

        public IActionResult Edit(int id)
        {
            var thisArgument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            return View(thisArgument);
        }
        [HttpPost]
        public IActionResult Edit(string text, string affirmative, int id)
        {
            Argument argument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            argument.Text = text;
            argument.IsAffirmative = bool.Parse(affirmative);
            _db.Entry(argument).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId});
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var argument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            argument.RemoveChildren();
            _db.Arguments.Remove(argument);
            _db.SaveChanges();
            return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId });
        }
    }
}