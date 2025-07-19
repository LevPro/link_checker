using System.Windows;
using Microsoft.Win32;

namespace LinkChecker.UI.Views
{
    public partial class LoadSitemapWindow : Window
    {
        public string PathOrUrl { get; private set; }
        public bool IsUrl { get; private set; }

        public LoadSitemapWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.WorkArea.Height;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SourceTab.SelectedIndex == 0)
            {
                PathOrUrl = FilePathBox.Text;
                IsUrl = false;
                if (string.IsNullOrWhiteSpace(PathOrUrl))
                {
                    MessageBox.Show("Выберите файл!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                PathOrUrl = UrlBox.Text;
                IsUrl = true;
                if (string.IsNullOrWhiteSpace(PathOrUrl))
                {
                    MessageBox.Show("Введите URL!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void FilePickerBtn_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog()
            {
                Filter = "XML файлы (*.xml)|*.xml|Все файлы (*.*)|*.*",
                Title = "Выберите sitemap.xml"
            };
            if (openDialog.ShowDialog() == true)
            {
                FilePathBox.Text = openDialog.FileName;
            }
        }
    }
}