using CalculatorService.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorService.Services
{
    public class CalculatorService : ICalculatorService
    {
        readonly ILogger _logger;
        public CalculatorService(ILogger<CalculatorService> logger)
        {
            _logger = logger;
        }
        public async Task<double> Add(double[] values)
        {
            try
            {
                _logger.LogInformation("Performing Addition...");
                return await Task.Run(() => values.Sum());
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"An error occurred while adding values: {ex.Message}", ex.StackTrace);
                throw;
            }
        }

        public async Task<double> Subtract(double[] values)
        {
            try
            {
                _logger.LogInformation("Performing Subtraction...");
                return await Task.Run(() => { return values.Aggregate((a, b) => a - b); });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"An error occurred while adding values: {ex.Message}", ex.StackTrace);
                throw;
            }
        }

        public async Task<double> Multiply(double[] values)
        {
            try
            {
                _logger.LogInformation("Performing Multiplication...");
                return await Task.Run(() => { return values.Aggregate((a, b) => a * b); });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"An error occurred while multiplying values: {ex.Message}", ex.StackTrace);
                throw;
            }

        }

        public async Task<double> Divide(double[] values)
        {
            try
            {
                _logger.LogInformation("Performing Division...");
                var result = await Task.Run(() => { return values.Aggregate((a, b) => a / b); });
                if(result == double.PositiveInfinity || result == double.NegativeInfinity || double.IsNaN(result))
                {
                    throw new DivideByZeroException();
                }
                return result;
            }
            catch (Exception ex) when (ex is DivideByZeroException || ex is ArgumentNullException)
            {
                _logger.LogError($"An error occurred while dividing values: {ex.Message}", ex.StackTrace);
                throw;
            }
        }
    }
}
