using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Installer.Forge;
using CmlLib.Core.ProcessBuilder;

namespace XasLauncher
{
    public partial class MainWindow : Window
    {
        private readonly MinecraftPath _path;
        private readonly MinecraftLauncher _launcher;

        public MainWindow()
        {
            InitializeComponent();
            _path = new MinecraftPath();
            _launcher = new MinecraftLauncher(_path);
            Loaded += async (s, e) => await LoadVersions();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); }
        private void BtnClose_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void BtnMinimize_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton btn && btn.Tag != null)
            {
                string tag = btn.Tag.ToString() ?? "";
                ViewHome.Visibility = tag == "Home" ? Visibility.Visible : Visibility.Collapsed;
                ViewSettings.Visibility = tag == "Settings" ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void BtnOpenMods_Click(object sender, RoutedEventArgs e)
        {
            string modsPath = Path.Combine(_path.BasePath, "mods");
            if (!Directory.Exists(modsPath)) Directory.CreateDirectory(modsPath);
            Process.Start("explorer.exe", modsPath);
        }

        private void sliderRam_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblRam != null)
                lblRam.Text = $"{(int)sliderRam.Value} MB";
        }

        private async Task LoadVersions()
        {
            lblStatus.Text = "Sürümler taranıyor...";
            try
            {
                var versions = await _launcher.GetAllVersionsAsync();
                foreach (var v in versions)
                {
                    comboVersions.Items.Add(v.Name);
                }
                if (comboVersions.Items.Count > 0)
                    comboVersions.SelectedIndex = 0;

                lblStatus.Text = "Hazır.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Hata!";
                MessageBox.Show("Sürüm listesi alınamadı: " + ex.Message);
            }
        }

        private async void BtnLaunch_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string? selectedVersion = comboVersions.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(selectedVersion))
            {
                MessageBox.Show("Lütfen kullanıcı adı girin ve bir sürüm seçin!");
                return;
            }

            lblStatus.Text = "İşleniyor...";
            btnLaunch.IsEnabled = false; // Çift tıklamayı engelle
            string finalVersionId = selectedVersion;
            int loaderType = comboLoader.SelectedIndex;

            try
            {
                if (loaderType == 1 || loaderType == 2)
                {
                    lblStatus.Text = "Mod yükleyici hazırlanıyor...";
                    var forgeInstaller = new ForgeInstaller(_launcher);

                    // Tarayıcı açılmasını engellemek için sessiz kurulumu zorla
                    // Not: Bazı v4 sürümlerinde bu otomatik gelir, ancak finalVersionId 
                    // üzerinden kontrol etmek en garantisidir.
                    finalVersionId = await forgeInstaller.Install(selectedVersion);
                }

                var session = MSession.CreateOfflineSession(username);
                var launchOption = new MLaunchOption
                {
                    MaximumRamMb = (int)sliderRam.Value,
                    Session = session,
                    JavaPath = "java" // Sistemdeki varsayılan Java'yı kullan
                };

                // Dosya kontrolü ve Başlatma
                lblStatus.Text = "Dosyalar doğrulanıyor...";
                var process = await _launcher.CreateProcessAsync(finalVersionId, launchOption);

                lblStatus.Text = "Oyun Açılıyor!";
                process.Start();

                this.Hide();
                await process.WaitForExitAsync();
                this.Show();
                lblStatus.Text = "Hazır.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Başlatma Hatası: {ex.Message}");
                lblStatus.Text = "Hata oluştu!";
                this.Show();
            }
            finally
            {
                btnLaunch.IsEnabled = true;
            }
        }
    }
}