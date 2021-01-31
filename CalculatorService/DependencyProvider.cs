using CalculatorService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Service = CalculatorService.Services;

namespace CalculatorService
{
    [ExcludeFromCodeCoverage]
    public static class DependencyProvider
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICalculatorService, Service.CalculatorService>();
        }
    }
}
