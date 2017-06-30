using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class BankMaster
    {
        [Key]
        public int bankId { get; set; }
        [Required]
        [Display(Name = "Bank Name")]
        public string bankName { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Range(100000, 99999999 , ErrorMessage = "Pincode should be between 6 and 8")]
        [Display(Name = "Pincode")]
        public int? pinCode { get; set; }
        [Required]
        //[StringLength(16, MinimumLength = 8, ErrorMessage = "The range is between 8 and 16")]
        [Range(1000000,99999999999999,ErrorMessage="Contact No 1 should be between 7 and 14")]
        public string ContactNo1 { get; set; }
        //[StringLength(16, MinimumLength = 8, ErrorMessage = "The range is between 8 and 16")]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 2 should be between 7 and 14")]
        public string ContactNo2 { get; set; }
        //[StringLength(16, MinimumLength = 8, ErrorMessage = "The range is between 8 and 16")]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No 3 should be between 7 and 14")]
        public string ContactNo3 { get; set; }
        [Required]
        public string Branch { get; set; }
        [Required]
        [Display(Name = "Email")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please Enter Correct email")]
        public string email { get; set; }
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Length Must Be 9")]
        public string MICRCode { get; set; }
        public string website { get; set; }
        [Required]
        [Display(Name = "IFSC")]
        public string IFSC { get; set; }
        public string status { get; set; }
        public DateTime? modifiedon { get; set; }
    }
}