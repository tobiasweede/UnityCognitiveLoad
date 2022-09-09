using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HeartRateSocket : MonoBehaviour
{
    // Socket
    static Socket listener;
    private CancellationTokenSource source;
    public ManualResetEvent allDone;

    // Network
    public Boolean externalInterface = false;
    public int Port = 65431;
    public static readonly int WAITTIME = 1;

    // Graph
    private HearthRateGraph hearthRateGraph;

    void Awake()
    {
        source = new CancellationTokenSource();
        allDone = new ManualResetEvent(false);
        hearthRateGraph = GetComponent<HearthRateGraph>();
    }

    async void Start()
    {
        await Task.Run(() => ListenEvents(source.Token));
    }

    private void ListenEvents(CancellationToken token)
    {


        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress;
        if (externalInterface)
            ipAddress = ipHostInfo.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        else
            ipAddress = IPAddress.Loopback;
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Port);

        listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);


            while (!token.IsCancellationRequested)
            {
                allDone.Reset();
                Debug.Log("Waiting for a connection... host :" + ipAddress.MapToIPv4().ToString() + " port : " + Port);
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                while (!token.IsCancellationRequested)
                {
                    if (allDone.WaitOne(WAITTIME))
                    {
                        break;
                    }
                }

            }

        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }

    void AcceptCallback(IAsyncResult ar)
    {
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        allDone.Set();

        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
    }

    void ReadCallback(IAsyncResult ar)
    {
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        int read = handler.EndReceive(ar);

        if (read > 0)
        {
            state.payload.Append(Encoding.UTF8.GetString(state.buffer, 0, read));
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }
        else
        {
            if (state.payload.Length > 1)
            {
                string content = state.payload.ToString();
                Debug.Log($"Read {content.Length} bytes from socket.\n Data : {content}");
                hearthRateGraph.AddObservation(float.Parse(content));
            }
            handler.Close();
        }
    }

    private void OnDestroy()
    {
        source.Cancel();
    }

    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder payload = new StringBuilder();
    }
}