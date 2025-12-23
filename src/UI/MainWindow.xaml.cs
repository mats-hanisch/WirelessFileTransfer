using QRCoder;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;

namespace UI
{
    public partial class MainWindow : Window
    {
        public MainWindow(int port)
        {
            InitializeComponent();
            
            // get local ip address
            string ip = GetLocalIPAddress();

            // assemble and set target url of api
            string url = $"http://{ip}:{port}/";
            UrlBlock.Text = url;

            // gen qr code
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q))
            using (QRCode qrCode = new QRCode(qrData))
            using (Bitmap bmp = qrCode.GetGraphic(20))
            {
                using var ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                QrImage.Source = image;
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(UrlBlock.Text);
        }

        private string GetLocalIPAddress()
        {
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "localhost";
        }
    }
}
