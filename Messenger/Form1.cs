using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace Messenger
{
    public partial class Form1 : Form
    {
        private BindingList<string> users = new BindingList<string>();
        private MenuStrip menuStrip;
        private ToolStripMenuItem startServerToolStripMenuItem;
        private ToolStripMenuItem stopServerToolStripMenuItem;
        private ListBox chatHistory;
        private TextBox txtMessage;
        private Button btnSend;
        private Panel panelConnection;
        private Panel panelMessege;
        private Label lblIPAddress;
        private TextBox txtIPAddress;
        private Label lblPort;
        private NumericUpDown numPort;
        private Label lblUsername;
        private TextBox txtUsername;
        private Button btnConnect;
        private Button btnDisConnect;
        private ListBox userList;

        private Server _server;
        public Server Server { get { return _server; } set { _server = value; } } 
        private Client _client;
        public Client client { get { return _client; } set { _client = value; } }
        public Form1()
        {
            InitializeComponent();
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.ClientSize = new Size(900, 600);
            this.Text = "Мессенджер";

            chatHistory = new ListBox();
            chatHistory.Dock = DockStyle.Fill;
            chatHistory.Location = new Point(0, 120);
            chatHistory.Height = 400;
            Controls.Add(chatHistory);

            panelMessege = new Panel();
            panelMessege.Dock = DockStyle.Bottom;
            panelMessege.Height = 40;
            Controls.Add(panelMessege);

            txtMessage = new TextBox();
            txtMessage.Location = new Point(0, 10);
            txtMessage.Width = 400;
            panelMessege.Controls.Add(txtMessage);

            btnSend = new Button();
            btnSend.Text = "Отправить";
            btnSend.Location = new Point(405, 10);
            btnSend.AutoSize = true;
            btnSend.Click += BtnSend_Click;
            panelMessege.Controls.Add(btnSend);
            this.AcceptButton = btnSend;

            panelConnection = new Panel();
            panelConnection.Dock = DockStyle.Top;
            panelConnection.Height = 35;
            Controls.Add(panelConnection);

            lblIPAddress = new Label();
            lblIPAddress.Text = "IP:";
            lblIPAddress.AutoSize = true;
            lblIPAddress.Location = new Point(10, 13);
            panelConnection.Controls.Add(lblIPAddress);

            txtIPAddress = new TextBox();
            txtIPAddress.Location = new Point(30, 10);
            panelConnection.Controls.Add(txtIPAddress);

            lblPort = new Label();
            lblPort.Text = "Порт:";
            lblPort.AutoSize = true;
            lblPort.Location = new Point(140, 13);
            panelConnection.Controls.Add(lblPort);

            numPort = new NumericUpDown();
            numPort.Minimum = 1024;
            numPort.Maximum = 65535;
            numPort.Value = 5000;
            numPort.Location = new Point(175, 10);
            numPort.Width = 50;
            panelConnection.Controls.Add(numPort);

            lblUsername = new Label();
            lblUsername.Text = "Имя пользователя:";
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(230, 13);
            panelConnection.Controls.Add(lblUsername);

            txtUsername = new TextBox();
            txtUsername.Location = new Point(336, 10);
            txtUsername.Width = 150;
            panelConnection.Controls.Add(txtUsername);

            btnConnect = new Button();
            btnConnect.Text = "Подключиться";
            btnConnect.AutoSize = true;
            btnConnect.Location = new Point(500, 9);
            btnConnect.Click += BtnConnect_Click;
            panelConnection.Controls.Add(btnConnect);

            btnDisConnect = new Button();
            btnDisConnect.Text = "Отключиться";
            btnDisConnect.AutoSize = true;
            btnDisConnect.Location = new Point(600, 9);
            btnDisConnect.Click += BtnDisConnect_Click;
            panelConnection.Controls.Add(btnDisConnect);

            userList = new ListBox();
            userList.Dock = DockStyle.Right;
            userList.Width = 200;
            Controls.Add(userList);
            userList.DataSource = users;

            menuStrip = new MenuStrip();
            startServerToolStripMenuItem = new ToolStripMenuItem("Запустить сервер");
            stopServerToolStripMenuItem = new ToolStripMenuItem("Остановить сервер");
            stopServerToolStripMenuItem.Enabled = false;
            menuStrip.Items.Add(startServerToolStripMenuItem);
            menuStrip.Items.Add(stopServerToolStripMenuItem);

            startServerToolStripMenuItem.Click += StartServerToolStripMenuItem_Click;
            stopServerToolStripMenuItem.Click += StopServerToolStripMenuItem_Click;

            this.Controls.Add(menuStrip);

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (_client != null) _client.Disconnect();
            if (_server != null) _server.StopListening();
        }


        public void AddMessage(MessegeClass msg)
        {
            Invoke((Action)(() =>
            {
                Console.WriteLine(msg.ToString());
                chatHistory.Items.Add(msg);
                chatHistory.Refresh();
            }));
        }
        public void AddHistory(List<MessegeClass> msges)
        {
            Invoke((Action)(() =>
            {
                Console.WriteLine("Recieved Chat History");
                foreach (var m in msges)
                {
                    chatHistory.Items.Add(m);
                }
                chatHistory.Refresh();
            }));
        }

        public void UpdateActiveUsers(List<string> newUsers)
        {
            Invoke((Action)(() =>
            {
                users.Clear();
                foreach (var user in newUsers.OrderBy(u => u))
                {
                    users.Add(user);
                }
            }));
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text) && _client != null && _client.isConnected)
            {
                _ = client.SendMessage(txtMessage.Text);
                txtMessage.Clear();
            }
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if ((_client == null || !_client.isConnected) && txtUsername.Text.Length > 0)
                _ = Client.Main(this, txtIPAddress.Text, (int)numPort.Value, txtUsername.Text);
        }
        private void BtnDisConnect_Click(object sender, EventArgs e)
        {
            if (_client != null)
            {
                _client.Disconnect();
                chatHistory.Items.Clear();
                chatHistory.Refresh();
                users.Clear();
                userList.Refresh();
                _client = null;
            }
        }

        private void StartServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _server = new Server();
            Console.WriteLine("Сервер запущен");
            startServerToolStripMenuItem.Enabled = false;
            stopServerToolStripMenuItem.Enabled = true;
        }


        private void StopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Server.StopListening();
            Console.WriteLine("Сервер остановлен");
            startServerToolStripMenuItem.Enabled = true;
            stopServerToolStripMenuItem.Enabled = false; 
        }

    }
}
