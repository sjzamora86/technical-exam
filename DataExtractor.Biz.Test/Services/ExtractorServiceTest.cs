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

            // Act

            // Assert
        }

        [TestMethod]
        public void ExtractData_MissingCostCentre_ReturnCalculatedDataWithUnknownCostCentre()
        {
            // Arrange

            // Act

            // Assert
        }

        [TestMethod]
        public void ExtractData_InvalidFormatMessage_ThrowInvalidFormatException()
        {
            // Arrange

            // Act

            // Assert
        }

        [TestMethod]
        public void ExtractData_MissingTotalTag_ThrowInvalidFormatException()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
