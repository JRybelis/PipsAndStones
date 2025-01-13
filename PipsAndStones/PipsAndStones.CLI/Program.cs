using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PipsAndStones.Interfaces;
using PipsAndStones.LIB.Interfaces.Services;
using PipsAndStones.LIB.Interfaces.Services.IO;
using PipsAndStones.Logic.Services;
using PipsAndStones.Services.IO;

namespace PipsAndStones;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var cli = host.Services.GetRequiredService<IPipsAndStonesCli>();
        cli.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Register services
                services.AddSingleton<IWriter, ConsoleLogger>();
                services.AddSingleton<IReader, ConsoleLogger>();
                services.AddSingleton<IInputValidationService, InputValidationService>();
                services.AddSingleton<IDominoChainSolverService, DominoChainSolverService>();


                // Register CLI
                services.AddSingleton<IPipsAndStonesCli, PipsAndStonesCli>();
                services.AddSingleton<IPipsAndStonesViewService, PipsAndStonesCli>();
            });
}