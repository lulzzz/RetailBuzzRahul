using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
    public class MainTaxMaster
    {
     [Key]
     public int Id { get; set; }
     public string Country { get; set; }
     public string State { get; set; }
     public string VAT { get; set; }
     public string SalesTax { get; set; }
     public string OtherTax { get; set; }
     public string Status{get;set;}
     public DateTime ModifiedOn { get; set; }
     public IEnumerable<Country> CountryList { get; set; }
     public IEnumerable<State> StateList { get; set; }
     public IEnumerable<District> DistrictList { get; set; }     
    }
}
