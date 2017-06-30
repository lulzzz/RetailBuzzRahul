using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class IncomeExpenseVoucher
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? Date { get; set; }
        public string VoucherType { get; set; }
        public string Leisure { get; set; }
        public string Narration { get; set; }
        [Required]
        public double? Amount { get; set; }
        public string PaymentType { get; set; }
        public int? Cash_1000 { get; set; }
        public double? Cash_1000_Amt { get; set; }
        public int? Cash_500 { get; set; }
        public double? Cash_500_Amt { get; set; }
        public int? Cash_100 { get; set; }
        public double? Cash_100_Amt { get; set; }
        public int? Cash_50 { get; set; }
        public double? Cash_50_Amt { get; set; }
        public int? Cash_20 { get; set; }
        public double? Cash_20_Amt { get; set; }
        public int? Cash_10 { get; set; }
        public double? Cash_10_Amt { get; set; }
        public double? Cash_1 { get; set; }
        public double? Cash_1_Amt { get; set; }
        public double? TotalCash { get; set; }
        public double? Refund { get; set; }
        public string SelectedCard { get; set; }
        public string CreditCardNo { get; set; }
        public string CreditCardType { get; set; }
        public double? CreditCardAmount { get; set; }
        public string CreditCardBank { get; set; }
        public string DebitCardName { get; set; }
        public string DebitCardNo { get; set; }
        public string DebitCardType { get; set; }
        public double? DebitCardAmount { get; set; }
        public string DebitCardBank { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeAccNo { get; set; }
        public double? ChequeAmount { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string ChequeBank { get; set; }
        public string ChequeBranch { get; set; }
        public string PreparedByName { get; set; }
        public string PreparedByEmail { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
