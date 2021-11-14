using System;

namespace SocketAsync
{
    public class ClientConnectedEventArgs : EventArgs
    {
        public string NewClient { get; set; }
        public ClientConnectedEventArgs(string _newClient)
        {
            NewClient = _newClient;
        }
    }
    public class StrokesReceivedEventArgs : EventArgs
    {
        public byte[] StrokesReceived { get; set; }
        public StrokesReceivedEventArgs(byte[] _strokesReceived)
        {
            StrokesReceived = _strokesReceived;
        }
    }
}
