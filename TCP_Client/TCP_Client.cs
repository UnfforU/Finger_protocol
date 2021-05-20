using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Client
{
    class TCP_Client
    {
        static string ipAdress = "127.0.0.1";
        static int port = 79;
        public Socket socket;

        public void CreateConnection()
        {
            Console.WriteLine("Input server IP");
            ipAdress = Console.ReadLine();

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipPoint);

            Console.WriteLine("Connection open\nInput request");
        }

        public void GetMessages()
        {
            StringBuilder inputMessage = new StringBuilder();
            int bytesRead = 0;
            byte[] inputData = new byte[1000];

            bytesRead = socket.Receive(inputData);
            string text = Encoding.UTF8.GetString(inputData, 0, bytesRead);
            inputMessage.Append(text);

            Console.WriteLine("\nAnswer:\n{0}", inputMessage.ToString());
        }

        public void SendMessage(string outputMessage)
        {
            byte[] outputData = Encoding.UTF8.GetBytes(outputMessage);
            socket.Send(outputData);
        }

        public void CloseConnection()
        {
            socket.Close();
            Console.WriteLine("Connection close");
        }
    }
}
