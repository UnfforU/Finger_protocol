using System;
using System.Runtime.Versioning;

namespace TCP_Server
{
    [SupportedOSPlatform("windows")]
    class Program
    {
        static void Main(string[] args)
        {
            TCP_Server server = new TCP_Server();
            Console.CancelKeyPress += ConsoleClose_Event;

            Console.WriteLine("{0}Server{0}\n", "-----------------------------------");

            server.CreateConnection();

            server.Listen();

            void ConsoleClose_Event(object sender, ConsoleCancelEventArgs e)
            {
                server.CloseConnection();
            }
        }
    }
}
