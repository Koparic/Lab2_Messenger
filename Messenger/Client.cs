using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    internal class Client
    {
        private TcpClient tcpClient;
        private bool isConnected;
        private string username;
        private ChatClass curChat;

        public Client(string address, int port, string username)
        {
            this.tcpClient = new TcpClient(address, port);
            isConnected = true;
            this.username = username;
        }

        public async Task ListenForMessagesAsync()
        {
            while (isConnected && tcpClient.Connected)
            {
                try
                {
                    NetworkStream stream = tcpClient.GetStream();

                    byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        curChat.AddMessege(message);
                    }
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine(ioEx.Message);
                    break;
                }
                catch (SocketException sockEx)
                {
                    Console.WriteLine(sockEx.Message);
                    break;
                }
            }
        }

        public void Disconnect()
        {
            isConnected = false;
            tcpClient.Close();
        }

        async Task SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await tcpClient.GetStream().WriteAsync(buffer, 0, buffer.Length);
        }

        public static async Task Main(string serverIp, int serverPort, string username)
        {
            Client client = new Client(serverIp, serverPort, username);

            await client.ListenForMessagesAsync();

            Console.WriteLine("Клиент завершил свою работу");
        }
    }
}
