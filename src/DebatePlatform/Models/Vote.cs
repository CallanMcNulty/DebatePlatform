using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DebatePlatform.Models
{
    [Table("Votes")]
    public class Vote
    {
        [Key]
        public int VoteId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int ArgumentId { get; set; }
        public virtual Argument Argument { get; set; }
    }
}
