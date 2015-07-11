using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace RollAnd
{
    public partial class Form1 : Form
    {
        int useport = 54047;
        public Client_TCP client;
        public Server_TCP server;
        char Split = '^';
        public DataTable dt = new DataTable();
        public DataTable list = new DataTable();
        Random ro = new Random();
        string name = "无此IP数据";
        public Form1()
        {
            InitializeComponent();
            loadform();
        }

        public void loadform()
        {
            client = new Client_TCP();
            client.OnConnectFail += client_OnConnectTimeout;
            client.OnConnectSuccess += client_OnConnectSuccess;
            client.OnDropped += client_OnDisconnect;
            client.OnServerDisconnect += client_OnRemoteDisconnect;
            client.OnReceive += clCMD;
            list.Columns.Add("IP");
            list.Columns.Add("Name");
            list.Columns.Add("NO");
            list.Rows.Add("172.60.100.222", "彪哥", "-1");
            list.Rows.Add("172.16.132.86", "振宇", "-1");
            list.Rows.Add("172.60.102.217", "通通", "-1");
            list.Rows.Add("172.60.102.80", "邱邱", "-1");
            list.Rows.Add("172.60.102.29", "杉杉", "-1");
            list.Rows.Add("172.16.132.12", "帅杰", "-1");
            list.Rows.Add("172.60.102.165", "丽丽", "-1");
            list.Rows.Add("127.0.0.1", "本地", "-1");

            for (int i = 0; i < list.Rows.Count; i++)
            {
                comboBox1.Items.Add(list.Rows[i][1].ToString() + "：" + list.Rows[i][0].ToString());
            }
        }

        void client_OnRemoteDisconnect(Form1.Client_TCP client)//客户端收到服务器提出的断开连接做的事
        {
            Invoke(new EventHandler(delegate
            {
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                textBox1.ReadOnly = false;
                comboBox1.Enabled = true;
                button_flash.Enabled = true;
                button1.Enabled = true;
                button_roll.Enabled = false;
                button_flash.Text = "连接";

            }));

            MessageBox.Show("服务器断开连接。", "提示");
        }

        void client_OnConnectTimeout()
        {
            MessageBox.Show("无法连接，连接超时。", "提示");
            Invoke(new EventHandler(delegate
            {
                button_flash.Text = "连接";
                button_flash.Enabled = true;
                button1.Text = "作为服务器";
                comboBox1.Enabled = true;
                textBox1.ReadOnly = false;
            }));
        }

        public void checkip()
        {
            client.Connect(comboBox1.Text.Split('：')[1], useport, 1000);
        }

        public void client_OnDisconnect(Client_TCP Client)
        {
            Invoke(new EventHandler(delegate
            {
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                textBox1.ReadOnly = false;
                comboBox1.Enabled = true;
                button_flash.Text = "连接";
                button1.Text = "作为服务器";
                button_Reset.Enabled = false;
                comboBox1.Enabled = true;
                textBox1.ReadOnly = false;
                button_roll.Enabled = false;
                button_flash.Enabled = true;
            }));
            //server.StopListen();
            MessageBox.Show("连接断开,请重新连接。", "提示");

        }

        //public void client_OnConnectFail()
        //{
        //    MessageBox.Show("与选择或输入的IP地址连接失败，请确认IP地址是否正确","提示");
        //}

        public void client_OnConnectSuccess()
        {
            Invoke(new EventHandler(delegate
            {
                
                comboBox1.Enabled = false;
                if (button1.Text != "停止")
                {
                    button1.Enabled = false;
                    button_flash.Text = "取消";
                    button_flash.Enabled = true;
                }
                button_roll.Enabled = true;
                textBox1.ReadOnly = true;
            }));
            client.Send(StringToByteArray("Info" + Split, Encoding.UTF8));
        }

        private void getuser()
        {
            string tep = "";
            if (dataGridView2.Rows.Count != 0)
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    tep += Split + dataGridView2.Rows[i].Cells[0].Value.ToString()
                        + Split + dataGridView2.Rows[i].Cells[1].Value.ToString();
                }

            }
            //server.SendAll(StringToByteArray("Come" + Split + textBox1.Text + tep + Split, Encoding.UTF8));
            server.Multicast(server.Clients, StringToByteArray("Come" + Split + textBox1.Text + tep + Split, Encoding.UTF8));
        }

        public void seCMD(Client_TCP client, byte[] ByteArray)
        {
            string msg = ByteArrayToString(ByteArray, Encoding.UTF8);
            string[] temp = msg.Split(Split);
            switch (temp[0])
            {
                case "Info"://刷新列表
                    {
                        //string ip = ((IPEndPoint)index.socket.RemoteEndPoint).Address.ToString();
                        name = "无此IP数据";
                        string ip = client.Get_RemoteIP();
                        for (int i = 0; i < list.Rows.Count; i++)
                        {
                            if (ip == list.Rows[i][0].ToString())
                            {
                                name = list.Rows[i][1].ToString();
                                break;
                            }
                        }
                        string tep = "";
                        if (dataGridView2.Rows.Count != 0)
                        {
                            for (int i = 0; i < dataGridView2.Rows.Count; i++)
                            {
                                tep += Split + dataGridView2.Rows[i].Cells[0].Value.ToString()
                                    + Split + dataGridView2.Rows[i].Cells[1].Value.ToString();
                            }
                        }
                        server.Multicast(server.Clients, StringToByteArray("Come" + Split + textBox1.Text + tep + Split + name + Split + ip, Encoding.UTF8));
                        //Thread.Sleep(100);
                        if (dataGridView1.Rows.Count != 0)
                        {
                            string info = "Info" + Split;
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                info += dataGridView1.Rows[i].Cells[0].Value.ToString()
                                    + Split + dataGridView1.Rows[i].Cells[1].Value.ToString()
                                + Split + dataGridView1.Rows[i].Cells[2].Value.ToString() + Split;
                            }
                            //client.Send(info);
                            client.Send(StringToByteArray(info, Encoding.UTF8));
                        }
                        else
                        {
                            // client.Send("Fail" + Split + "连接成功。");
                            client.Send(StringToByteArray("Mess" + Split + "连接成功。", Encoding.UTF8));
                        }
                        break;
                    }
                case "Roll":
                    {
                        string ip = client.Get_RemoteIP();
                        Boolean rolled = false;
                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                        {
                            if (ip == dataGridView1.Rows[j].Cells[0].Value.ToString())
                            {
                                rolled = true;
                                //index.Send("Fail" + Split + "你已经Roll过点了。");
                                client.Send(StringToByteArray("Mess" + Split + "你已经Roll过点了。", Encoding.UTF8));
                                break;
                            }
                        }
                        if (!rolled)
                        {
                            string NO = ro.Next(Int32.Parse(textBox1.Text)).ToString();
                            for (int i = 0; i < list.Rows.Count; i++)
                            {
                                if (ip == list.Rows[i][0].ToString())
                                {
                                    name = list.Rows[i][1].ToString();
                                    break;
                                }
                            }

                            server.Multicast(server.Clients, StringToByteArray("Roll" + Split + ip + Split + name + Split + NO, Encoding.UTF8));
                            //server.SendAll("Roll" + Split + ip + Split + name + Split + NO);
                        }

                        break;
                    }
            }
        }

        public void clCMD(Client_TCP client, byte[] ByteArray)
        {
            string cmd = ByteArrayToString(ByteArray, Encoding.UTF8);
            string[] temp = cmd.Split(Split);
            switch (temp[0])
            {
                case "Come":
                    {
                        Invoke(new EventHandler(delegate
                        {
                            dataGridView2.Rows.Clear();
                            textBox1.Text = temp[1];
                            for (int i = 2; i < temp.Length - 1; i++)
                            {
                                dataGridView2.Rows.Add(temp[i], temp[++i]);
                                dataGridView2.ClearSelection();
                            }
                        }));

                        break;
                    }
                case "Info"://刷新列表
                    {
                        Invoke(new EventHandler(delegate
                        {

                            dataGridView1.Rows.Clear();
                            for (int i = 1; i < temp.Length - 1; i++)
                            {
                                dataGridView1.Rows.Add(temp[i], temp[++i], temp[++i]);
                            }
                        }));
                        break;
                    }
                case "Roll":
                    {
                        Invoke(new EventHandler(delegate
                        {
                            dataGridView1.Rows.Add(temp[1], temp[2], temp[3]);
                            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
                        }));
                        break;
                    }
                case "Reset":
                    {
                        Invoke(new EventHandler(delegate
                        {
                            dataGridView1.Rows.Clear();
                        }));
                        break;
                    }
                case "Mess":
                    {
                        Invoke(new EventHandler(delegate
                        {
                            MessageBox.Show(temp[1], "提示");
                        }));
                        break;
                    }
            }
        }

        private void button_flash_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                switch (button_flash.Text)
                {
                    case "连接":
                        {
                            if (button1.Text == "停止")
                            {
                                server.StopListen();
                                button1.Text = "作为服务器";
                            }
                            button_flash.Text = "连接中";
                            button_flash.Enabled = false;
                            checkip();
                            break;
                        }
                    case "取消":
                        {
                            if (client != null)
                            {
                                client.Shutdown(true);
                            }
                            button1.Enabled = true;
                            button_roll.Enabled = false;
                            button_flash.Text = "连接";
                            dataGridView1.Rows.Clear();
                            dataGridView2.Rows.Clear();
                            textBox1.ReadOnly = false;
                            comboBox1.Enabled = true;
                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("请选择IP连接。", "提示");
            }


        }

        private void button_roll_Click(object sender, EventArgs e)
        {
            if (button1.Text == "停止" && dataGridView1.Rows.Count < 1)
            {
                MessageBox.Show("为了防止服务端作弊，你必须等待客户端Roll点后才能Roll!", "提示");
            }
            else
            {
                // client.Send("Roll");
                client.Send(StringToByteArray("Roll", Encoding.UTF8));
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (button1.Text)
            {
                case "作为服务器":
                    {
                        if (comboBox1.Text != "")
                        {
                            dataGridView1.Rows.Clear();
                            button1.Text = "停止";
                            comboBox1.Enabled = false;
                            button_Reset.Enabled = true;
                            server = new Server_TCP();
                            server.OnBindFail += server_OnBindFail;
                            //server.OnServerDisconnect += server_OnListenSuccess;
                            server.OnReceive += seCMD;
                            server.OnClientDropped += server_OnClientDropped;
                            server.OnClientDisconnect += server_OnClientDisconnect;
                            button_flash.Enabled = false;
                            textBox1.ReadOnly = true;
                            if (server.Listen(comboBox1.Text.Split('：')[1], useport, 10))
                            {
                                client.Connect(comboBox1.Text.Split('：')[1], useport, 1000);
                            }
                            
                            break;
                        }
                        else
                        {
                            MessageBox.Show("请选择自己的IP作为服务器地址", "提示");
                            break;
                        }

                    }
                case "停止":
                    {
                        DialogResult dr= MessageBox.Show("所有数据和客户端会断开，确定要断开服务器？","提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.OK)
                        {
                            server.StopListen();
                            button1.Text = "作为服务器";
                            button_Reset.Enabled = false;
                            comboBox1.Enabled = true;
                            textBox1.ReadOnly = false;
                            button_roll.Enabled = false;
                            button_flash.Enabled = true;
                            break;
                        }
                        else
                        { 
                            break;
                        }
                        
                    }
            }
        }

        void server_OnClientDropped(Form1.Client_TCP client)
        {
            string temp = client.IP;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Cells[1].Value.ToString() == temp)
                {
                    dataGridView2.Rows.RemoveAt(i);
                    dataGridView2.ClearSelection();
                    getuser();
                    break;
                }
            }
        }

        void server_OnClientDisconnect(Form1.Client_TCP client)//服务端收到某个客户端主动提出的断开连接发生的事件
        {

            Invoke(new EventHandler(delegate
            {
                string temp = client.IP;
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells[1].Value.ToString() == temp)
                    {
                        dataGridView2.Rows.RemoveAt(i);
                        dataGridView2.ClearSelection();
                        getuser();
                        break;
                    }
                }
            }));
        }

        void server_OnBindFail()
        {
            Invoke(new EventHandler(delegate
            {
                button1.Text = "作为服务器";
                button_Reset.Enabled = false;
                comboBox1.Enabled = true;
                textBox1.ReadOnly = false;
                button_roll.Enabled = false;
                button_flash.Enabled = true;
            }));
            MessageBox.Show("启动服务器失败，你选择的本机IP地址（" + comboBox1.Text.Split('：')[1] + "）错误或" + useport + "端口已被其他程序占用！", "提示");
        }

        //void server_OnListenSuccess()
        //{

        //    client.Connect(comboBox1.Text.Split('：')[1], useport, 1000);
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("所有客户端和服务端的Roll点将会丢失，确定要重新Roll点？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                server.Multicast(server.Clients, StringToByteArray("Reset", Encoding.UTF8));
                
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }


        #region   网络连接

        public class Server_TCP
        {
            public delegate void Delegate_OnClientEvent(Client_TCP client);
            /// <summary>
            /// 与新的主动连接到服务端的客户端成功建立连接时引发的事件
            /// </summary>
            public event Delegate_OnClientEvent OnAddNewClient;
            /// <summary>
            /// 客户端主动断开连接时引发的事件
            /// </summary>
            public event Delegate_OnClientEvent OnClientDisconnect;
            /// <summary>
            /// 与客户端之间的连接发生异常时引发的事件
            /// </summary>
            public event Delegate_OnClientEvent OnClientDropped;
            /// <summary>
            /// 绑定端口失败时引发的事件
            /// </summary>
            public event Client_TCP.Delegate_Do OnBindFail;
            /// <summary>
            /// 收到客户端发来的消息时引发的事件
            /// </summary>
            public event Client_TCP.Delegate_Receive OnReceive;

            Socket Listener;
            public List<Client_TCP> Clients = new List<Client_TCP>();

            /// <summary>
            /// 开始监听端口并等待客户端主动连接到服务器
            /// </summary>
            /// <param name="Port">监听的端口</param>
            /// <param name="ListeningQueueLength">来不及处理（即接受客户端的连接）的连接请求队列的最大值</param>
            /// <returns></returns>
            public bool Listen(string IP, int Port, int ListeningQueueLength)
            {
                Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    Listener.Bind(new IPEndPoint(IPAddress.Parse(IP), Port));
                }
                catch
                {
                    if (OnBindFail != null)
                    {
                        OnBindFail();
                    }
                    return false;
                }
                try
                {
                    Listener.Listen(ListeningQueueLength);
                    Listener.BeginAccept(new AsyncCallback(AcceptCallBack), Listener);
                    return true;
                }
                catch
                {

                }
                return false;
            }

            private void AcceptCallBack(IAsyncResult ar)
            {
                try
                {
                    Client_TCP temp_client = new Client_TCP(Listener.EndAccept(ar));
                    Listener.BeginAccept(new AsyncCallback(AcceptCallBack), Listener);
                    temp_client.IP = temp_client.Get_RemoteIP();
                    temp_client.OnServerDisconnect += Temp_client_OnServerDisconnect;
                    temp_client.OnDropped += Temp_client_OnDropped;
                    if (OnReceive != null)
                    {
                        temp_client.OnReceive += OnReceive;
                    }
                    Clients.Add(temp_client);
                    if (OnAddNewClient != null)
                    {
                        OnAddNewClient(temp_client);
                    }
                    temp_client.Receive();
                }
                catch
                {

                }
            }

            private void Temp_client_OnDropped(Client_TCP client)
            {
                Clients.Remove(client);
                if (OnClientDropped != null)
                {
                    OnClientDropped(client);
                }
            }

            private void Temp_client_OnServerDisconnect(Client_TCP client)
            {
                Clients.Remove(client);
                if (OnClientDisconnect != null)
                {
                    OnClientDisconnect(client);
                }
            }

            /// <summary>
            /// 停止监听端口并主动断开所有与客户端的连接
            /// </summary>
            public void StopListen()
            {
                Listener.Close();
                Listener.Dispose();
                for (int i = Clients.Count - 1; i > -1; i--)
                {
                    RemoveClient(Clients[i]);
                }
            }

            /// <summary>
            /// 向多个客户端发送
            /// </summary>
            /// <param name="clients">要发送的客户端列表</param>
            /// <param name="ByteArray">要发送的内容</param>
            public void Multicast(List<Client_TCP> clients, byte[] ByteArray)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    clients[i].Send(ByteArray);
                }
            }

            /// <summary>
            /// 断开与指定客户端的连接
            /// </summary>
            /// <param name="client"></param>
            public void RemoveClient(Client_TCP client)
            {
                Clients.Remove(client);
                client.Shutdown(true);
            }
        }

        public class Client_TCP
        {
            public delegate void Delegate_Do();
            /// <summary>
            /// 主动连接服务器并成功建立连接时引发的事件
            /// </summary>
            public event Delegate_Do OnConnectSuccess;
            /// <summary>
            /// 主动连接服务器并建立连接失败时引发的事件
            /// </summary>
            public event Delegate_Do OnConnectFail;
            public delegate void Delegate_Receive(Client_TCP client, byte[] ByteArray);
            /// <summary>
            /// 收到服务端发来的消息时引发的事件
            /// </summary>
            public event Delegate_Receive OnReceive;
            /// <summary>
            /// 服务器主动断开连接时引发的事件
            /// </summary>
            public event Server_TCP.Delegate_OnClientEvent OnServerDisconnect;
            /// <summary>
            /// 与服务端之间的连接发生异常时引发的事件
            /// </summary>
            public event Server_TCP.Delegate_OnClientEvent OnDropped;

            Socket socket;
            System.Timers.Timer Timer_Connect, Timer_KeepAlive;
            bool IsNotSending = true;
            List<byte[]> List_SendBuffer = new List<byte[]>();
            public string IP;

            public Client_TCP()
            {
                InitKeepAlive();
            }

            public Client_TCP(Socket s)
            {
                socket = s;
                InitKeepAlive();
            }

            private void InitKeepAlive()
            {
                Timer_KeepAlive = new System.Timers.Timer(5000);
                Timer_KeepAlive.AutoReset = false;
                Timer_KeepAlive.Elapsed += new System.Timers.ElapsedEventHandler(Timer_KeepAlive_CallBack);
            }

            /// <summary>
            /// 主动尝试连接到指定的服务端
            /// </summary>
            /// <param name="IP">服务端的IP地址</param>
            /// <param name="Port">服务端监听的端口</param>
            /// <param name="Timeout">指定连接多少毫秒后仍无法成功建立连接则停止连接并引发OnConnectFail事件</param>
            public void Connect(string IP, int Port, int Timeout)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Timer_Connect = new System.Timers.Timer(Timeout);
                Timer_Connect.AutoReset = false;
                Timer_Connect.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Connect_CallBack);
                socket.BeginConnect(new IPEndPoint(IPAddress.Parse(IP), Port), new AsyncCallback(ConnectCallBack), socket);
                Timer_Connect.Start();
            }

            private void ConnectCallBack(IAsyncResult ar)
            {
                try
                {
                    socket.EndConnect(ar);
                    Timer_Connect.Stop();
                    List_SendBuffer = new List<byte[]>();
                    Receive();
                    if (OnConnectSuccess != null)
                    {
                        OnConnectSuccess();
                    }
                }
                catch
                {
                    if (OnConnectFail != null)
                    {
                        OnConnectFail();
                    }
                }
            }

            /// <summary>
            /// 向连接的另一端发送消息（长度太大请自行拆分╮(￣▽￣")╭）
            /// </summary>
            /// <param name="ByteArray">发送的内容</param>
            public void Send(byte[] ByteArray)
            {
                if (ByteArray != null && ByteArray.Length > 0)
                {
                    List_SendBuffer.Add(ByteArray);
                }
                if (IsNotSending)
                {
                    IsNotSending = false;
                    try
                    {
                        socket.BeginSend(new byte[4], 0, 4, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                    }
                    catch
                    {
                        if (OnDropped != null)
                        {
                            OnDropped(this);
                        }
                    }
                }
            }

            private void SendCallback(IAsyncResult ar)
            {
                try
                {
                    if (socket.EndSend(ar) == 4)
                    {
                        while (List_SendBuffer.Count > 0)
                        {
                            socket.Send(BitConverter.GetBytes(List_SendBuffer[0].Length));
                            socket.Send(List_SendBuffer[0]);
                            List_SendBuffer.RemoveAt(0);
                        }
                        IsNotSending = true;
                    }
                    else
                    {
                        Shutdown(false);
                        if (OnDropped != null)
                        {
                            OnDropped(this);
                        }
                    }
                }
                catch
                {
                    Shutdown(false);
                    if (OnDropped != null)
                    {
                        OnDropped(this);
                    }
                }
            }

            /// <summary>
            /// 开始接收连接的另一端发来的消息（仅需调用一次，会自动在接收之后再次执行）
            /// </summary>
            public void Receive()
            {
                byte[] ReceiveBuffer = new byte[4];
                Timer_KeepAlive.Start();
                socket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), ReceiveBuffer);
            }

            private void ReceiveCallback(IAsyncResult ar)
            {
                int HeadLength;
                try
                {
                    HeadLength = socket.EndReceive(ar);
                    Timer_KeepAlive.Stop();
                }
                catch
                {
                    return;
                }
                if (HeadLength != 4)
                {
                    Shutdown(false);
                    if (OnServerDisconnect != null)
                    {
                        OnServerDisconnect(this);
                    }
                    return;
                }
                int Length = BitConverter.ToInt32((byte[])ar.AsyncState, 0);
                if (Length == 0)
                {
                    Receive();
                    return;
                }
                byte[] ReceiveArray = new byte[Length];
                try
                {
                    socket.Receive(ReceiveArray);
                }
                catch
                {
                    Shutdown(false);
                    if (OnDropped != null)
                    {
                        OnDropped(this);
                    }
                }
                Receive();
                if (OnReceive != null)
                {
                    OnReceive(this, ReceiveArray);
                }
            }

            /// <summary>
            /// 主动关闭连接
            /// </summary>
            /// <param name="IsPositive">连接正常时主动释放则该值设为true，连接异常时释放连接则为false（主动关闭连接会遗留一个Time_Wait）</param>
            public void Shutdown(bool IsPositive)
            {
                try
                {
                    Timer_KeepAlive.Stop();
                    if (IsPositive)
                    {
                        socket.Send(new byte[] { });
                    }
                    socket.Shutdown(SocketShutdown.Both);
                }
                catch
                {

                }
                socket.Close();
            }

            /// <summary>
            /// 获取连接另一端的IP地址
            /// </summary>
            /// <returns></returns>
            public string Get_RemoteIP()
            {
                return ((IPEndPoint)(socket.RemoteEndPoint)).Address.ToString();
            }

            /// <summary>
            /// 获取连接另一端的端口
            /// </summary>
            /// <returns></returns>
            public int Get_RemotePort()
            {
                return ((IPEndPoint)(socket.RemoteEndPoint)).Port;
            }

            /// <summary>
            /// 获取连接本地端的IP地址
            /// </summary>
            /// <returns></returns>
            public string Get_LocalIP()
            {
                return ((IPEndPoint)(socket.LocalEndPoint)).Address.ToString();
            }

            /// <summary>
            /// 获取连接本地端的端口
            /// </summary>
            /// <returns></returns>
            public int Get_LocalPort()
            {
                return ((IPEndPoint)(socket.LocalEndPoint)).Port;
            }

            private void Timer_Connect_CallBack(object source, System.Timers.ElapsedEventArgs e)
            {
                Shutdown(false);
            }

            private void Timer_KeepAlive_CallBack(object source, System.Timers.ElapsedEventArgs e)
            {
                Send(null);
            }
        }
        /*
        public class FileTransmission_Server_TCP
        {
            public delegate void Delegate_Do();
            public event Delegate_Do OnCancel;

            Server_TCP server = new Server_TCP();
            FileStream fs;
            public string ip, filepath;
            public int port;
            public long filesize = 0;

            public FileTransmission_Server_TCP(string FilePath)
            {
                if (FilePath == "")
                {
                    if (OnCancel != null)
                    {
                        OnCancel();
                    }
                    return;
                }
                filepath = FilePath;
                fs = new FileStream(FilePath, FileMode.Open);
                filesize = fs.Length;
                fs.Close();
                server.OnClientsReceive += server_OnClientsReceive;
                server.Listen(0, 10);
                ip = ((IPEndPoint)(server.Listener.LocalEndPoint)).Address.ToString();
                port = ((IPEndPoint)(server.Listener.LocalEndPoint)).Port;
            }

            void server_OnClientsReceive(Client_TCP client, byte[] ByteArray)
            {
                long temp_position = BitConverter.ToInt64(ByteArray, 0);
                fs = new FileStream(filepath, FileMode.Open);
                fs.Position = temp_position;
                ByteArray = new byte[fs.Length - temp_position];
                fs.Read(ByteArray, 0, ByteArray.Length);
                client.Send(ByteArray);
                fs.Close();
            }
        }

        public class FileTransmission_Client_TCP
        {
            public delegate void Delegate_Do();
            public event Delegate_Do OnFileReceiveSuccess;

            Client_TCP client = new Client_TCP();
            FileStream fs;
            string filepath;
            long filesize, count = 0;

            public FileTransmission_Client_TCP(string ip, int port, string FilePath, long FileSize, int TimeOut)
            {
                filepath = FilePath;
                filesize = FileSize;
                client.OnReceive += client_OnReceiveByteArray;
                client.Connect(ip, port, TimeOut);
                client.Send(BitConverter.GetBytes(count));
            }

            void client_OnReceiveByteArray(Client_TCP client, byte[] ByteArray)
            {
                fs = new FileStream(filepath, FileMode.Append);
                fs.Write(ByteArray, 0, ByteArray.Length);
                fs.Close();
                client.Shutdown();
                if (OnFileReceiveSuccess != null)
                {
                    OnFileReceiveSuccess();
                }
            }
        }
        */
        public class Connection_UDP
        {
            Socket socket;
            private byte[] ReceiveBuffer = new byte[4096], temp_ReceiveBuffer;
            EndPoint RemoteEP = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));
            string temp_ip;
            int temp_port;

            public delegate void Do();
            public event Do OnBindFail, OnReceiveFail;
            public delegate void Delegate_Receive(string ip, int port, byte[] buffer);
            public event Delegate_Receive OnReceiveByteArray;

            public void Listen(int Port)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                try
                {
                    socket.Bind(new IPEndPoint(IPAddress.Parse("0"), Port));
                    socket.BeginReceiveFrom(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, ref RemoteEP, new AsyncCallback(ReceiveFrom_Callback), socket);
                }
                catch
                {
                    if (OnBindFail != null)
                    {
                        OnBindFail();
                    }
                }
            }

            public void StopListen()
            {
                socket.Close();
            }

            public void Send(string ip, int port, byte[] temp_sendbuffer)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.BeginSendTo(temp_sendbuffer, 0, temp_sendbuffer.Length, SocketFlags.None, (EndPoint)(new IPEndPoint(IPAddress.Parse(ip), port)), new AsyncCallback(SendTo_Callback), socket);
            }

            private void SendTo_Callback(IAsyncResult ar)
            {
                socket.EndSendTo(ar);
            }

            private void ReceiveFrom_Callback(IAsyncResult ar)
            {
                try
                {
                    socket.EndReceiveFrom(ar, ref RemoteEP);
                    temp_ReceiveBuffer = ReceiveBuffer;
                    temp_ip = ((IPEndPoint)(RemoteEP)).Address.ToString();
                    temp_port = ((IPEndPoint)(RemoteEP)).Port;
                    socket.BeginReceiveFrom(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, ref RemoteEP, new AsyncCallback(ReceiveFrom_Callback), socket);
                    if (OnReceiveByteArray != null)
                    {
                        OnReceiveByteArray(temp_ip, temp_port, temp_ReceiveBuffer);
                    }
                }
                catch
                {
                    socket.Close();
                    if (OnReceiveFail != null)
                    {
                        OnReceiveFail();
                    }
                }
            }
        }

        public static byte[] StringToByteArray(string String, Encoding encoding)
        {
            return encoding.GetBytes(String);
        }

        public static string ByteArrayToString(byte[] ByteArray, Encoding encoding)
        {
            return encoding.GetString(ByteArray);
        }
    }
        #endregion   
        
    

}
