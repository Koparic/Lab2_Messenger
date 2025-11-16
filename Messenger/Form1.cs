using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Messenger
{
    public partial class Form1 : Form
    {
        private ObservableCollection<Message> messages = new ObservableCollection<Message>();
        private BindingList<string> users = new BindingList<string>();
        private MenuStrip menuStrip;
        private ToolStripMenuItem startServerToolStripMenuItem;
        private ToolStripMenuItem stopServerToolStripMenuItem;
        private ListBox chatHistory;
        private TextBox txtMessage;
        private Button btnSend;
        private Label lblIPAddress;
        private TextBox txtIPAddress;
        private Label lblPort;
        private NumericUpDown numPort;
        private Label lblUsername;
        private TextBox txtUsername;
        private Button btnConnect;
        private ListBox userList;

        private Server server;
        private Client client;
        public Form1()
        {
            InitializeComponent();
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.ClientSize = new Size(800, 600);
            this.Text = "Мессенджер";

            menuStrip = new MenuStrip();
            startServerToolStripMenuItem = new ToolStripMenuItem("Запустить сервер");
            stopServerToolStripMenuItem = new ToolStripMenuItem("Остановить сервер");
            stopServerToolStripMenuItem.Enabled = false;
            menuStrip.Items.Add(startServerToolStripMenuItem);
            menuStrip.Items.Add(stopServerToolStripMenuItem);

            startServerToolStripMenuItem.Click += StartServerToolStripMenuItem_Click;
            stopServerToolStripMenuItem.Click += StopServerToolStripMenuItem_Click;

            this.Controls.Add(menuStrip);

            chatHistory = new ListBox();
            chatHistory.Dock = DockStyle.Fill;
            chatHistory.Location = new Point(0, 120);
            chatHistory.Height = 400;
            Controls.Add(chatHistory);

            txtMessage = new TextBox();
            txtMessage.Dock = DockStyle.Bottom;
            txtMessage.Location = new Point(0, 520);
            txtMessage.Width = 600;
            Controls.Add(txtMessage);

            btnSend = new Button();
            btnSend.Text = "Отправить";
            btnSend.Dock = DockStyle.Bottom;
            btnSend.Location = new Point(600, 520);
            btnSend.Click += BtnSend_Click;
            Controls.Add(btnSend);

            lblIPAddress = new Label();
            lblIPAddress.Text = "IP:";
            lblIPAddress.Location = new Point(10, 10);
            Controls.Add(lblIPAddress);

            txtIPAddress = new TextBox();
            txtIPAddress.Location = new Point(100, 10);
            Controls.Add(txtIPAddress);

            lblPort = new Label();
            lblPort.Text = "Порт:";
            lblPort.Location = new Point(10, 40);
            Controls.Add(lblPort);

            numPort = new NumericUpDown();
            numPort.Minimum = 1024;
            numPort.Maximum = 65535;
            numPort.Value = 5000;
            numPort.Location = new Point(100, 40);
            Controls.Add(numPort);

            lblUsername = new Label();
            lblUsername.Text = "Имя пользователя:";
            lblUsername.Location = new Point(10, 70);
            Controls.Add(lblUsername);

            txtUsername = new TextBox();
            txtUsername.Location = new Point(100, 70);
            Controls.Add(txtUsername);

            btnConnect = new Button();
            btnConnect.Text = "Подключиться";
            btnConnect.Location = new Point(10, 100);
            btnConnect.Click += BtnConnect_Click;
            Controls.Add(btnConnect);

            userList = new ListBox();
            userList.Dock = DockStyle.Right;
            userList.Width = 200;
            Controls.Add(userList);
            chatHistory.DataSource = messages;
            userList.DataSource = users;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void AddMessage(Message msg)
        {
            Invoke((Action)(() =>
            {
                messages.Add(msg);
                chatHistory.SelectedIndex = messages.Count - 1; 
            }));
        }

        public void UpdateActiveUsers(List<Server.ActiveClients> newUsers)
        {
            Invoke((Action)(() =>
            {
                users.Clear();
                foreach (var user in newUsers.OrderBy(u => u.username))
                {
                    users.Add(user.username);
                }
            }));
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                // Логика отправки сообщения
                txtMessage.Clear();
            }
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            //логика подключения к серверу
        }

        private void StartServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Логика старта сервера
            Console.WriteLine("Сервер запущен");
            startServerToolStripMenuItem.Enabled = false;
            stopServerToolStripMenuItem.Enabled = true;
        }


        private void StopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Логика остановки сервера
            Console.WriteLine("Сервер остановлен");
            startServerToolStripMenuItem.Enabled = true;
            stopServerToolStripMenuItem.Enabled = false; 
        }

    }
}
