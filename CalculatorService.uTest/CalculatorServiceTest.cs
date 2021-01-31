using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Svc = CalculatorService.Services;

namespace CalculatorService.uTest
{
    [TestFixture]
    public class CalculatorServiceTest
    {
        Mock<ILogger<Svc.CalculatorService>> _mockLogger;

        Svc.CalculatorService _mockCalculatorService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<Svc.CalculatorService>>();

            _mockCalculatorService = new Svc.CalculatorService(_mockLogger.Object);
        }

        [Test]
        [Category("Add Service Test")]
        [TestCase(new double[] { 3, 4.2, 6 }, 13.2)]
        [TestCase(new double[] { 3 }, 3)]
        [TestCase(new double[] { 3, 1.1234 }, 4.12)]
        [TestCase(new double[] { 0 }, 0)]
        [TestCase(new double[] { -1, -10.1 }, -11.1)]
        public async Task Add_ValidNumbers_Success(double[] input, double expected)
        {
            var result = await _mockCalculatorService.Add(input);
            Assert.AreEqual(expected, Math.Round(result, 2));
        }

        [Test]
        [Category("Add Service Test")]
        public void Add_NullArgument_ThrowsException()
        {            
            var exception = Assert.ThrowsAsync(typeof(ArgumentNullException), () =>  _mockCalculatorService.Add(null));
            Assert.That(exception.Message.Contains("Value cannot be null."));
        }

        [Test]
        [Category("Subtract Service Test")]
        [TestCase(new double[] { 3, 4.2, 6 }, -7.2)]
        [TestCase(new double[] { 3 }, 3)]
        [TestCase(new double[] { 3, 1.1234 }, 1.88)]
        [TestCase(new double[] { 0 }, 0)]
        [TestCase(new double[] { -1, -10.1 }, 9.1)]
        public async Task Subtract_ValidNumbers_Success(double[] input, double expected)
        {
            var result = await _mockCalculatorService.Subtract(input);
            Assert.AreEqual(expected, Math.Round(result, 2));
        }

        [Test]
        [Category("Subtract Service Test")]
        public void Subtract_NullArgument_ThrowsException()
        {
            var exception = Assert.ThrowsAsync(typeof(ArgumentNullException), () => _mockCalculatorService.Subtract(null));
            Assert.That(exception.Message.Contains("Value cannot be null."));
        }

        [Test]
        [Category("Multiply Service Test")]
        [TestCase(new double[] { 3, 4.2, 6 }, 75.6)]
        [TestCase(new double[] { 3 }, 3)]
        [TestCase(new double[] { 3, 1.1234 }, 3.37)]
        [TestCase(new double[] { -1, -10.1 }, 10.1)]
        [TestCase(new double[] { 3, 4.2, 0 }, 0)]
        [TestCase(new double[] { 0 }, 0)]
        public async Task Multiply_ValidNumbers_Success(double[] input, double expected)
        {
            var result = await _mockCalculatorService.Multiply(input);
            Assert.AreEqual(expected, Math.Round(result, 2));
        }

        [Test]
        [Category("Multiply Service Test")]
        public void Multiply_NullArgument_ThrowsException()
        {
            var exception = Assert.ThrowsAsync(typeof(ArgumentNullException), () => _mockCalculatorService.Multiply(null));
            Assert.That(exception.Message.Contains("Value cannot be null."));
        }

        [Test]
        [Category("Divide Service Test")]
        [Category("Division")]
        [TestCase(new double[] { 3, 4.2, 6 }, 0.12)]
        [TestCase(new double[] { 3 }, 3)]
        [TestCase(new double[] { 3, 1.1234 }, 2.67)]
        [TestCase(new double[] { -1, -10.1 }, 0.1)]
        public async Task Divide_ValidNumbers_Success(double[] input, double expected)
        {
            var result = await _mockCalculatorService.Divide(input);
            Assert.AreEqual(expected, Math.Round(result, 2));
        }

        [Test]
        [Category("Divide Service Test")]
        public void Divide_NullArgument_ThrowsException()
        {
            var exception = Assert.ThrowsAsync(typeof(ArgumentNullException), () => _mockCalculatorService.Divide(null));
            Assert.That(exception.Message.Contains("Value cannot be null."));
        }

        [Test]
        [Category("Divide Service Test")]
        [TestCase(new double[] { 3, 4.2, 0 })]
        [TestCase(new double[] { -1.1, 0 })]
        [TestCase(new double[] { 0, 0 })]
        public void Divide_ByZero_ThrowsException(double[] input)
        {
            var exception = Assert.ThrowsAsync(typeof(DivideByZeroException), () => _mockCalculatorService.Divide(input));
            Assert.That(exception.Message.Contains("Attempted to divide by zero."));
        }
    }
}
