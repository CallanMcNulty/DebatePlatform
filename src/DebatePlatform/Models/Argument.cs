using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DebatePlatform.Models
{
    [Table("Arguments")]
    public class Argument
    {
        public Argument()
        {
            this.Children = new HashSet<Argument>();
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
