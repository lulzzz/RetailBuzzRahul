using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class UserCredential
    {
        [Key]
        public int AssignId { get; set; }
        public int UserId { get; set; }
        public string UseName { get; set; }
        public string Modules { get; set; }
        public string Status { get; set; }
        public int ModuleId { get; set; }
        public string ModuleRights { get; set; }
        public string Icon { get; set; }
        public string Email { get; set; }
        public string AssignRightsCode { get; set; }
        public string AssignDashboardId { get; set; }
    }
}
