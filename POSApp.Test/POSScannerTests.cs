using Moq;
using NUnit.Framework;
using POSApp.ConsoleManager;
using POSApp.POSInterface;
using POSApp.ProductScan;
using POSApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSApp.Test
{
    public class POSScannerTests
    {
        private Mock<IConsoleManager> _mockConsoleManager;
        private Mock<IProductScanner> _mockProductScanner;
        private Mock<IProductRepository> _mockProductRepository;

        private POSScanner _subject;
        [SetUp]
        public void Setup()
        {
            _mockConsoleManager = new Mock<IConsoleManager>();
            _mockProductScanner = new Mock<IProductScanner>();
            _mockProductRepository = new Mock<IProductRepository>();    

            _subject = new POSScanner(_mockConsoleManager.Object, _mockProductScanner.Object, _mockProductRepository.Object);
        }

        [Test]
        public void Run_Verify_ReadProducts()
        {
            //Arrange
            _mockConsoleManager.SetupSequence(x => x.ReadLine())
                .Returns("ABCD")
                .Returns("#");

            //Act
            _subject.Run();

            //Assert
            _mockProductRepository.Verify(x => x.ReadProducts(), Times.Once);
        }

        [Test]
        public void Run_Verify_ConsoleManagerWriteLine()
        {
            //Arrange
            _mockConsoleManager.SetupSequence(x => x.ReadLine())
                .Returns("nmnmnmnmn")
                .Returns("#");

            var totalCost = 0;

            //Act
            _subject.Run();

            //Assert
            _mockConsoleManager.Verify(x => x.WriteLine("Scan Products and press enter when finsihed"), Times.Once);
            _mockConsoleManager.Verify(x=>x.WriteLine($"Your total bill amount is ${totalCost}"),Times.Once);
            _mockConsoleManager.Verify(x=>x.WriteLine("Do you want to continue? Type '#' to exit"),Times.Once);
            _mockConsoleManager.Verify(x => x.WriteLine("Thanks for using the POSScanner"), Times.Once);


        }

        [Test]
        public void Run_Verify_InvalidConsoleWriteLine()
        {
            //Arrange
            _mockConsoleManager.SetupSequence(x => x.ReadLine())
                .Returns("kmkmkmkmkm")
                .Returns("#");
            _mockProductScanner.Setup(x => x.ScanProductAndCalculateCost(It.IsAny<string>())).Returns(-1);

            //Act
            _subject.Run();

            //Assert
            _mockConsoleManager.Verify(x => x.WriteLine("Invalid products are scanned, please scan valid products"), Times.Once);


        }
    }
}
