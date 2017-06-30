using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
    public class JobWorker
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        [Required]
        [StringLength(80)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [StringLength(800)]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        [Range(1000000, 99999999999999, ErrorMessage = "Residence No should be between 7 and 14")]
        [Display(Name = "ResidenceNo")]
        public string ResidenceNo { get; set; }
        [Required]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No should be between 7 and 14")]
        [Display(Name = "MobileNo")]
        public string MobileNo { get; set; }
        [StringLength(80)]
        [Display(Name = "Email")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }
        [StringLength(80)]
        [Display(Name = "Designation")]
        public string Designation { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public IEnumerable<TypeOfSupplier> Industries { get; set; }
        public string Industry { get; set; }
        public int? TotalExpYear { get; set; }
        public int? TotalExpmonth { get; set; }
        [Range(1, 99999999, ErrorMessage = "Salary should be between 1 and 8")]
        [Display(Name = "Salary")]
        public long? Salary { get; set; }
        public string BloodGroup { get; set; }
        public IEnumerable<BloodGroup> BloodGroups { get; set; }
        [StringLength(50)]
        [Display(Name = "PancardNo")]
        public string PancardNo { get; set; }
        public string Department { get; set; }
        public string Photo { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        public string BankName { get; set; }
        [StringLength(40)]
        public string BankIFSCNo { get; set; }
        [StringLength(25, ErrorMessage = "The {0} Must Be Minimum {2} And Maximum {1}", MinimumLength = 10)]
        public string Account_No { get; set; }
        public string AccountType { get; set; }
        public string BankAddress { get; set; }
        public string ESIC { get; set; }
        public string PFNO { get; set; }
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Length Must Be 9")]
        public string MICRCode { get; set; }
        public string Branch { get; set; }
        public string CompanyCode { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
