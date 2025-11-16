using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    public class Server
    {
        private TcpListener listener;
        private List<ActiveClients> curActiveClients;
        
        public struct ActiveClients
        {
            public TcpClient tcpclient;
            public string username;
        }

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
                Console.WriteLine("Новый клиент подключился!");
                NetworkStream stream = client.GetStream();
                ActiveClients newC = new ActiveClients();
                byte[] buffer;
                buffer = Encoding.UTF8.GetBytes("Name?");
                await stream.WriteAsync(buffer, 0, buffer.Length);

                buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    newC.username = dataReceived;
                    newC.tcpclient = client;
                    curActiveClients.Add(newC);
                }
                _ = HandleClient(newC);
            }
        }

        private async Task HandleClient(ActiveClients client)
        {
            while (true)
            {
                try
                {
                    NetworkStream stream = client.tcpclient.GetStream();

                    byte[] buffer = new byte[client.tcpclient.ReceiveBufferSize];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        MessegeClass newMSSG = new MessegeClass(dataReceived);
                        if (newMSSG.Messege.recipient != null)
                        {
                            foreach (ActiveClients actClient in curActiveClients)
                            {
                                if (newMSSG.Messege.recipient == "All" || newMSSG.Messege.recipient == actClient.username)
                                {
                                    await actClient.tcpclient.GetStream().WriteAsync(buffer, 0, buffer.Length);
                                }
                            }
                        }
                    }
                }

                catch (IOException ex)
                {
                    Console.WriteLine("Исключение при приёме данных: " + ex.Message);
                    curActiveClients.Remove(client);
                    client.tcpclient.Close();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Общая ошибка при обработке клиента: {ex.Message}");
                    curActiveClients.Remove(client);
                    client.tcpclient.Close();
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
