using Moq;
using NUnit.Framework;
using POSApp.Log;
using POSApp.Models;
using POSApp.ProductScan;
using POSApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSApp.Test
{
    public class ProductScannerTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<ILogger> _mockLogger;
        private ProductScanner _subject;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger>();
            _mockProductRepository = new Mock<IProductRepository>();
            _subject = new ProductScanner(_mockProductRepository.Object, _mockLogger.Object);

        }
        
        [TestCase("ABCDABA",13.25f)]
        [TestCase("CCCCCCC",6.00f)]
        [TestCase("ABCD",7.25f)]
        [TestCase("AACCCAABDC", 13.25f)]
        [TestCase("CAACACCCACCBCCCDCC", 19.25f)]
        [TestCase("GHTYU",-1f)]
        [TestCase("123345", -1f)]
        [TestCase("@@@@@@@@", -1f)]
        public void ScanProductAndCalculateCost_Returns_Total(string products, float expectedTotal)
        {
            var mockProducts = new List<Product>
            {
                new Product("A")
                {
                    SinglePrice = 1.25f,
                    ComboQuantity = 3,
                    ComboPrice = 3.00f
                },
                new Product("B")
                {
                    SinglePrice = 4.25f,
                    ComboQuantity = 0,
                    ComboPrice = 0.00f
                },
                new Product("C")
                {
                    SinglePrice = 1.00f,
                    ComboQuantity = 6,
                    ComboPrice = 5.00f
                },
                new Product("D")
                {
                    SinglePrice = 0.75f,
                    ComboQuantity = 0,
                    ComboPrice = 0.00f
                }

            };
            _mockProductRepository.Setup(x => x.Products).Returns(mockProducts);

            //Act
            var totalCost = _subject.ScanProductAndCalculateCost(products);

            //Assert
            Assert.AreEqual(expectedTotal, totalCost);


        }

      [Test]
        public void ScanProductAndCalculateCost_Returns_LogInvalidMessage()
        {
            var mockProducts = new List<Product>
            {
                new Product("A")
                {
                    SinglePrice = 1.25f,
                    ComboQuantity = 3,
                    ComboPrice = 3.00f
                },
                new Product("B")
                {
                    SinglePrice = 4.25f,
                    ComboQuantity = 0,
                    ComboPrice = 0.00f
                },
                new Product("C")
                {
                    SinglePrice = 1.00f,
                    ComboQuantity = 6,
                    ComboPrice = 5.00f
                },
                new Product("D")
                {
                    SinglePrice = 0.75f,
                    ComboQuantity = 0,
                    ComboPrice = 0.00f
                }

            };
            _mockProductRepository.Setup(x => x.Products).Returns(mockProducts);
            var products = "POITYU";
            var expectedTotal = -1;
            var expectedMessage = $"This product: {products.ToCharArray()[0]} is not present in product database";
            //Act
            var totalCost = _subject.ScanProductAndCalculateCost(products);

            //Assert
            Assert.AreEqual(expectedTotal, totalCost);
            _mockLogger.Verify(x=>x.LogMessage(expectedMessage), Times.Once);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ScanProductAndCalculateCost_Returns_ArgumentNullException(string products)
        {
            //Arrange
            var expectedTotal = 0;
            var expectedMessage = "Exception: Value cannot be null. (Parameter 'products')";

            //Act
            var total = _subject.ScanProductAndCalculateCost(products);

            //Assert
            Assert.AreEqual(expectedTotal, total);
            _mockLogger.Verify(x=>x.LogMessage(expectedMessage),Times.Once);
        }
    }
}
