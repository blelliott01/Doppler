using System;
using System.Linq;

namespace Doppler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("--tui"))
            {
                DopplerTUI.Run();
                return;
            }

            // --- Normal Doppler console logic ---
            Console.WriteLine("Doppler running in standard mode.");
            Console.WriteLine("Use `dotnet run -- --tui` to launch the text UI.");
            // Call whatever your existing Doppler logic is here
            // e.g., DopplerCore.Run(); or CommandLine.Parse(args);
        }
    }
}
