using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class ClientBankDetail
    {
        [Key]
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string ClientCode { get; set; }
        public string BankIFSCNo { get; set; }
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Length Must Be 9")]
        public string MICRCode { get; set; }
        public string BankAddress { get; set; }
        [Required]
        [StringLength(25,ErrorMessage="The Account No Must Be Minimum {2} And Maximum {1}", MinimumLength = 10)]
        public string AccountNo { get; set; }
        [Required]
        public string AccountType { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [Required]
        public string Branch { get; set; }
        public IEnumerable<BankName> BankNameList { get; set; }       
        public IEnumerable<BankMaster> BankBranchList { get; set; }
    }
}
