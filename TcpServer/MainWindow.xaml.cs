using SocketAsync;
using System.Collections.ObjectModel;
using System.Windows;

namespace TcpServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SocketServer mServer;
        public ObservableCollection<string> Clients { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            mServer = new SocketServer();
            Clients = new ObservableCollection<string>();
            mServer.RaiseClientConnectedEvent += HandleClientConnected;
        }

        private void Start_Listening_Button_Click(object sender, RoutedEventArgs e)
        {
            mServer.StartListeningForIncomingConnection();
        }

        void HandleClientConnected(object sender, ClientConnectedEventArgs ccea)
        {
            Clients.Add(ccea.NewClient);
            ClientsList.ItemsSource = Clients;
        }
    }
}
