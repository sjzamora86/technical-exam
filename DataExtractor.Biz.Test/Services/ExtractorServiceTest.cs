using DataExtractor.Biz.Exceptions;
using DataExtractor.Biz.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataExtractor.Biz.Test
{
    [TestClass]
    public class ExtractorServiceTest
    {
        private ExtractorService _extractorService;

        [TestInitialize]
        public void Initializing()
        {
            _extractorService = new ExtractorService();
        }

        [TestMethod]
        public void ExtractData_ValidMessage_ReturnCompleteCalculatedData()
        {
            // Arrange
            var stubRequest = @"Please create an expense claim for the below. Relevant details are marked up as requested
                <expense>
                    <cost_centre>DEV002</cost_centre>
                    <total>1150.00</total>
                    <payment_method>personal card</payment_method>
                </expense>";
           
            // Act
            var result = _extractorService.ExtractData(stubRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1150, result.TotalAmount);
            Assert.AreEqual(1000, result.TotalAmountExcludedGst);
            Assert.AreEqual(150, result.GstAmount);
            Assert.AreEqual("DEV002", result.CostCentre);
            Assert.AreEqual("personal card", result.PaymentMethod);
        }

        [TestMethod]
        public void ExtractData_MissingCostCentre_ReturnCalculatedDataWithUnknownCostCentre()
        {
            // Arrange
            var stubRequest = @"Please create an expense claim for the below. Relevant details are marked up as requested
                <expense>
                    <total>1150.00</total>
                    <payment_method>personal card</payment_method>
                </expense>";
            
            // Act
            var result = _extractorService.ExtractData(stubRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1150, result.TotalAmount);
            Assert.AreEqual(1000, result.TotalAmountExcludedGst);
            Assert.AreEqual(150, result.GstAmount);
            Assert.AreEqual("UNKNOWN", result.CostCentre);
            Assert.AreEqual("personal card", result.PaymentMethod);
        }

        [TestMethod]
        public void ExtractData_EmptyMessage_ThrowInvalidFormatException()
        {
            // Arrange
            // Act
            // Assert
            Assert.ThrowsException<InvalidFormatException>(() =>
            {
                var result = _extractorService.ExtractData(string.Empty);
            });
        }

        [TestMethod]
        public void ExtractData_InvalidFormatMessage_ThrowInvalidFormatException()
        {
            // Arrange
            var stubRequest = "No xml tags";

            // Act
            // Assert
            Assert.ThrowsException<InvalidFormatException>(() =>
            {
                var result = _extractorService.ExtractData(stubRequest);
            });
        }

        [TestMethod]
        public void ExtractData_MissingTotalTag_ThrowInvalidFormatException()
        {
            // Arrange
            var stubRequest = @"Please create an expense claim for the below. Relevant details are marked up as requested
                <expense>
                    <cost_centre>DEV002</cost_centre>
                    <payment_method>personal card</payment_method>
                </expense>";

            // Act
            // Assert
            Assert.ThrowsException<InvalidFormatException>(() =>
            {
                var result = _extractorService.ExtractData(stubRequest);
            });
        }
    }
}
