using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class TransportMaster
    {
        [Key]
        public int TransportId { get; set; }
        [Required]
        public string ModeOfTransport { get; set; }
        [Required]
        [StringLength(80)]
        [Display(Name = "Transport Name")]
        public string TransportName { get; set; }
        [Required]
        [StringLength(800)]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Range(100000, 99999999, ErrorMessage = "Pincode should be between 6 and 8")]
        [Display(Name = "pinCode")]
        public int? Pincode { get; set; }
        [Required]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 1 should be between 7 and 14")]
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
        public string Pancard { get; set; }
        [StringLength(40)]
        [Display(Name = "STNo")]
        public string STNo { get; set; }
        [StringLength(40)]
        [Display(Name = "VTNo")]
        public string VTNo { get; set; }
        [StringLength(40)]
        [Display(Name = "TDSNo")]
        public string TDSNo { get; set; }
        public string OtherTax { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Status { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string District { get; set; }
        public string TransportCode { get; set; }
        public IEnumerable<Country> CountryList { get; set; }
        public IEnumerable<State> StateList { get; set; }
        public IEnumerable<District> DistrictList { get; set; }
    }
}
