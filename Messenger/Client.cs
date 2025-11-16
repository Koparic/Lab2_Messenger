using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Messenger.Server;

namespace Messenger
{
    public class Client
    {
        private Form1 form;
        private TcpClient tcpClient;
        private bool isConnected;
        private bool historyLoaded = false;
        private string username;

        public Client(Form1 form, string address, int port, string username)
        {
            this.tcpClient = new TcpClient(address, port);
            isConnected = true;
            this.username = username;
            this.form = form;
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
                        if (message == "Name?")
                        {
                            Console.WriteLine("Сервер запросил Имя. ВЫСЫЛАЮ");
                            buffer = Encoding.UTF8.GetBytes(username);
                            await tcpClient.GetStream().WriteAsync(buffer, 0, buffer.Length);
                            buffer = new byte[tcpClient.ReceiveBufferSize];
                            bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                            message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            try {
                                form.AddHistory(JsonConvert.DeserializeObject<List<MessegeClass>>(message));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Не смог преобразовать Историю:" + ex.Message);
                            }
                        }
                        else
                        {
                            if (!NewUserAdded(message)) form.AddMessage(new MessegeClass(message)); ;
                        }
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

        public bool NewUserAdded(string JSON)
        {
            try
            {
                List<string> users = JsonConvert.DeserializeObject<List<string>>(JSON);
                form.UpdateActiveUsers(users);
                return true;
            }
            catch (Exception ex) { Console.WriteLine("Recieved messege is not UserList"); }
            return false;
        }


        public async Task SendMessage(string message)
        {
            Console.WriteLine("Messege sended from " + username);
            MessegeClass newMssg = new MessegeClass(username, message);
            byte[] buffer = Encoding.UTF8.GetBytes(newMssg.ConvertMessege());
            if (buffer != null)
                await tcpClient.GetStream().WriteAsync(buffer, 0, buffer.Length);
        }

        public static async Task Main(Form1 form, string serverIp, int serverPort, string username)
        {
            Client client = new Client(form, serverIp, serverPort, username);
            form.client = client;
            await client.ListenForMessagesAsync();

            Console.WriteLine("Клиент завершил свою работу");
        }
    }
}
