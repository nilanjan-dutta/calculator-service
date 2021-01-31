using CalculatorService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorService.Controllers
{
    [Route("api/v1/calculator")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        readonly ICalculatorService _calculatorService;
        readonly ILogger _logger;
        public CalculatorController(ICalculatorService calculatorService, ILogger<CalculatorController> logger)
        {
            _calculatorService = calculatorService;
            _logger = logger;
        }
        // GET: api/calculator/add
        [Route("add")]
        [HttpGet]
        public async Task<ActionResult<double>> Add([FromQuery(Name = "n")] double[] numbers)
        {
            var sw = new Stopwatch();
            sw.Start();
            using (_logger.BeginScope($"{nameof(Add)} with {numbers})"))
            {
                try
                {
                    var result = await _calculatorService.Add(numbers).ConfigureAwait(false);
                    _logger.LogInformation($"Completed {nameof(Add)} in : {sw.Elapsed.TotalSeconds} seconds.");
                    return Ok(Math.Round(result, 2));
                }
                catch (ArgumentNullException ex)
                {
                    _logger.LogError($"Error occurred while processing the {nameof(Add)} request: {ex.Message}", ex.StackTrace);
                    return BadRequest($"Unable to process the request: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while processing the {nameof(Add)} request: {ex.Message}", ex.StackTrace);
                    return StatusCode(500, $"Unable to process the request: {ex.Message}");
                }
            }
        }

        // GET: api/calculator/subtract
        [Route("subtract")]
        [HttpGet]
        public async Task<ActionResult<double>> Subtract([FromQuery(Name = "n")] double[] numbers)
        {
            var sw = new Stopwatch();
            sw.Start();
            using (_logger.BeginScope($"{nameof(Subtract)} with {numbers})"))
            {
                try
                {
                    var result = await _calculatorService.Subtract(numbers).ConfigureAwait(false);
                    _logger.LogInformation($"Completed {nameof(Subtract)} in : {sw.Elapsed.TotalSeconds} seconds.");
                    return Ok(Math.Round(result, 2));
                }
                catch (ArgumentNullException ex)
                {
                    _logger.LogError($"Error occurred while processing the {nameof(Subtract)} request: {ex.Message}", ex.StackTrace);
                    return BadRequest($"Unable to process the request: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while processing the {nameof(Add)} request: {ex.Message}", ex.StackTrace);
                    return StatusCode(500, $"Unable to process the request: {ex.Message}");
                }
            }
        }

        // GET: api/calculator/multiply
        [Route("multiply")]
        [HttpGet]
        public async Task<ActionResult<double>> Multiply([FromQuery(Name = "n")] double[] numbers)
        {
            var sw = new Stopwatch();
            sw.Start();
            using (_logger.BeginScope($"{nameof(Multiply)} with {numbers})"))
            {
                try
                {
                    if (numbers.Any(n => n == 0))
                    {
                        return Ok(0);
                    }

                    var result = await _calculatorService.Multiply(numbers).ConfigureAwait(false);
                    _logger.LogInformation($"Completed {nameof(Multiply)} in : {sw.Elapsed.TotalSeconds} seconds.");
                    return Ok(Math.Round(result, 2));
                }
                catch (Exception ex) when (ex is ArgumentNullException || ex is OverflowException)
                {
                    _logger.LogError($"Error occurred while processing the {nameof(Multiply)} request: {ex.Message}", ex.StackTrace);
                    return BadRequest($"Unable to process the request: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while processing the {nameof(Add)} request: {ex.Message}", ex.StackTrace);
                    return StatusCode(500, $"Unable to process the request: {ex.Message}");
                }
            }
        }

        // GET: api/calculator/divide
        [Route("divide")]
        [HttpGet]
        public async Task<ActionResult<double>> Divide([FromQuery(Name = "n")] double[] numbers)
        {
            var sw = new Stopwatch();
            sw.Start();
            using (_logger.BeginScope($"{nameof(Divide)} with {numbers})"))
            {
                try
                {
                    var result = await _calculatorService.Divide(numbers).ConfigureAwait(false);
                    _logger.LogInformation($"Completed {nameof(Divide)} in : {sw.Elapsed.TotalSeconds} seconds.");
                    return Ok(Math.Round(result, 2));
                }
                catch (Exception ex) when (ex is DivideByZeroException || ex is ArgumentNullException)
                {
                    _logger.LogError($"Error occurred while processing the {nameof(Divide)} request: {ex.Message}", ex.StackTrace);
                    return BadRequest($"Unable to process the request: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while processing the {nameof(Add)} request: {ex.Message}", ex.StackTrace);
                    return StatusCode(500, $"Unable to process the request: {ex.Message}");
                }
            }
        }
    }
}
