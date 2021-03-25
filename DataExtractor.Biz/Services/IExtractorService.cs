using DataExtractor.Biz.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataExtractor.Biz
{
    public interface IExtractorService
    {
        ClaimExpense ExtractData(string message);
    }
}
