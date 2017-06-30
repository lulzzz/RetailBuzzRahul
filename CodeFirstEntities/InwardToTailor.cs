using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardToTailor
    {
        [Key]
        public int Id { get; set; }
        public string InwardCode { get; set; }
        public DateTime Date { get; set; }
        public string TailorName { get; set; }
        public string TailorAddress { get; set; }
        public string TailorContact { get; set; }
        public string TailorEmail { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string PreparedBy { get; set; }
        public string PreparedByEmail { get; set; }
        public string PreparedByDesignation { get; set; }
    }
}
