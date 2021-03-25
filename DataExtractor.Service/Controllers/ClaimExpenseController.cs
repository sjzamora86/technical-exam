using DataExtractor.Biz;
using DataExtractor.Biz.Exceptions;
using DataExtractor.Biz.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractor.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/claim-expenses")]
    public class ClaimExpenseController : Controller
    {
        private readonly IExtractorService _extractorService;

        public ClaimExpenseController(IExtractorService extractorService)
        {
            _extractorService = extractorService;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    var requestTextBlock = await reader.ReadToEndAsync();
                    var result = _extractorService.ExtractData(requestTextBlock);
                    return new ObjectResult(result) { StatusCode = (int)HttpStatusCode.OK };
                }
                
            }
            catch (InvalidFormatException ex)
            {
                return new ObjectResult(new Fault { Message = "Invalid Request" }) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                return new ObjectResult(new Fault { Message = "Server Error" }) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
