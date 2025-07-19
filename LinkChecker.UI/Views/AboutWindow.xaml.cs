using System.Diagnostics;
using System.Windows;

namespace LinkChecker.UI.Views
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.WorkArea.Height;
        }

        private void AuthorSiteBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://levpro.ru") { UseShellExecute = true });
        }

        private void GithubBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/LevPro/link_checker") { UseShellExecute = true });
        }
    }
}