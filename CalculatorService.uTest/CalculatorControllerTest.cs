using CalculatorService.Controllers;
using CalculatorService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CalculatorService.uTest
{
    [TestFixture]
    public class CalculatorControllerTest
    {
        Mock<ILogger<CalculatorController>> _mockLogger;
        Mock<ICalculatorService> _mockCalculatorService;

        CalculatorController _mockCalculatorController;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<CalculatorController>>();
            _mockCalculatorService = new Mock<ICalculatorService>();

            _mockCalculatorController = new CalculatorController(_mockCalculatorService.Object, _mockLogger.Object);
        }

        #region Add Tests
        [Test]
        [Category("Add")]
        public async Task Add_CalculatorService_Called()
        {
            var positiveNumbers = new double[] { 3, 4.2, 6 };
            await _mockCalculatorController.Add(positiveNumbers);

            _mockCalculatorService.Verify(x => x.Add(positiveNumbers), Times.Once);
        }

        [Test]
        [Category("Add")]
        [TestCase(new double[] { 3, 4.2, 6 }, 13.2)]
        [TestCase(new double[] { 3 }, 3)]
        [TestCase(new double[] { 3, 1.1234 }, 4.12)]
        [TestCase(new double[] { 0 }, 0)]
        [TestCase(new double[] { -1, -10.1 }, -11.1)]
        public async Task Add_ValidArguments_Success(double[] input, double expected)
        {

            _mockCalculatorService.Setup(x => x.Add(input)).Returns(Task.FromResult(expected));

            var result = await _mockCalculatorController.Add(input);

            _mockCalculatorService.Verify(x => x.Add(input), Times.Once);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.AreEqual(expected, objectResult.Value);
        }

        [Test]
        [Category("Add")]
        public async Task Add_ArgumentNullException_BadRequest()
        {
            _mockCalculatorService.Setup(x => x.Add(null)).Throws(new ArgumentNullException(It.IsAny<string>()));

            var result = await _mockCalculatorController.Add(null);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual("Unable to process the request: Value cannot be null.", objectResult.Value);
        }
   

        [Test]
        [Category("Add")]
        public async Task Add_Exception_InternalServerError()
        {
            var numbers = new double[] { int.MaxValue, 1.0 };

            _mockCalculatorService.Setup(x => x.Add(numbers)).Throws(new Exception(It.IsAny<string>()));

            var result = await _mockCalculatorController.Add(numbers);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Unable to process the request: Exception of type 'System.Exception' was thrown.", objectResult.Value);
        }
        #endregion

        #region Subtract Tests
        [Test]
        [Category("Subtract")]
        public async Task Subtract_CalculatorService_Called()
        {
            var positiveNumbers = new double[] { 3, 4.2, 6 };
            await _mockCalculatorController.Subtract(positiveNumbers);

            _mockCalculatorService.Verify(x => x.Subtract(positiveNumbers), Times.Once);
        }

        [Test]
        [Category("Subtract")]
        [TestCase(new double[] { 3, 4.2, 6 }, -7.2)]
        [TestCase(new double[] { 3 }, 3)]
        [TestCase(new double[] { 3, 1.1234 }, 1.88)]
        [TestCase(new double[] { 0 }, 0)]
        [TestCase(new double[] { -1, -10.1 }, 9.1)]
        public async Task Subtract_ValidArguments_Success(double[] input, double expected)
        {

            _mockCalculatorService.Setup(x => x.Subtract(input)).Returns(Task.FromResult(expected));

            var result = await _mockCalculatorController.Subtract(input);

            _mockCalculatorService.Verify(x => x.Subtract(input), Times.Once);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.AreEqual(expected, objectResult.Value);
        }

        [Test]
        [Category("Subtract")]
        public async Task Subtract_ArgumentNullException_BadRequest()
        {
            _mockCalculatorService.Setup(x => x.Subtract(null)).Throws(new ArgumentNullException(It.IsAny<string>()));

            var result = await _mockCalculatorController.Subtract(null);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual("Unable to process the request: Value cannot be null.", objectResult.Value);
        }

        [Test]
        [Category("Subtract")]
        public async Task Subtract_Exception_InternalServerError()
        {
            var numbers = new double[] { int.MaxValue, 1.0 };

            _mockCalculatorService.Setup(x => x.Subtract(numbers)).Throws(new Exception(It.IsAny<string>()));

            var result = await _mockCalculatorController.Subtract(numbers);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Unable to process the request: Exception of type 'System.Exception' was thrown.", objectResult.Value);
        }
        #endregion

        #region Multiply Tests
        [Test]
        [Category("Multiply")]
        public async Task Multiply_CalculatorService_Called()
        {
            var positiveNumbers = new double[] { 3, 4.2, 6 };
            await _mockCalculatorController.Multiply(positiveNumbers);

            _mockCalculatorService.Verify(x => x.Multiply(positiveNumbers), Times.Once);
        }

        [Test]
        [Category("Multiply")]
        [TestCase(new double[] { 3, 4.2, 6 }, 75.6)]
        [TestCase(new double[] { 3 }, 3)]
        [TestCase(new double[] { 3, 1.1234 }, 3.37)]
        [TestCase(new double[] { -1, -10.1 }, 10.1)]
        public async Task Multiply_ValidArguments_Success(double[] input, double expected)
        {

            _mockCalculatorService.Setup(x => x.Multiply(input)).Returns(Task.FromResult(expected));

            var result = await _mockCalculatorController.Multiply(input);

            _mockCalculatorService.Verify(x => x.Multiply(input), Times.Once);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.AreEqual(expected, objectResult.Value);
        }

        [Test]
        [Category("Multiply")]
        [TestCase(new double[] { 3, 4.2, 0 }, 0)]
        [TestCase(new double[] { 0 }, 0)]
        public async Task Multiply_ValidArgumentsWithZero_ServiceNotCalled(double[] input, double expected)
        {
            var result = await _mockCalculatorController.Multiply(input);

            _mockCalculatorService.Verify(x => x.Multiply(input), Times.Never);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.AreEqual(expected, objectResult.Value);
        }

        [Test]
        [Category("Multiply")]
        public async Task Multiply_ArgumentNullException_BadRequest()
        {
            _mockCalculatorService.Setup(x => x.Multiply(null)).Throws(new ArgumentNullException(It.IsAny<string>()));

            var result = await _mockCalculatorController.Multiply(null);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual("Unable to process the request: Value cannot be null. (Parameter 'source')", objectResult.Value);
        }

        [Test]
        [Category("Multiply")]
        public async Task Multiply_OverflowException_BadRequest()
        {
            var numbers = new double[] { int.MaxValue, 2 };

            _mockCalculatorService.Setup(x => x.Multiply(numbers)).Throws(new OverflowException(It.IsAny<string>()));

            var result = await _mockCalculatorController.Multiply(numbers);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual("Unable to process the request: Exception of type 'System.OverflowException' was thrown.", objectResult.Value);
        }

        [Test]
        [Category("Multiply")]
        public async Task Multiply_Exception_InternalServerError()
        {
            var numbers = new double[] { 10.1, 1.0 };

            _mockCalculatorService.Setup(x => x.Multiply(numbers)).Throws(new Exception(It.IsAny<string>()));

            var result = await _mockCalculatorController.Multiply(numbers);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Unable to process the request: Exception of type 'System.Exception' was thrown.", objectResult.Value);
        }
        #endregion

        #region Division Tests
        [Test]
        [Category("Division")]
        public async Task Division_CalculatorService_Called()
        {
            var positiveNumbers = new double[] { 3, 4.2, 6 };
            await _mockCalculatorController.Divide(positiveNumbers);

            _mockCalculatorService.Verify(x => x.Divide(positiveNumbers), Times.Once);
        }

        [Test]
        [Category("Division")]
        [TestCase(new double[] { 3, 4.2, 6 }, 0.12)]
        [TestCase(new double[] { 3 }, 3)]
        [TestCase(new double[] { 3, 1.1234 }, 2.67)]
        [TestCase(new double[] { -1, -10.1 }, 0.1)]
        public async Task Division_ValidArguments_Success(double[] input, double expected)
        {

            _mockCalculatorService.Setup(x => x.Divide(input)).Returns(Task.FromResult(expected));

            var result = await _mockCalculatorController.Divide(input);

            _mockCalculatorService.Verify(x => x.Divide(input), Times.Once);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.AreEqual(expected, objectResult.Value);
        }

        [Test]
        [Category("Division")]
        public async Task Division_ArgumentNullException_BadRequest()
        {
            _mockCalculatorService.Setup(x => x.Divide(null)).Throws(new ArgumentNullException(It.IsAny<string>()));

            var result = await _mockCalculatorController.Divide(null);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual("Unable to process the request: Value cannot be null.", objectResult.Value);
        }

        [Test]
        [Category("Division")]
        [TestCase(new double[] { 3, 4.2, 0 })]
        [TestCase(new double[] { -1.1, 0 })]
        public async Task Division_DividebyZeroException_BadRequest(double[] input)
        {
            _mockCalculatorService.Setup(x => x.Divide(input)).Throws(new DivideByZeroException(It.IsAny<string>()));
            
            var result = await _mockCalculatorController.Divide(input);           

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual("Unable to process the request: Exception of type 'System.DivideByZeroException' was thrown.", objectResult.Value);
        }

        [Test]
        [Category("Division")]
        public async Task Division_Exception_InternalServerError()
        {
            var numbers = new double[] { 10.1, 1.0 };

            _mockCalculatorService.Setup(x => x.Divide(numbers)).Throws(new Exception(It.IsAny<string>()));

            var result = await _mockCalculatorController.Divide(numbers);

            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Unable to process the request: Exception of type 'System.Exception' was thrown.", objectResult.Value);
        }
        #endregion

    }
}