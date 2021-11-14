using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketAsync
{
    public class SocketServer
    {
        TcpListener mTcpListener;
        public bool KeepRunning { get; set; }
        List<TcpClient> mClients;
        public EventHandler<ClientConnectedEventArgs> RaiseClientConnectedEvent;

        public SocketServer()
        {
            mClients = new List<TcpClient>();
        }

        protected virtual void OnRaiseClientConnectedEvent(ClientConnectedEventArgs ccea)
        {
            EventHandler<ClientConnectedEventArgs> handler = RaiseClientConnectedEvent;
            if(handler != null)
            {
                handler(this, ccea);
            }
        }
        public async Task StartListeningForIncomingConnection()
        {
            mTcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 23000);
            try
            {
                mTcpListener.Start();
                KeepRunning = true;

                while (KeepRunning)
                {
                    var returnedByAccept = await mTcpListener.AcceptTcpClientAsync();
                    mClients.Add(returnedByAccept);
                    Debug.WriteLine("Client connected");
                    HandleTcpClient(returnedByAccept);

                    ClientConnectedEventArgs eaClientConnected;
                    eaClientConnected = new ClientConnectedEventArgs(
                        returnedByAccept.Client.RemoteEndPoint.ToString());
                    OnRaiseClientConnectedEvent(eaClientConnected);
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.ToString());
            }

        }


        private async Task HandleTcpClient(TcpClient mClient)
        {
            NetworkStream stream = null;
            try
            {
                stream = mClient.GetStream();
                byte[] msgBuffer;
                msgBuffer = new byte[1024];
                
                while (KeepRunning)
                {
                    int numOfReturnedCharacters = await stream.ReadAsync(msgBuffer, 0, msgBuffer.Length);

                    if (numOfReturnedCharacters == 0)
                    {
                        RemoveClient(mClient);
                        Debug.WriteLine("Client disconnected");
                        break;
                    }
                    SendToAll(msgBuffer);
                }
            }
            catch (Exception exc)
            {
                RemoveClient(mClient);
                Debug.WriteLine(exc.ToString());
            }
        }
        private void RemoveClient(TcpClient paramClient)
        {
            if(mClients.Contains(paramClient))
            {
                mClients.Remove(paramClient);
            }
        }
        public async Task SendToAll(byte[] strk)
        {
            try
            {
                foreach (var client in mClients)
                {
                    await client.GetStream().WriteAsync(strk, 0, strk.Length);
                }
            }
            catch (Exception exc)
            {

                Debug.WriteLine(exc.ToString());
            }

        }
    }
}
