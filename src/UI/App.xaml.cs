using System.Configuration;
using System.Data;
using System.Windows;


namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length != 1)
            {
                MessageBox.Show(
                    "Der erforderliche Befehlszeilenparameter <port> fehlt. Das Programm wurde abgebrochen. Bitte starten Sie das Programm erneut mit dem Port als Argument.",
                    "Kritischer Fehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                Shutdown(1);
                return;
            }

            int port = int.Parse(e.Args[0]);
            
            MainWindow = new MainWindow(port);
            MainWindow.Show();
        }
    }

}
