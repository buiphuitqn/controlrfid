using Quobject.SocketIoClientDotNet.Client;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControllRFID
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void Update_text(string text);
        public MainWindow()
        {
            InitializeComponent();
            ConnectSocket();
        }

        private void ConnectSocket()
        {
            var option = new IO.Options()
            {
                QueryString = "Type=Desktop",
                Timeout = 5000,
                ReconnectionDelay = 5000,
                Reconnection = true,
                Transports = Quobject.Collections.Immutable.ImmutableList.Create<string>("websocket")
            };
            Socket socket = IO.Socket("http://10.40.12.4:7798", option);
            //socket.Connect();
            socket.On(Socket.EVENT_CONNECT, () => {
                //socket.Emit("Hi");
                Dispatcher.BeginInvoke(new Update_text(AddText), new Object[] { "Kết nối"});
            });
            socket.On(Socket.EVENT_CONNECT_ERROR, (data) => {
                Dispatcher.BeginInvoke(new Update_text(AddText), new Object[] { "Mất kết nối" });
            });
            socket.On("Callmenow", (data) =>
            {
                //MessageBox.Show("Cho phép băng tải tiếp tục");
                Dispatcher.BeginInvoke(new Update_text(AddText), new Object[] { "Cho phép băng tải tiếp tục" });
            });
            socket.On(Socket.EVENT_DISCONNECT, () => {
                //MessageBox.Show("Mất kết nối");
                Dispatcher.BeginInvoke(new Update_text(AddText), new Object[] { "Mất kết nối" });
                //socket.Connect();
            });
        }

        private void AddText(string text)
        {
            TextRange oldtext = new TextRange(txtStatus.Document.ContentStart, txtStatus.Document.ContentEnd);
            FlowDocument mcdoc = new FlowDocument();
            Paragraph para = new Paragraph();
            para.Inlines.Add(new Run(oldtext.Text));
            para.Inlines.Add(new Run(text));
            mcdoc.Blocks.Add(para);
            txtStatus.Document = mcdoc;
        }
    }
}
