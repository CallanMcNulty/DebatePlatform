using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DebatePlatform.Models
{
    [Table("ProposedEdits")]
    public class ProposedEdit
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsAffirmative { get; set; }
        public int ParentId { get; set; }
        public bool IsDelete { get; set; }
        public string Reason { get; set; }
        public int ArgumentId { get; set; }
        public virtual Argument Argument{ get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int Votes { get; set; }
    }
}
