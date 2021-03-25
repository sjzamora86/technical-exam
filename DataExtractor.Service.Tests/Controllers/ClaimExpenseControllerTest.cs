using DataExtractor.Biz;
using DataExtractor.Biz.Exceptions;
using DataExtractor.Biz.Models;
using DataExtractor.Service.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractor.Service.Tests
{
    [TestClass]
    public class ClaimExpenseControllerTest
    {
        private Mock<IExtractorService> _stubExtractorService;
        private ClaimExpenseController _claimExpenseController;

        [TestMethod]
        public async Task PostExtractData_ValidRequest_Success()
        {
            // Arrange
            var stubRequest = @"Please create an expense claim for the below. Relevant details are marked up as requested
                <expense>
                    <cost_centre>DEV002</cost_centre>
                    <total>1150.00</total>
                    <payment_method>personal card</payment_method>
                </expense>";

            var httpContext = new DefaultHttpContext();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(stubRequest));
            httpContext.Request.Body = stream;
            httpContext.Request.ContentLength = stream.Length;

            var expectedResult = new ClaimExpense()
            {
                CostCentre = "DEV002",
                TotalAmount = 1000,
                GstAmount = 150,
                PaymentMethod = "Credit Card"
            };

            _stubExtractorService = new Mock<IExtractorService>();
            _stubExtractorService.Setup(svc => svc.ExtractData(It.IsAny<string>())).Returns(expectedResult);

            _claimExpenseController = new ClaimExpenseController(_stubExtractorService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            // Act
            var actionResult = await _claimExpenseController.Post();
            var resultStatus = ((ObjectResult)actionResult).StatusCode;
            var resultValue = ((ObjectResult)actionResult).Value as ClaimExpense;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(expectedResult, resultValue);
            Assert.AreEqual((int)HttpStatusCode.OK, resultStatus);
        }

        [TestMethod]
        public async Task PostExtractData_WithInvalidFormatException_BadRequest()
        {
            // Arrange
            var stubRequest = "Invalid request";

            var httpContext = new DefaultHttpContext();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(stubRequest));
            httpContext.Request.Body = stream;
            httpContext.Request.ContentLength = stream.Length;

            _stubExtractorService = new Mock<IExtractorService>();
            _stubExtractorService.Setup(svc => svc.ExtractData(It.IsAny<string>())).Throws(new InvalidFormatException());

            _claimExpenseController = new ClaimExpenseController(_stubExtractorService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            // Act
            var actionResult = await _claimExpenseController.Post();
            var resultStatus = ((ObjectResult)actionResult).StatusCode;
            var resultValue = ((ObjectResult)actionResult).Value as Fault;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, resultStatus);
            Assert.AreEqual("Invalid Request", resultValue.Message);
        }

        [TestMethod]
        public async Task PostExtractData_WithOtherException_InternalServerError()
        {
            // Arrange
            var stubRequest = "Any request";

            var httpContext = new DefaultHttpContext();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(stubRequest));
            httpContext.Request.Body = stream;
            httpContext.Request.ContentLength = stream.Length;

            _stubExtractorService = new Mock<IExtractorService>();
            _stubExtractorService.Setup(svc => svc.ExtractData(It.IsAny<string>())).Throws(new NotImplementedException());

            _claimExpenseController = new ClaimExpenseController(_stubExtractorService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            // Act
            var actionResult = await _claimExpenseController.Post();
            var resultStatus = ((ObjectResult)actionResult).StatusCode;
            var resultValue = ((ObjectResult)actionResult).Value as Fault;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, resultStatus);
            Assert.AreEqual("Server Error", resultValue.Message);
        }
    }
}
