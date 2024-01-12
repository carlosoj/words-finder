using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using WordsFinder.Core;
using WordsFinder.Services;
using WordsFinder.Utils;

namespace WordsFinder.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddTransient<IWordsFinderService, WordsFinderService>();
                })
                .Build();
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

            await StartAsync(host.Services);
        }

        private static async Task StartAsync(IServiceProvider serviceProvider)
        {
            var wordFinderService = serviceProvider.GetRequiredService<IWordsFinderService>();
            var exitWord = "exit";

            string? input;
            do
            {
                System.Console.Clear();
                Log.Information($"Enter the path to the file containing the matrix or \"{exitWord}\" to quit", exitWord);
                input = System.Console.ReadLine();

                if (input != exitWord)
                {

                    if (input != null && File.Exists(input))
                    {
                        var matrix = await RequestMatrix(input);
                        IEnumerable<string>? wordsStream;
                        do
                        {
                            wordsStream = RequestValidWordStreamInputs();
                        } while (!wordsStream.Any());

                        try
                        {
                            var wordFinder = new WordFinder(wordFinderService, matrix);
                            var foundWords = wordFinder.Find(wordsStream);
                            DisplayResults(foundWords);
                        }
                        catch (ArgumentNullException)
                        {
                            Log.Warning("The input that is wrong, verify that matrix that you provided has data and NxN size");
                        }
                        catch(InvalidOperationException)
                        {
                            Log.Warning("The input that is wrong, verify that matrix that you provided has data, same amount" +
                                "of rows and columns and does not exceeds 64x64");
                        }
                    }
                    else
                    {
                        Log.Warning("The file does not exist. Check the path and try again");
                    }
                    System.Console.WriteLine("\nPress any key to reset.");
                    System.Console.ReadKey();
                }
            } while (input != exitWord);
        }

        private static async Task<IEnumerable<string>> RequestMatrix(string pathToFileInput)
        {
            IEnumerable<string> matrix;

            if (!File.Exists(pathToFileInput))
            {
                return Enumerable.Empty<string>();
            }
            matrix = await WordsFinderUtils.GetMatrixFromFileAsync(pathToFileInput);

            return matrix;
        }

        private static IEnumerable<string> RequestValidWordStreamInputs()
        {
            System.Console.WriteLine("Enter the words to find separated by a comma.");
            var wordStreamInput = System.Console.ReadLine();
            if (string.IsNullOrWhiteSpace(wordStreamInput))
                return Enumerable.Empty<string>();

            var wordsStream = wordStreamInput.Split(',').Select(s => s.Trim());
            return wordsStream;
        }

        private static void DisplayResults(IEnumerable<string> results)
        {
            
            
            if(!results.Any())
            {
                Log.Warning("No words were found.");
                return;
            }

            System.Console.WriteLine("These are the words found in the matrix ordered by how many times they appear:");
            var resultsCount = results.Count();
            for (int i = 0; i < resultsCount; i++)
            {
                System.Console.WriteLine($"{i+1} - {results.ElementAt(i)}");
            }
        }
    }
}
