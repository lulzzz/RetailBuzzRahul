using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class SupplierMaster
    {
        [Key]
        public int SupplierId { get; set; }
        public string ShortCode { get; set; }
        [Required]
        public string SupplierType { get; set; }
        [Required]
        [StringLength(80)]
        [Display(Name = "Name")]
        public string SupplierName { get; set; }
        [Required]
        [StringLength(800)]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Range(100000,99999999,ErrorMessage="Pincode should be between 6 and 8")]
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
        [StringLength(80)]
        [Display(Name = "Email")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }
        [StringLength(80)]
        [Display(Name = "Website")]
        public string Website { get; set; }
        [StringLength(40)]
        [Display(Name = "Pancard")]
        public string pancard { get; set; }
        [StringLength(40)]
        [Display(Name = "Sales Tax No")]
        public string STNo { get; set; }
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Vat No Must Be 12 Characters Long")]
        [Display(Name = "Vat No")]
        public string VTNo { get; set; }
        [StringLength(40)]
        [Display(Name = "TDSNo")]
        public string TDSNo { get; set; }
        public string OtherTax { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Status { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "District")]
        public string district { get; set; }
        public string SupplierCode { get; set; }
        public IEnumerable<Country> CountriesList { get; set; }
        public IEnumerable<State> StatesList { get; set; }
        public IEnumerable<District> DistrictsList { get; set; }
        public string checkState { get; set; }
        public string Registered { get; set; }
    }
}