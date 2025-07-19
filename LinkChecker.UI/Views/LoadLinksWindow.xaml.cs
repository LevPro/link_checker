using System.Windows;

namespace LinkChecker.UI.Views
{
    public partial class LoadLinksWindow : Window
    {
        public string LinksText { get; private set; }

        public LoadLinksWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.WorkArea.Height;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            LinksText = LinksInput.Text;
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