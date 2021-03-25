using System;
using System.Collections.Generic;
using System.Text;

namespace DataExtractor.Biz.Models
{
    public class ClaimExpense
    {
        public decimal TotalAmount { get; set; }
        public decimal GstAmount { get; set; }
        public string CostCentre { get; set; } = "UNKNOWN";
        public string PaymentMethod { get; set; }

    }
}
