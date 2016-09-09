using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DebatePlatform.Models
{
    public class User
    {
        public User()
        {
            Arguments = new HashSet<Argument>();
        }

        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public virtual ICollection<Argument> Arguments { get; set; }
    }
}
