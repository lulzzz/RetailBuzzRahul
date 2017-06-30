using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
namespace CodeFirstEntities
{
    [Serializable]
    public class OutwardToClient
    {
        [Key]
        public int OutwardId { get; set; }
        public string OutwardCode { get; set; }
        public string GatePass { get; set; }
        public string Narration { get; set; }
        public string GodownCode { get; set; }
        public string GodownName { get; set; }
        public string GodownAddress { get; set; }
        public string GodownContactNo { get; set; }
        public string GodownEmail { get; set; }
        public string GodownContactPerson { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopContactNo { get; set; }
        public string ShopEmail { get; set; }
        public string ShopContactPerson { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public string ClientContactNo { get; set; }
        public string ClientEmail { get; set; }
        public string ClientState { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string PrepareBy { get; set; }
    }
}
