using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
    public class EmployeeMaster
    {
        [Key]
        public int EmpId { get; set; }
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
        //[Required(ErrorMessage = "Contact No 2 is Required")]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 2 should be between 7 and 14")]
        [Display(Name = "ResidenceNo")]
        public string ResidenceNo { get; set; }
        [Required(ErrorMessage="Contact No 1 is Required")]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 1 should be between 7 and 14")]
        [Display(Name = "MobileNo")]
        public string MobileNo { get; set; }
        //[Required]
        [StringLength(80)]
        [Display(Name = "Email")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }
        [Required]
        [StringLength(80)]
        [Display(Name = "Designation")]
        public string Designation { get; set; }
        [Required]
        public DateTime? DateOfJoining { get; set; }
        public IEnumerable<TypeOfSupplier> Industries { get; set; }
        public string Industry { get; set; }
        [Required]
        public int totalExpYear { get; set; }
        public IEnumerable<YearExperience> totalExpYears { get; set; }
        [Required]
        public int totalExpmonth { get; set; }
        public IEnumerable<MonthExperience> totalExpmonths { get; set; }
        [Required]
        [Range(1, 99999999, ErrorMessage = "Salary should be between 1 and 8")]
        [Display(Name = "Salary")]
        public long? Salary { get; set; }
        public string BloodGroup { get; set; }
        public IEnumerable<BloodGroup> BloodGroups { get; set; }
        [StringLength(50)]
        [Display(Name = "PancardNo")]
        public string PancardNo { get; set; }
        public string AadharNo { get; set; }
        [Required]
        public string department { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Status { get; set; }
        public string CommissionType { get; set; }
        //[Required]
        public string Photo { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        public string BankName { get; set; }
        [StringLength(40)]
        public string BankIFSCNo { get; set; }
        [StringLength(25, ErrorMessage = "The {0} Must Be Minimum {2} And Maximum {1}", MinimumLength = 10)]
        //[Required]
        public string Account_No { get; set; }
        //[Required]
        public string AccountType { get; set; }
        public string BankAddress { get; set; }
        public string EmployeeCode { get; set; }
        public string ESIC { get; set; }
        [Required]
        public string PFNO { get; set; }
        [StringLength(9,MinimumLength=9,ErrorMessage="Length Must Be 9")]
        public string MICRCode { get; set; }
        //[Required]
        public string branch { get; set; }
        public IEnumerable<State> StateList { get; set; }
        public IEnumerable<City> CityList { get; set; }
        public IEnumerable<EmployeeMaster> EmpList { get; set; }
        public IEnumerable<EmployeeMaster> empnamelist { get; set; }
        public IEnumerable<DesignationMaster> DesignationList { get; set; }
        public IEnumerable<BankName> BankNameList { get; set; }
        public IEnumerable<BankMaster> BranchList { get; set; }
        public IEnumerable<Department> deptlist { get; set; }
        public string companyCode { get; set; }
    }
}
