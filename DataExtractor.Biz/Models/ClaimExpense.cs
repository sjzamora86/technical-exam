using System;
using System.Collections.Generic;
using System.Text;

namespace DataExtractor.Biz.Models
{
    public class ClaimExpense
    {
        public double TotalAmount { get; set; }
        public double TotalAmountExcludedGst => Math.Round(TotalAmount / 1.15, 2);
        public double GstAmount => Math.Round(TotalAmount - TotalAmountExcludedGst, 2);
        public string CostCentre { get; set; } = "UNKNOWN";
        public string PaymentMethod { get; set; }

    }
}
