using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace CodeFirstEntities
{
    [Serializable]
    public class GetItemsFromItemMaster
    {
        [Key]
        //public int itemId { get; set; }
        public string itemCode { get; set; }
       // public string itemName { get; set; }
    }
}
