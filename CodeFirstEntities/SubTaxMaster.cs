using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace CodeFirstEntities
{
     [Serializable]
    public class SubTaxMaster
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public double Percentage { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string TaxType { get; set; }
        public IEnumerable<SubTaxMaster> Vatlist { get; set; }
        public IEnumerable<SubTaxMaster> Stlist { get; set; }
        public IEnumerable<SubTaxMaster> Otlist { get; set; }
    }
}
