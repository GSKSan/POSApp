using Moq;
using NUnit.Framework;
using POSApp.Log;
using POSApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSApp.Test
{
    public class ProductRepositoryTests
    {
        private Mock<ILogger> _mockLogger;

        private ProductRepository _subject;
        private const string TestDataPath = @".\TestData\Products.json";

        [SetUp]
        public void Setup()
        {



            _mockLogger = new Mock<ILogger>();
            _subject = new ProductRepository(TestDataPath, _mockLogger.Object);

            

        }

        [Test]
        public void ReadProducts_ConvertJSONToProducts()
        {
            //Act
            _subject.ReadProducts();

            //Assert
            var products = _subject.Products;
            Assert.AreEqual(4, products.Count);

        }

        [Test]
        public void ReadProducts_EmptyPathException()
        {
            //Arrange
            _subject = new ProductRepository("", _mockLogger.Object);

            //Act
            _subject.ReadProducts();

            //Assert
            _mockLogger.Verify(x => x.LogMessage("Exception: Empty path name is not legal."),Times.Once);
        }

        [Test]
        public void ReadProducts_InvalidPathException()
        {
            //Arrange
            _subject = new ProductRepository("../TestData.JSON", _mockLogger.Object);

            //Act
            _subject.ReadProducts();

            //Assert
            _mockLogger.Verify(x => x.LogMessage(It.IsAny<string>()),Times.Once);
        }
    }
}
