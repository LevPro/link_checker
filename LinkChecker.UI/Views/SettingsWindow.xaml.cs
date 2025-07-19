using System.Windows;

namespace LinkChecker.UI.Views
{
    public partial class SettingsWindow : Window
    {
        public int TimeoutSeconds { get; private set; }
        public string UserAgent { get; private set; }
        public string Proxy { get; private set; }

        public SettingsWindow(int currentTimeout, string currentUserAgent, string currentProxy)
        {
            InitializeComponent();
            TimeoutBox.Text = currentTimeout.ToString();
            UserAgentBox.Text = currentUserAgent;
            ProxyBox.Text = currentProxy;
            this.MaxHeight = SystemParameters.WorkArea.Height;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            int timeout;
            if (!int.TryParse(TimeoutBox.Text, out timeout) || timeout < 1)
            {
                MessageBox.Show("Введите корректное значение таймаута (целое число > 0)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TimeoutSeconds = timeout;
            UserAgent = UserAgentBox.Text;
            Proxy = ProxyBox.Text;
            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}