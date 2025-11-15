using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    internal class Server
    {
        private TcpListener listener;
        private List<TcpClient> activeClients;
        public void StartListening(int port, string localAddr)
        {
            listener = new TcpListener(IPAddress.Parse(localAddr), port);
            listener.Start();

            AcceptClientsAsync();
        }

        private async Task AcceptClientsAsync()
        {
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                activeClients.Add(client);
                Console.WriteLine("Новый клиент подключился!");

                _ = HandleClient(client);
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            while (true)
            {
                try
                {
                    NetworkStream stream = client.GetStream();

                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        //обработчик получения сообщений
                        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Полученное сообщение: {dataReceived}");
                    }
                }

                catch (IOException ex)
                {
                    Console.WriteLine("Исключение при приёме данных: " + ex.Message);
                    activeClients.Remove(client);
                    client.Close();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Общая ошибка при обработке клиента: {ex.Message}")
                    activeClients.Remove(client);
                    client.Close();
                    break;
                }
            }
        }

        public void StopListening()
        {
            listener?.Stop();
        }

        public Server(int port = 9000, string localAddr = "192.168.0.5")
        {
            this.StartListening(port, localAddr);
        }

    }
}
