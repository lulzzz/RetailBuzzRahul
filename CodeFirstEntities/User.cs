using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ContactNo { get; set; }
        public string UserCode { get; set; }
        public DateTime ModifiedBy { get; set; }
        public string Status { get; set; }
        public string CompanyCode { get; set; }
        public IEnumerable<User> userlist { get; set; }
        public IEnumerable<Company> CompanyList { get; set; }
    }
}
