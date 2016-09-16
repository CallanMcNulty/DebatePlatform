using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DebatePlatform.Models
{
    [Table("Arguments")]
    public class Argument
    {
        public Argument AddParent()
        {
            DebatePlatformContext db = new DebatePlatformContext();
            Parent = db.Arguments.FirstOrDefault(a => a.ArgumentId == ParentId);
            return Parent;
        }
        public List<Argument> AddChildren()
        {
            DebatePlatformContext db = new DebatePlatformContext();
            Children = db.Arguments
                .Where(a => a.ParentId == ArgumentId)
                .ToList();
            foreach(Argument child in Children)
            {
                child.AddParent();
            }
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

        public int GetTotalStrength()
        {
            if(Children.Count == 0)
            {
                if(IsAffirmative)
                {
                    return Strength;
                }
                else
                {
                    return Strength * -1;
                }
            }
            else
            {
                int total = Strength;
                foreach(Argument child in Children)
                {
                    total += child.GetTotalStrength();
                }
                total = IsAffirmative ? total : total * -1;
                if(total < 0 && IsAffirmative && ParentId != 0)
                {
                    return 0;
                }
                else if(total > 0 && !IsAffirmative && ParentId != 0)
                {
                    return 0;
                }
                else
                {
                    return total;
                }
            }
        }

        public Argument GetRoot()
        {
            if(ParentId == 0)
            {
                return this;
            }
            Parent = Parent ?? this.AddParent();
            return Parent.GetRoot();
        }

        [Key]
        public int ArgumentId { get; set; }
        public bool IsAffirmative { get; set; }
        public string Text { get; set; }
        public int Strength { get; set; }

        public int ParentId { get; set; }
        public virtual Argument Parent { get; set; }
        public virtual ICollection<Argument> Children { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
