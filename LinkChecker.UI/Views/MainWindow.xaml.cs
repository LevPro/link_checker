using LinkChecker.Core.Models;
using LinkChecker.Core.Services;
using LinkChecker.UI.Helpers;
using LinkChecker.UI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace LinkChecker.UI.Views
{
    public partial class MainWindow : Window
    {
        private List<Site> Sites = new();
        private readonly LinkService linkService = new();
        private readonly SitemapService sitemapService = new();
        private readonly CacheService cacheService = new();
        private readonly SoundService soundService = new();

        private int timeoutSeconds = 10;
        private string userAgent = "WpfLinkChecker";
        private string proxy = "";

        private const string CurrentVersion = "1.0.0";
        private const string RepoOwner = "LevPro";
        private const string RepoName = "WpfLinkChecker";

        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.WorkArea.Height;
        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new AboutWindow();
            wnd.Owner = this;
            wnd.ShowDialog();
        }

        private async void UpdateCheckBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string latestVersion = await GetLatestReleaseVersionAsync();
                if (string.IsNullOrEmpty(latestVersion))
                {
                    MessageBox.Show("Не удалось получить информацию об обновлениях.", "Обновления", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (latestVersion != CurrentVersion)
                {
                    if (MessageBox.Show(
                        $"Доступна новая версия: {latestVersion}\nОткрыть страницу релиза?",
                        "Обновление найдено", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(
                            $"https://github.com/{RepoOwner}/{RepoName}/releases/latest") { UseShellExecute = true });
                    }
                }
                else
                {
                    MessageBox.Show("У вас установлена последняя версия.", "Обновления", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            finally { }
        }

        private async Task<string> GetLatestReleaseVersionAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("WpfLinkChecker-Updater");
                var url = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest";
                var json = await client.GetStringAsync(url);
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("tag_name", out var tag))
                    return tag.GetString();
            }
            catch { }
            return null;
        }

        private void OpenSettingsWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SettingsWindow(timeoutSeconds, userAgent, proxy);
            if (dlg.ShowDialog() == true)
            {
                timeoutSeconds = dlg.TimeoutSeconds;
                userAgent = dlg.UserAgent;
                proxy = dlg.Proxy;
            }
        }

        private void OpenLoadLinksWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new LoadLinksWindow();
            if (dlg.ShowDialog() == true)
            {
                var links = linkService.ParseLinks(dlg.LinksText);
                Sites = linkService.GroupLinksBySite(links);
                RefreshLinksList();
            }
        }

        private void OpenLoadSitemapWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new LoadSitemapWindow();
            if (dlg.ShowDialog() == true)
            {
                List<string> links = dlg.IsUrl
                    ? sitemapService.LoadLinksFromRemoteSitemapAsync(dlg.PathOrUrl).Result
                    : sitemapService.LoadLinksFromLocalSitemap(dlg.PathOrUrl);

                Sites = linkService.GroupLinksBySite(links);
                RefreshLinksList();
            }
        }

        private async void CheckAllBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.Value = 0;
            int total = 0, checkedLinks = 0;
            foreach (var site in Sites)
                total += site.Links.Count;

            foreach (var site in Sites)
            {
                foreach (var link in site.Links)
                {
                    var result = await CheckLinkWithSettingsAsync(link.Url);
                    link.ResponseCode = result.ResponseCode;
                    link.ResponseDescription = result.ResponseDescription;
                    cacheService.SaveLink(link);
                    checkedLinks++;
                    ProgressBar.Value = (double)checkedLinks / total * 100;
                }
            }
            ProgressBar.Visibility = Visibility.Collapsed;
            soundService.PlaySuccess();
            RefreshLinksList();
        }

        private async Task<Core.Models.ResponseInfo> CheckLinkWithSettingsAsync(string url)
        {
            using var handler = new HttpClientHandler();
            if (!string.IsNullOrWhiteSpace(proxy))
                handler.Proxy = new WebProxy(proxy);

            using var client = new HttpClient(handler)
            {
                Timeout = System.TimeSpan.FromSeconds(timeoutSeconds)
            };
            if (!string.IsNullOrWhiteSpace(userAgent))
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            try
            {
                var response = await client.GetAsync(url);
                var code = (int)response.StatusCode;
                return new Core.Models.ResponseInfo
                {
                    ResponseCode = code,
                    ResponseDescription = Core.Helpers.ResponseDescriptionHelper.GetDescription(code)
                };
            }
            catch (TaskCanceledException)
            {
                return new Core.Models.ResponseInfo
                {
                    ResponseCode = 0,
                    ResponseDescription = $"Timeout ({timeoutSeconds}s)"
                };
            }
            catch
            {
                return new Core.Models.ResponseInfo
                {
                    ResponseCode = 0,
                    ResponseDescription = "Connection Error"
                };
            }
        }

        private void RefreshLinksList()
        {
            LinksList.Items.Clear();
            foreach (var site in Sites)
            {
                foreach (var link in site.Links)
                {
                    var item = new ListBoxItem
                    {
                        Content = $"{link.Url} [{link.ResponseCode}] {link.ResponseDescription}",
                        Background = ColorHelper.GetBrushByResponseCode(link.ResponseCode)
                    };
                    LinksList.Items.Add(item);
                }
            }
        }

        private void DeleteAllBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var site in Sites)
                cacheService.DeleteAllLinks(site.Host);
            Sites.Clear();
            RefreshLinksList();
        }
    }
}