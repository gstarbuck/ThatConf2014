using System;

namespace Sign
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://yahara-gstarb81:9000";

            // Start the OWIN host
            var service = Microsoft.Owin.Hosting.WebApp.Start<StartupConfig>(url: baseAddress);
            Console.WriteLine("Service started, listening at " + baseAddress);
            Console.WriteLine("Hit any key to terminate");

            // Listen for a keypress to terminate
            Console.ReadLine();

            // When terminated, dispose the service
            service.Dispose();
        }
    }
}
