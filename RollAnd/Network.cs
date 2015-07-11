using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server_TCP
{
    public event Client_TCP.Delegate_ReceiveMsg OnClientsReceiveMsg;
    public delegate void Delegate_OnClientEvent(Client_TCP client);
    public event Delegate_OnClientEvent OnAcceptNewClient, OnClientDisconnect;
    public delegate void Delegate_Do();
    public event Delegate_Do OnBindFail, OnListenSuccess, OnListenFail;

    public Socket Listener;
    public List<Client_TCP> Clients = new List<Client_TCP>();

    public void Listen(int Port, int ListeningQueueLength)
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
        }
        try
        {
            Listener.Listen(ListeningQueueLength);
            Listener.BeginAccept(new AsyncCallback(AcceptCallBack), Listener);
        }
        catch
        {
            if (OnListenFail != null)
            {
                OnListenFail();
            }
        }
    }

    private void AcceptCallBack(IAsyncResult ar)
    {
        Client_TCP temp_client = new Client_TCP();
        try
        {
            temp_client.socket = Listener.EndAccept(ar);
        }
        catch
        {
            Listener.Dispose();
            return;
        }
        Listener.BeginAccept(new AsyncCallback(AcceptCallBack), Listener);
        if (OnClientsReceiveMsg != null)
        {
            temp_client.OnReceiveMsg += OnClientsReceiveMsg;
        }
        Clients.Add(temp_client);
        temp_client.Receive();
        if (OnClientDisconnect != null)
        {
            temp_client.OnDisconnect += OnClientDisconnect;
        }
        if (OnListenSuccess != null)
        {
            OnListenSuccess();
        }
        if(OnAcceptNewClient != null)
        {
            OnAcceptNewClient(temp_client);
        }
    }

    public void StopListen()
    {
        Listener.Close();
        Listener.Dispose();
        for (int i = Clients.Count - 1; i >= 0; i--)
        {
            Client_Remote(Clients[i]);
        }
    }

    public void Send(Client_TCP client, string msg)
    {
        if(client.socket.Connected)
        {
            client.Send(msg);
        }
        else
        {
            client.Shutdown();
        }
    }

    public void Client_Remote(Client_TCP client)
    {
        foreach(Client_TCP temp in Clients)
        {
            if(temp == client)
            {
                if (client.socket.Connected)
                {
                    client.Shutdown();
                }
                Clients.Remove(temp);
                return;
            }
        }    
    }
}

public class Client_TCP
{
    public delegate void Delegate_Do();
    public event Delegate_Do OnConnectSuccess, OnConnectFail, OnInvalidIP;
    public delegate void Delegate_Exception(Exception exception);
    public event Delegate_Exception OnConnectException;
    public delegate void Delegate_ReceiveMsg(Client_TCP client, string msg);
    public event Delegate_ReceiveMsg OnReceiveMsg;
    public event Server_TCP.Delegate_OnClientEvent OnDisconnect;

    public Socket socket;
    System.Timers.Timer Timer_Do;
    public byte[] byteArray;
     public string name;
     public int code;
    public int ReceiveBufferSize = 4096;

    public void Send(string msg)
    {
        try
        {
            byte[] temp_byteArray = Encoding.Unicode.GetBytes(msg);
            socket.BeginSend(temp_byteArray, 0, temp_byteArray.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
        }
        catch
        {
            Shutdown();
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        socket.EndSend(ar);
    }

    public void Receive()
    {
        try
        {
            byteArray = new byte[ReceiveBufferSize];
            socket.BeginReceive(byteArray, 0, byteArray.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }
        catch
        {
            Shutdown();
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if (socket.EndReceive(ar) > 0)
            {
                if (OnReceiveMsg != null)
                {
                    OnReceiveMsg(this, Encoding.Unicode.GetString(byteArray).Trim('\0'));
                }
                Receive();
            }
            else
            {
                Shutdown();
            }
        }
        catch
        {
            Shutdown();
        }
    }

    public void Shutdown()
    {
        try
        {
            socket.Shutdown(SocketShutdown.Both);
            if (OnDisconnect != null)
            {
                OnDisconnect(this);
            }
        }
        catch
        {

        }
        finally
        {
            socket.Dispose();
        }
    }

    public void Connect(string ip, int port ,int timeout)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), new AsyncCallback(ConnectCallBack), socket);
            Timer_Do = new System.Timers.Timer(timeout);
            Timer_Do.AutoReset = true;
            Timer_Do.Elapsed += new System.Timers.ElapsedEventHandler(Timer_ToDo);
            Timer_Do.Enabled = true;
        }
        catch
        {
            if (OnInvalidIP != null)
            {
                OnInvalidIP();
            }
        }
    }

    private void ConnectCallBack(IAsyncResult ar)
    {
        Timer_Do.Enabled = false;
        Timer_Do = null;
        if (socket.Connected)
        {
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

   public void SendAll(string msg)
    {
        for (int i = 0; i < Clients.Length; i++)
        {
            Send(Clients[i], msg);
        }
    }


    private void Timer_ToDo(object source, System.Timers.ElapsedEventArgs e)
    {
        socket.Close();
    }
}