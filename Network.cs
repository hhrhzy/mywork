using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// 网络类 Version 1.9999
/// </summary>
public class Network
{
    public class Server_TCP
    {
        public delegate void Delegate_OnClientEvent(Client_TCP client);
        public event Delegate_OnClientEvent OnAcceptNewClient, OnRemoteDisconnect, OnDropped;
        public event Client_TCP.Delegate_Do OnBindFail, OnAcceptClientFail;
        public event Client_TCP.Delegate_Receive OnClientsReceive;

        Socket Listener;
        public List<Client_TCP> Clients = new List<Client_TCP>();

        public bool Listen(int Port, int ListeningQueueLength)
        {
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Listener.Bind(new IPEndPoint(IPAddress.Parse("0"), Port));
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
                if (OnRemoteDisconnect != null)
                {
                    temp_client.OnRemoteDisconnect += OnRemoteDisconnect;
                }
                if (OnClientsReceive != null)
                {
                    temp_client.OnReceive += OnClientsReceive;
                }
                if (OnDropped != null)
                {
                    temp_client.OnDropped += OnDropped;
                }
                Clients.Add(temp_client);
                if (OnAcceptNewClient != null)
                {
                    OnAcceptNewClient(temp_client);
                }
                temp_client.Receive();
            }
            catch
            {
                Listener.BeginAccept(new AsyncCallback(AcceptCallBack), Listener);
                if (OnAcceptClientFail != null)
                {
                    OnAcceptClientFail();
                }
            }
        }

        public void StopListen()
        {
            Listener.Close();
            Listener.Dispose();
            for (int i = Clients.Count - 1; i > -1; i--)
            {
                RemoveClient(Clients[i]);
            }
        }

