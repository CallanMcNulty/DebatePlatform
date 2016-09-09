using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DebatePlatform.Models
{
    [Table("Arguments")]
    public class Argument
    {
        public List<Argument> AddChildren()
        {
            DebatePlatformContext db = new DebatePlatformContext();
            Children = db.Arguments
                .Where(a => a.ParentId == ArgumentId)
                .ToList();
            return Children.ToList();
        }
        public void AddChildrenRecursive()
        {
            AddChildren();
            foreach(Argument child in Children)
            {
                child.AddChildrenRecursive();
            }
        }
        public void RemoveChildrenRecursive(DebatePlatformContext db)
        {
            AddChildren();
            foreach (Argument child in Children)
            {
                child.RemoveChildrenRecursive(db);
                db.Arguments.Remove(child);
            }
        }
        public void RemoveChildren()
        {
            DebatePlatformContext db = new DebatePlatformContext();
            RemoveChildrenRecursive(db);
            db.SaveChanges();
        }

        [Key]
        public int ArgumentId { get; set; }
        public bool IsAffirmative { get; set; }
        public string Text { get; set; }
        public int Strength { get; set; }

        public int ParentId { get; set; }
        public virtual Argument Parent { get; set; }
        public virtual ICollection<Argument> Children { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
