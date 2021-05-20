using System;

namespace TCP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("{0}Client{0}\n", "-----------------------------------");

            TCP_Client client = new TCP_Client();
            client.CreateConnection();

            string request = Console.ReadLine();
            client.SendMessage(request);

            client.GetMessages();

            client.CloseConnection();
            Console.ReadLine();
        }
    }
}
