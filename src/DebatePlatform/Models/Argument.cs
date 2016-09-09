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
            var db = new DebatePlatformContext();
            this.Children = db.Arguments
                .Where(a => a.ParentId == ArgumentId)
                .ToList();
            return Children.ToList();
        }

        [Key]
        public int ArgumentId { get; set; }
        public bool IsAffirmative { get; set; }
        public string Text { get; set; }
        public int Strength { get; set; }

        public int ParentId { get; set; }
        public virtual Argument Parent { get; set; }
        public virtual ICollection<Argument> Children { get; set; }
    }
}
