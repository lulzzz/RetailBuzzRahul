using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class GetUsersEmail
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
