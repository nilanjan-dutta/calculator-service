using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CalculatorClient
{
    public class Program
    {
        readonly static List<string> validInput = new List<string>() { "A", "S", "M", "D" };
        readonly static Dictionary<string, string> userInputRouteMap = new Dictionary<string, string>();
        static bool isContinue = true;
        static IConfigurationRoot config;
        static void Main(string[] args)
        {
            // For reading the Base Uri of the service
            LoadConfig();

            userInputRouteMap.Add("A", "add");
            userInputRouteMap.Add("S", "subtract");
            userInputRouteMap.Add("M", "multiply");
            userInputRouteMap.Add("D", "divide");

            while (isContinue)
            {
                PerformCalculation();
                Console.WriteLine("To continue press 'Y'");
                var choice = Console.ReadLine();
                isContinue = choice == "Y";
            }
        }

        private static void LoadConfig()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            config = builder.Build();
        }

        private static void PerformCalculation()
        {
            string userInput = string.Empty;

            while (!validInput.Contains(userInput))
            {
                AskForOperation();
                userInput = Console.ReadLine();
            }

            var numbers = GetNumbersFromUser();
            var queryParam = FormQueryParam(numbers);
            if (userInputRouteMap.TryGetValue(userInput, out string subRoute))
            {
                var result = CallService(subRoute, queryParam);
                if (result != null)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine($"The Result is: {result.Result}");
                    Console.WriteLine($"Status Code: {result.StatusCode}");
                    if (!string.IsNullOrWhiteSpace(result.Message))
                    {
                        Console.WriteLine($"Additional Message: {result.Message}");
                    }
                    Console.WriteLine(Environment.NewLine);
                }
            }

            static ResultModel CallService(string userInput, string queryParam)
            {
                try
                {
                    using var client = new HttpClient() { BaseAddress = new Uri(config["BaseUri"]) };
                    var responseTask = client.GetAsync(userInput + queryParam);
                    responseTask.Wait();

                    var result = responseTask.Result;

                    var content = result.Content.ReadAsStringAsync();
                    var resultModel = new ResultModel()
                    {
                        Result = content.Result,
                        StatusCode = result.StatusCode.ToString(),
                        Message = result.ReasonPhrase
                    };
                    return resultModel;
                }
                catch (Exception ex) when 
                    (ex is UriFormatException 
                    || ex is ArgumentNullException
                    || ex is HttpRequestException 
                    || ex is AggregateException 
                    || ex is ObjectDisposedException)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }                
            }

            static void AskForOperation()
            {
                Console.WriteLine("Enter choice: ");
                Console.WriteLine("A- Add");
                Console.WriteLine("S- Subtract");
                Console.WriteLine("M- Multiply");
                Console.WriteLine("D- Divide");
            }

            static void AskForNumbers()
            {
                Console.WriteLine("Enter the numbers, separated by comma: ");
            }

            static List<double> GetNumbersFromUser()
            {
                bool validNumberInput = false;
                List<double> numberInputList = null;
                while (!validNumberInput)
                {
                    AskForNumbers();
                    var numberInput = Console.ReadLine();
                    var numberInputArray = numberInput.Split(',');
                    numberInputList = new List<double>();
                    foreach (var item in numberInputArray)
                    {
                        try
                        {
                            numberInputList.Add(double.Parse(item));
                        }
                        catch (Exception ex) when (ex is FormatException || ex is OverflowException)
                        {
                            Console.WriteLine("Please enter valid numbers");
                            validNumberInput = false;
                            break;
                        }
                    }

                    if (numberInputArray.Length == numberInputList.Count)
                    {
                        validNumberInput = true;
                    }
                }
                return numberInputList;
            }

            static string FormQueryParam(List<double> numbers)
            {
                var builder = new StringBuilder();
                builder.Append("?");

                foreach (var num in numbers)
                {
                    builder.Append("n=");
                    builder.Append(num);
                    builder.Append("&");
                }

                return builder.ToString();
            }
        }
    }
}
