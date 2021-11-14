using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketAsync
{
    public class SocketClient
    {
        TcpClient mClient;
        public EventHandler<StrokesReceivedEventArgs> RaiseStrokesReceivedEvent;
        public async Task ConnectToServer()
        {
            if(mClient == null)
            {
                mClient = new TcpClient();
            }
            try
            {
                await mClient.ConnectAsync("127.0.0.1", 23000);
                ReadDataAsync(mClient);
            }
            catch (Exception exc)
            {

                Debug.WriteLine(exc.ToString());
            }
        }
        protected virtual void OnRaiseStrokesReceivedEvent(StrokesReceivedEventArgs mrea)
        {
            EventHandler<StrokesReceivedEventArgs> handler = RaiseStrokesReceivedEvent;
            if (handler != null)
            {
                handler(this, mrea);
            }
        }
        private async Task ReadDataAsync(TcpClient mClient)
        {
            NetworkStream stream = null;
            try
            {
                stream = mClient.GetStream();
                byte[] msgBuffer;
                msgBuffer = new byte[1024];

                while (true)
                {
                    int numOfReturnedCharacters = await stream.ReadAsync(msgBuffer, 0, msgBuffer.Length);
                    OnRaiseStrokesReceivedEvent(new StrokesReceivedEventArgs(
                        msgBuffer));
                    Array.Clear(msgBuffer, 0, msgBuffer.Length);
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.ToString());
            }
        
    }
        public async Task SendStrokesToServer(MemoryStream ms)
        {
            if (mClient != null)
            {
                if (mClient.Connected)
                {
                    mClient.Client.Send(ms.ToArray());
                }
            }
        }

        public async Task SendMessageToServer(string strInputUser)
        {
            if(mClient != null)
            {
                if(mClient.Connected)
                {
                    StreamWriter clientStreamWriter = new StreamWriter(mClient.GetStream());
                    clientStreamWriter.AutoFlush = true;

                    await clientStreamWriter.WriteAsync(strInputUser);
                }
            }
        }
    }
}
