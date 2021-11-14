using SocketAsync;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

namespace TcpClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SocketClient mClient;
        public MainWindow()
        {
            InitializeComponent();
            mClient = new SocketClient();
            mClient.RaiseStrokesReceivedEvent += HandleStrokesReceived;
        }

        private void Connect_To_Server_Button_Clicked(object sender, RoutedEventArgs rea)
        {
            mClient.ConnectToServer();
        }
        private void HandleStrokesReceived(object sender, StrokesReceivedEventArgs srea)
        {
            MemoryStream ms = new MemoryStream(srea.StrokesReceived);
            InkCanv.Strokes = new StrokeCollection(ms);
        }

        private void inkCanv_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs icscea)
        {
            MemoryStream _ms = new MemoryStream();
            InkCanv.Strokes.Save(_ms);
            _ms.Flush();

            mClient.SendStrokesToServer(_ms);
        }
    }
}
