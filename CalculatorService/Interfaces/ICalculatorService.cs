using System.Threading.Tasks;

namespace CalculatorService.Interfaces
{
    public interface ICalculatorService
    {
        Task<double> Add(double[] values);
        Task<double> Subtract(double[] values);
        Task<double> Multiply(double[] values);
        Task<double> Divide(double[] values);
    }
}
