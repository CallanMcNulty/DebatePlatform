using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DebatePlatform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        private async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _userManager.FindByIdAsync(userId);
        }
        private Argument PerformEdit(string text, bool affirmative, int parentId, int id)
        {
            Argument argument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            argument.Text = text ?? argument.Text;
            argument.IsAffirmative = affirmative;
            argument.ParentId = parentId==0 ? argument.ParentId : parentId;
            _db.Entry(argument).State = EntityState.Modified;
            _db.SaveChanges();
            return argument;
        }
        private Argument PerformDelete(int id)
        {
            var argument = _db.Arguments
                .Include(a => a.Votes)
                .FirstOrDefault(a => a.ArgumentId == id);
            argument.AddChildren();
            if (argument.Children.Count == 0)
            {
                foreach (Vote vote in argument.Votes)
                {
                    _db.Votes.Remove(vote);
                }
                _db.Arguments.Remove(argument);
                _db.SaveChanges();
            }
            return argument;
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
            ViewBag.UserType = 0;
            ViewBag.Citation = _db.Citations.FirstOrDefault(c => c.ArgumentId == id);
            if (User.IsInRole("user"))
            {
                ViewBag.UserType = 1;
            }
            else if (User.IsInRole("admin"))
            {
                ViewBag.UserType = 2;
            }
            return View(thisArgument);
        }
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
            string response = "{";
            while (argument.ParentId != 0)
            {
                argument.AddChildrenRecursive();
                response += "\""+argument.ArgumentId.ToString() +"\": "+ argument.GetTotalStrength().ToString()+",";
                argument = argument.AddParent();
            }
            argument.AddChildrenRecursive();
            response += "\"" + argument.ArgumentId.ToString() + "\": " + argument.GetTotalStrength().ToString() + "}";
            return Content(response, "text/plain");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var argument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            argument.AddChildren();
            ViewBag.CanDelete = false;
            if (argument.Children.Count == 0)
            {
                ViewBag.CanDelete = true;
            }
            return View(argument);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(string text, string affirmative, int parentId, int id)
        {
            Argument argument = PerformEdit(text, bool.Parse(affirmative), parentId, id);
            return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId});
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            Argument argument = PerformDelete(id);
            return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId });
        }
        public IActionResult Details(int id)
        {
            Argument argument = _db.Arguments
                .Include(a => a.ProposedEdits)
                .FirstOrDefault(a => a.ArgumentId == id);
            return View(argument);
        }
        
        public IActionResult ProposeEdit(int id)
        {
            Argument argument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            Argument root = argument.GetRoot();
            root.AddChildrenRecursive();
            ViewBag.Root = root;
            return View(argument);
        }

        [HttpPost]
        public async Task<IActionResult> ProposeEdit(string text, string affirmative, string reason, int parentId, int id, string delete)
        {
            Argument argument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == id);
            ProposedEdit edit = new ProposedEdit();
            edit.Text = text == "" ? null : text;
            edit.IsAffirmative = affirmative == null ? argument.IsAffirmative : bool.Parse(affirmative);
            edit.Reason = reason;
            edit.IsDelete = delete == "True" ? true : false;
            edit.ArgumentId = id;
            ApplicationUser current = await GetCurrentUser();
            edit.UserId = current.Id;
            edit.Votes = 1;
            edit.ParentId = parentId;
            _db.ProposedEdits.Add(edit);
            _db.SaveChanges();
            return RedirectToAction("Details", new { id = edit.ArgumentId });
        }

        [HttpPost]
        public async Task<IActionResult> CastEditVote(int id)
        {
            ProposedEdit edit = _db.ProposedEdits.FirstOrDefault(pe => pe.Id == id);

            ApplicationUser current = await GetCurrentUser();
            EditVote existingVote = _db.EditVotes.FirstOrDefault(ev => ev.UserId == current.Id && ev.ProposedEditId == id);
            if (existingVote == null)
            {
                edit.Votes += 1;
                _db.Entry(edit).State = EntityState.Modified;
                EditVote vote = new EditVote();
                vote.ProposedEditId = edit.Id;
                vote.UserId = current.Id;
                _db.EditVotes.Add(vote);
                _db.SaveChanges();
                if (edit.Votes >= 2)
                {
                    List<EditVote> votes = _db.EditVotes.Where(ev => ev.ProposedEditId == edit.Id).ToList();
                    foreach(EditVote v in votes)
                    {
                        _db.EditVotes.Remove(v);
                    }
                    _db.ProposedEdits.Remove(edit);
                    _db.SaveChanges();
                    if (edit.IsDelete)
                    {
                        Argument argument = PerformDelete(edit.ArgumentId);
                        return RedirectToAction("Tree", new { id = argument.GetRoot().ArgumentId });
                    }
                    else
                    {
                        PerformEdit(edit.Text, edit.IsAffirmative, edit.ParentId, edit.ArgumentId);
                    }

                }
            }
            else
            {
                edit.Votes -= 1;
                _db.Entry(edit).State = EntityState.Modified;
                _db.EditVotes.Remove(existingVote);
            }
                        
            _db.SaveChanges();
            return RedirectToAction("Details", new { id = edit.ArgumentId });
        }

        public IActionResult Cite(int id)
        {
            return View(id);
        }

        public IActionResult SearchDPLA(string term, int page)
        {
            return Json( Citation.SearchPage(term, page) );
        }

        [HttpPost]
        public async Task<IActionResult> Cite(string creator, string title, string format, string url, string date, string institution, string description, string text, int argumentId)
        {
            Citation newCite = new Citation();
            newCite.Creator = creator;
            newCite.Title = title;
            newCite.Format = format;
            newCite.URL = url;
            newCite.Date = date;
            newCite.Institution = institution;
            newCite.Description = description;
            newCite.Text = text;
            newCite.ArgumentId = argumentId;
            _db.Citations.Add(newCite);
            Argument citationArgument = new Argument();
            citationArgument.ParentId = argumentId;
            citationArgument.isCitation = true;
            citationArgument.IsAffirmative = true;
            citationArgument.Strength = 1;
            citationArgument.Text = text;
            ApplicationUser user = await GetCurrentUser();
            citationArgument.UserId = user.Id;
            _db.Arguments.Add(citationArgument);
            _db.SaveChanges();
            Argument argument = _db.Arguments.FirstOrDefault(a => a.ArgumentId == argumentId);
            int rootId = argument.GetRoot().ArgumentId;
            return RedirectToAction("Tree", new { id = rootId });
        }
    }
}