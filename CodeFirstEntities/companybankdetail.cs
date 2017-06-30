using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace CodeFirstEntities
{
    [Serializable]
 public class companybankdetail
    {
     [Key]
     public int BankId { get; set; }
     public string CompanyCode { get; set; }
     public string BankName { get; set; }
     public string BankIfsc { get; set; }
     public string BankLocation { get; set; }
     [Required]
     public string AccountType { get; set; }
     [Required]
     [StringLength(25, ErrorMessage = "The {0} Must Be Minimum {2} And Maximum {1}", MinimumLength = 10)]
     public string AccountNo { get; set; }
     public string Branch { get; set; }
     [StringLength(9, MinimumLength = 9, ErrorMessage = "Length Must Be 9")]
     public string MICRCode { get; set; }
     public string Status { get; set; }
     public DateTime? ModifiedOn { get; set; }
     public IEnumerable<BankName> listOfBankNames { get; set; }
     public IEnumerable<BankMaster> listOfIfsc { get; set; }
     public IEnumerable<companybankdetail> BnkList { get; set; }
     public IEnumerable<BankMaster> BranchList { get; set; }

    }
}
