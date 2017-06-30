using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CodeFirstEntities;

namespace CodeFirstEntities
{
    [Serializable]
    public class ClientMaster
    {
        [Key]
        public int ClientId { get; set; }
        [Required]
        [Display(Name = "Membershipcard")]
        public string TypeOfMembershipCard { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string ClientName { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Range(100000,99999999,ErrorMessage= "Pincode should be between 6 and 8")]
        [Display(Name = "Pincode")]
        public int? Pincode { get; set; }
        [Required]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 1 should be between 7 and 14")]
        [Display(Name = "Contact No")]
        public string ContactNo1 { get; set; }
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 2 should be between 7 and 14")]
        public string ContactNo2 { get; set; }
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 3 should be between 7 and 14")]
        public string ContactNo3 { get; set; }
        [Display(Name = "Email")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }
        [Display(Name = "Website")]
        public string Website { get; set; }
        [Display(Name = "Sales Tax No")]
        public string STNo { get; set; }
        [Display(Name = "Vat No")]
        [StringLength(12,MinimumLength=12,ErrorMessage="Vat No Must Be 12 Characters Long")]
        public string VTNo { get; set; }
        [Display(Name = "TDSNo")]
        public string TDSNo { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Status { get; set; }
        [Display(Name = "Pancard")]
        public string PanCard { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string City { get; set; }
        public string ClientCode { get; set; }
        public string OtherTax { get; set; }
        public string FormType { get; set; }
        public string NameOnCard { get; set; }       
        public long? CardNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public string checkState { get; set; }
        public string ClientType { get; set; }
        public string ConsumeResell { get; set; }
        public IEnumerable<Country> CountryList { get; set; }
        public IEnumerable<State> StateList { get; set; }
        public IEnumerable<District> DistrictList { get; set; }
    }
}