        public void Multicast(List<Client_TCP> clients, byte[] ByteArray)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Send(ByteArray);
            }
        }

        public void RemoveClient(Client_TCP client)
        {
            Clients.Remove(client);
            client.Shutdown();
        }
    }

    public class Client_TCP
    {
        public delegate void Delegate_Do();
        public event Delegate_Do OnConnectSuccess, OnConnectFail, OnConnectTimeout;
        public delegate void Delegate_Receive(Client_TCP client, byte[] ByteArray);
        public event Delegate_Receive OnReceive;
        public event Server_TCP.Delegate_OnClientEvent OnRemoteDisconnect, OnDropped;

        Socket socket;
        System.Timers.Timer Timer_Connect, Timer_KeepAlive;
        byte[] ReceiveBuffer;
        List<byte[]> List_SendBuffer = new List<byte[]>();
        int ReceiveLength, Index;
        ushort PacketLength;

        public Client_TCP()
        {
            Timer_KeepAlive = new System.Timers.Timer(3000);
            Timer_KeepAlive.AutoReset = false;
            Timer_KeepAlive.Elapsed += new System.Timers.ElapsedEventHandler(Timer_KeepAlive_CallBack);

            Timer_Connect = new System.Timers.Timer();
            Timer_Connect.AutoReset = false;
            Timer_Connect.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Connect_CallBack);
        }

        public Client_TCP(Socket s)
        {
            socket = s;
            Timer_KeepAlive = new System.Timers.Timer(5000);
            Timer_KeepAlive.AutoReset = false;
            Timer_KeepAlive.Elapsed += new System.Timers.ElapsedEventHandler(Timer_KeepAlive_CallBack);

            Timer_Connect = new System.Timers.Timer();
            Timer_Connect.AutoReset = false;
            Timer_Connect.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Connect_CallBack);
        }

        public void Connect(string IP, int Port, int Timeout)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.BeginConnect(new IPEndPoint(IPAddress.Parse(IP), Port), new AsyncCallback(ConnectCallBack), socket);
                Timer_Connect.Interval = Timeout;
                Timer_Connect.Start();
            }
            catch
            {
                if (OnConnectFail != null)
                {
                    OnConnectFail();
                }
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            Timer_Connect.Stop();
            if (socket.Connected)
            {
                List_SendBuffer = new List<byte[]>();
                ReceiveBuffer = new byte[socket.ReceiveBufferSize];
                if (OnConnectSuccess != null)
                {
                    OnConnectSuccess();
                }
                Receive();
            }
            else
            {
                if (OnConnectFail != null)
                {
                    OnConnectFail();
                }
            }
        }

        public void Send(byte[] ByteArray)
        {
            if (ByteArray.Length > 65495)
            {
                for (int index = 0; index < ByteArray.Length; )
                {
                    int Size = ByteArray.Length - index;
                    if (Size > 65495)
                    {
                        Size = 65495;
                    }
                    byte[] temp = new byte[Size];
                    for (int j = 0; j < Size; j++)
                    {
                        temp[j] = ByteArray[index++];
                    }
                    Send(ByteArray);
                }
            }
            else
            {
                byte[] temp = new byte[ByteArray.Length + 2];
                byte[] Length = BitConverter.GetBytes(ByteArray.Length);
                for (int i = 0; i < 2; i++)
                {
                    temp[i] = Length[i];
                }
                for (int i = 2; i < temp.Length; i++)
                {
                    temp[i] = ByteArray[i - 2];
                }
                List_SendBuffer.Add(temp);
                if (List_SendBuffer.Count == 1)
                {
                    try
                    {
                        socket.BeginSend(List_SendBuffer[0], 0, List_SendBuffer[0].Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
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
        }

        private void SendCallback(IAsyncResult ar)
        {
            socket.EndSend(ar);
            List_SendBuffer.RemoveAt(0);
            if (List_SendBuffer.Count > 0)
            {
                socket.BeginSend(List_SendBuffer[0], 0, List_SendBuffer[0].Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
            }
        }

        public void Receive()
        {
            ReceiveBuffer = new byte[socket.ReceiveBufferSize];
            Timer_KeepAlive.Start();
            socket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            ReceiveLength = socket.EndReceive(ar);
            Timer_KeepAlive.Stop();
            if (ReceiveLength == 0)
            {
                Shutdown();
                if (OnRemoteDisconnect != null)
                {
                    OnRemoteDisconnect(this);
                }
                return;
            }
            PacketLength = BitConverter.ToUInt16(ReceiveBuffer, 0);
            byte[] ReceiveBytes = new byte[PacketLength];
            for (Index = 0; Index < ReceiveLength - 2; Index++)
            {
                ReceiveBytes[Index] = ReceiveBuffer[Index + 2];
            }
            while (Index < PacketLength)
            {
                try
                {
                    ReceiveLength = socket.Receive(ReceiveBuffer);
                }
                catch
                {
                    Shutdown();
                    if (OnDropped != null)
                    {
                        OnDropped(this);
                    }
                }
                for (int i = 0; i < ReceiveLength; i++)
                {
                    ReceiveBytes[++Index] = ReceiveBuffer[i];
                }
            }
            Receive();
            if (OnReceive != null)
            {
                OnReceive(this, ReceiveBytes);
            }
        }

        public void Shutdown()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {

            }
            socket.Close();
        }

        public string Get_RemoteIP()
        {
            return ((IPEndPoint)(socket.RemoteEndPoint)).Address.ToString();
        }

        public int Get_RemotePort()
        {
            return ((IPEndPoint)(socket.RemoteEndPoint)).Port;
        }

        public string Get_LocalIP()
        {
            return ((IPEndPoint)(socket.LocalEndPoint)).Address.ToString();
        }

        public int Get_LocalPort()
        {
            return ((IPEndPoint)(socket.LocalEndPoint)).Port;
        }

        private void Timer_Connect_CallBack(object source, System.Timers.ElapsedEventArgs e)
        {
            socket.Close();
            socket.Dispose();
            if (OnConnectTimeout != null)
            {
                OnConnectTimeout();
            }
        }

        private void Timer_KeepAlive_CallBack(object source, System.Timers.ElapsedEventArgs e)
        {
            Send(new byte[] { });
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