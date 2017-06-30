using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeFirstEntities
{
     [Serializable]
   public class Module
    {
       [Key]
       public int Id { get; set; }
       public string ModuleName { get; set; }
       public string ModuleRights { get; set; }
       public string Icon { get; set; }
       public string AssignRightsCode { get; set; }
       public string DashboardId { get; set; }
    }
}
