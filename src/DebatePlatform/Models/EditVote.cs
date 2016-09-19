using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DebatePlatform.Models
{
    [Table("EditVotes")]
    public class EditVote
    {
        [Key]
        public int EditVoteId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int ProposedEditId { get; set; }
        public virtual ProposedEdit ProposedEdit { get; set; }
    }
}
