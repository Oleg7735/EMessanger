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
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.IO;
using System.Diagnostics;
using EncryptMessangerClient.ViewModel;

namespace EncryptMessangerClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            MainViewModel vm = DataContext as MainViewModel;
            vm.ScrollMessages += OnScrollMessagesToEnd;
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
           
           // IPHostEntry ipHost = Dns.GetHostEntry("localhost");
           // IPAddress ipAddr = ipHost.AddressList[0];
           // IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

           // TcpClient client = new TcpClient();
           // client.Connect("127.0.0.1", 11000);
           // // Соединяем сокет с удаленной точкой


           // Debug.WriteLine("connected");
           // //запись
           // XmlWriter xWriter = XmlWriter.Create(new StreamWriter(client.GetStream(), Encoding.UTF8), new XmlWriterSettings()
           // {
           //     ConformanceLevel = ConformanceLevel.Auto,
           //     OmitXmlDeclaration = false,
           //     Indent = true,
           //     NamespaceHandling = NamespaceHandling.OmitDuplicates
           // });

           // //xWriter.Settings.ConformanceLevel = System.Xml.ConformanceLevel.Auto;

           // //xWriter.WriteStartElement("stream");
           // //xWriter.WriteString("stream");
           // //xWriter.WriteEndElement();
           // //xWriter.Flush();
           // byte[] msg = Encoding.UTF8.GetBytes("<stream>");
           // client.GetStream().Write(msg, 0, msg.Length);
           // Debug.WriteLine("Sended");
           // XmlReader xReader = XmlReader.Create(new StreamReader(client.GetStream(), Encoding.UTF8), new XmlReaderSettings()
           // {
           //     ConformanceLevel = ConformanceLevel.Auto
           // });

           // do
           // {
           //     xReader.MoveToContent();
           //     textBox.Text = xReader.GetAttribute("text");
           //     Debug.WriteLine("readed");
           //     xWriter.WriteStartElement("message");
           //     xWriter.WriteAttributeString("text", textBox.Text);
           //     xWriter.WriteEndElement();
           //     xWriter.Flush();
           // } while (xReader.Read());


           // //xWriter.WriteEndElement();
           // //xWriter.Flush();


           // /*byte[] msg = Encoding.UTF8.GetBytes("<stream>");
           // xsender.Send(msg);
           //int n= xsender.Receive(msg);
           // textBox.Text = Encoding.UTF8.GetString(msg, 0, n);
           // xsender.Shutdown(SocketShutdown.Both);
           // xsender.Close();*/
        }
        private void OnScrollMessagesToEnd(object sender, EventArgs args)
        {
            Decorator border = VisualTreeHelper.GetChild(messagesListBox, 0) as Decorator;
            if (border != null)
            {
                // Get scrollviewer
                ScrollViewer scrollViewer = border.Child as ScrollViewer;
                if (scrollViewer != null)
                {
                    //// center the Scroll Viewer...
                    //double center = scrollViewer.ScrollableHeight / 2.0;
                    //scrollViewer.ScrollToVerticalOffset(center);
                    scrollViewer.ScrollToEnd();
                    
                }
            }
        }
        private void dialogListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = DataContext as MainViewModel;            
            Decorator border = VisualTreeHelper.GetChild(messagesListBox, 0) as Decorator;
            if (border != null)
            {
                // Get scrollviewer
                ScrollViewer scrollViewer = border.Child as ScrollViewer;
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollChanged += vm.OnMessagesScroll;
                }
            }
        }
    }
}
