using DataExtractor.Biz.Exceptions;
using DataExtractor.Biz.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DataExtractor.Biz.Services
{
    public class ExtractorService : IExtractorService
    {
        public ClaimExpense ExtractData(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new InvalidFormatException();
            }
            
            var startIndex = message.IndexOf("<");
            var endIndex = message.LastIndexOf(">");

            if (startIndex < 0 || endIndex < 0)
            {
                throw new InvalidFormatException();
            }

            var xmlMessage = message.Substring(startIndex, (endIndex + 1) - startIndex);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlMessage);

            string xpath = "expense";
            var expenses = xmlDoc.SelectNodes(xpath);

            if (expenses.Count == 0)
            {
                throw new InvalidFormatException();
            }

            var expense = expenses.Item(0);
            var mapping = new Dictionary<string, string>();
            foreach (XmlNode childrenNode in expense.ChildNodes)
            {
                var name = childrenNode.Name;
                var value = childrenNode.InnerText;
                mapping.Add(name, value);
            }

            if (!mapping.ContainsKey("total"))
            {
                throw new InvalidFormatException();
            }

            return ComposeClaimExpense(mapping);
        }

        private ClaimExpense ComposeClaimExpense(IDictionary<string, string> mapping)
        {
            var total = Convert.ToDouble(mapping["total"]);
            var costCentre = mapping.ContainsKey("cost_centre") ? mapping["cost_centre"] : "UNKNOWN";
            var paymentMethod = mapping.ContainsKey("payment_method") ? mapping["payment_method"] : "";

            return new ClaimExpense
            {
                TotalAmount = total,
                CostCentre = costCentre,
                PaymentMethod = paymentMethod
            };
        }
    }
}
