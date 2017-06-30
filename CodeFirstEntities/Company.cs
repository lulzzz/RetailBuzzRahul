using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
    public class Company
    {
        [Key]
        public int companyId { get; set; }
        //public IEnumerable<Company> companylist { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "CompanyName")]
        public string companyName { get; set; }
        [Required]
        [StringLength(800)]
        [Display(Name = "address")]
        public string address { get; set; }
        [Required]
        //[StringLength(8, MinimumLength = 6, ErrorMessage = "The range must be between 6 and 8")]
        [Range(100000, 99999999, ErrorMessage = "Pincode should be between 6 and 8")]
        [Display(Name = "pincode")]
        public int? pincode { get; set; }
        [Required]
        //[StringLength(16, MinimumLength = 8, ErrorMessage = "The range must be between 8 and 16")]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 1 should be between 7 and 14")]
        [Display(Name = "ContactNo1")]
        public string ContactNo1 { get; set; }
        //[StringLength(16, MinimumLength = 8, ErrorMessage = "The range must be between 8 and 16")]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 2 should be between 7 and 14")]
        public string ContactNo2 { get; set; }
        //[StringLength(16, MinimumLength = 8, ErrorMessage = "The range must be between 8 and 16")]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 3 should be between 7 and 14")]
        public string ContactNo3 { get; set; }
        [Required]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please enter correct email")]
        [Display(Name = "eMail")]
        public string eMail { get; set; }
        [Required]
        [Display(Name = "website")]
        public string website { get; set; }
        [Required]
        [Display(Name = "salesTaxNo")]
        public string salesTaxNo { get; set; }
        [Required]
        [Display(Name = "vatNo")]
        public string vatNo { get; set; }
        [Required]
        [Display(Name = "regNo")]
        public string regNo { get; set; }
        public string CompanyCode { get; set; }
        public string panCard { get; set; }
        public DateTime? CompanyRegisterationDate { get; set; }
        public string isEnabled { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string MailingAddress { get; set; }        
        public IEnumerable<BankName> BankNames { get; set; }
        public IEnumerable<BankAccountType> BankAccountTypes { get; set; }
        [Required]
        public DateTime? FinancialYearFrom { get; set; }
        [Required]
        public DateTime? FinancialYearTo { get; set; }
        public string Replicated { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public IEnumerable<State> StateList { get; set; }
        public IEnumerable<City> CityList { get; set; }
        public IEnumerable<BankName> BankNameList { get; set; }
        public IEnumerable<BankMaster> BankIFSCNoList { get; set; }
    }
}
