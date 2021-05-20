using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Server
{
    [SupportedOSPlatform("windows")]
    class TCP_Server
    {
        static string ipAdress = "127.0.0.1";
        static int port = 79;
        private Socket socket;
        public Socket clientSocket;

        public void CreateConnection()
        {
            String host = Dns.GetHostName();

            for(int i = 0; i < Dns.GetHostEntry(host).AddressList.Length; i++)
            {
                if (Dns.GetHostEntry(host).AddressList[i].AddressFamily.ToString() == "InterNetwork")
                {
                    ipAdress = Dns.GetHostEntry(host).AddressList[i].ToString();
                    break;
                }
            }

            Console.WriteLine("Server IP: {0}", ipAdress);

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(ipPoint);
        }

        public void Listen()
        {
            socket.Listen(3);
            while (true) {
                clientSocket = socket.Accept();
                Console.WriteLine("Accept");

                GetMessage(clientSocket);
            }
        }

        private void GetMessage(Socket clientSocket)
        {

            StringBuilder inputMessage = new StringBuilder();
            int bytesRead = 0;
            byte[] inputData = new byte[1000];


            bytesRead = clientSocket.Receive(inputData);
            string text = Encoding.UTF8.GetString(inputData, 0, bytesRead);
            
            inputMessage.Append(text);
            Console.WriteLine("Request:\n{0}", inputMessage.ToString());
            Console.WriteLine();

            string answer = GetAnswer(inputMessage.ToString());
            SendMessage(answer);
        }

        private void SendMessage(string outputMessage)
        {
            byte[] outputData = Encoding.UTF8.GetBytes(outputMessage);
            clientSocket.Send(outputData);
        }

        private string GetAnswer(string input)
        {
            string result = "";

            SelectQuery query = new SelectQuery("Win32_UserAccount");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            if (input == "finger")
            {
                foreach (ManagementObject wmiObj in searcher.Get()) {
                    result += "AccountType: " + (string)wmiObj.Properties["AccountType"].Value.ToString() + "\n";
                    result += "Caption: " + (string)wmiObj.Properties["Caption"].Value + "\n";
                    result += "Disabled: " + (string)wmiObj.Properties["Disabled"].Value.ToString() + "\n";
                    result += "Domain: " + (string)wmiObj.Properties["Domain"].Value + "\n";
                    result += "Full Name: " + (string)wmiObj.Properties["FullName"].Value + "\n";
                    result += "InstallDate: " + (string)wmiObj.Properties["InstallDate"].Value + "\n";
                    result += "LocalAccount: " + (string)wmiObj.Properties["LocalAccount"].Value.ToString() + "\n";
                    result += "Name: " + (string)wmiObj.Properties["Name"].Value + "\n";
                    result += "PasswordChangeable: " + (string)wmiObj.Properties["PasswordChangeable"].Value.ToString() + "\n";
                    result += "PasswordExpires: " + (string)wmiObj.Properties["PasswordExpires"].Value.ToString() + "\n";
                    result += "PasswordRequired: " + (string)wmiObj.Properties["PasswordRequired"].Value.ToString() + "\n";
                    //result += "SID: " + (string)wmiObj.Properties["SID"].Value + "\n";
                    //result += "SIDType: " + (string)wmiObj.Properties["SIDType"].Value.ToString() + "\n";
                    //result += "Status: " + (string)wmiObj.Properties["Status"].Value + "\n";
                    //result += "Domain: " + (string)wmiObj.Properties["Domain"].Value + "\n\n";
                    result += "\n";
                }
            }
            else{
                foreach (ManagementObject wmiObj in searcher.Get()) {
                    if ((input == "finger " + (string)wmiObj.Properties["Name"].Value) || 
                        (input == "finger " + (string)wmiObj.Properties["FullName"].Value))
                    {
                        result += "Caption: " + (string)wmiObj.Properties["Caption"].Value + "\n";
                        result += "Domain: " + (string)wmiObj.Properties["Domain"].Value + "\n";
                        result += "Full Name: " + (string)wmiObj.Properties["FullName"].Value + "\n";
                        result += "User: " + (string)wmiObj.Properties["Name"].Value + "\n";
                        result += "\n";
                        //result += "Domain: " + (string)wmiObj.Properties["Domain"].Value + "\n\n";
                    }
                }
            }
            return result;
        }

        public void CloseConnection()
        {
            socket.Close();
        }
    }
}
