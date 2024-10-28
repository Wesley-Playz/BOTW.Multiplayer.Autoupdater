using System;
using System.IO;
using System.Net.Http;
using System.IO.Compression;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing; // Required for color handling

namespace BotWMultiplayerUpdaterGUI
{
    public partial class Form1 : Form
    {
        private static readonly string repoApiUrl = "https://gitea.30-seven.cc/api/v1/repos/Wesley/BotW.Multiplayer.Release/releases";
        private static readonly string releaseZipFile = "latest_release.zip";
        private static readonly string versionFileName = "Version.txt";
        private static readonly string updaterFileName = "BOTWM_Autoupdater_GUI.exe";

        // Dark mode flag
        private bool isDarkMode = false;

        private CheckBox darkModeCheckbox;

        public Form1()
        {
            InitializeComponent();
            this.ClientSize = new Size(721, 598); // Adjust the size as needed
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Prevent resizing
            this.MaximizeBox = false; // Disable the maximize button
            LoadVersions();
            AutoCheckForUpdates();

            // Create dark mode toggle checkbox
            darkModeCheckbox = new CheckBox
            {
                Text = "Dark Mode",
                Location = new Point(2, -5), // Top left corner
                AutoSize = true
            };
            darkModeCheckbox.CheckedChanged += DarkModeCheckbox_CheckedChanged; // Add event handler
            this.Controls.Add(darkModeCheckbox); // Add checkbox to the form

            // Set the checkbox on top of textBox1
            this.Controls.SetChildIndex(darkModeCheckbox, 0); // Ensure it's on top
        }

        // Event handler for dark mode toggle
        private void DarkModeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            isDarkMode = checkbox.Checked;
            ApplyTheme(); // Apply the selected theme
        }

        // Method to apply dark or light theme
        private void ApplyTheme()
        {
            if (isDarkMode)
            {
                // Set dark theme colors
                this.BackColor = Color.FromArgb(30, 30, 30);
                labelStatus.ForeColor = Color.White;
                listBoxVersions.BackColor = Color.FromArgb(45, 45, 48);
                listBoxVersions.ForeColor = Color.White;
                buttonDownload.BackColor = Color.FromArgb(51, 51, 55);
                buttonDownload.ForeColor = Color.White;
                progressBarDownload.BackColor = Color.FromArgb(51, 51, 55);

                // Dark theme for textBox1
                textBox1.BackColor = Color.FromArgb(30, 30, 30);
                textBox1.ForeColor = Color.White;

                // Dark mode checkbox colors
                darkModeCheckbox.BackColor = Color.FromArgb(30, 30, 30);
                darkModeCheckbox.ForeColor = Color.White;
            }
            else
            {
                // Set light theme colors
                this.BackColor = Color.White;
                labelStatus.ForeColor = Color.Black;
                listBoxVersions.BackColor = Color.White;
                listBoxVersions.ForeColor = Color.Black;
                buttonDownload.BackColor = Color.LightGray;
                buttonDownload.ForeColor = Color.Black;
                progressBarDownload.BackColor = Color.LightGray;

                // Light theme for textBox1
                textBox1.BackColor = Color.White;
                textBox1.ForeColor = Color.Black;

                // Light mode checkbox colors
                darkModeCheckbox.BackColor = Color.White;
                darkModeCheckbox.ForeColor = Color.Black;
            }
        }

        private async void LoadVersions()
        {
            try
            {
                var availableVersions = await GetAvailableVersions();
                listBoxVersions.Items.Clear();
                foreach (var version in availableVersions)
                {
                    listBoxVersions.Items.Add(version);
                }
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error loading versions: " + ex.Message;
            }
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            if (listBoxVersions.SelectedItem == null)
            {
                labelStatus.Text = "Please select a version.";
                return;
            }

            string selectedVersion = listBoxVersions.SelectedItem.ToString();
            string currentVersion = GetCurrentVersion();

            if (selectedVersion == currentVersion)
            {
                labelStatus.Text = "The mod is already up-to-date.";
                return;
            }

            buttonDownload.Enabled = false;
            labelStatus.Text = $"Downloading version: {selectedVersion}...";
            DeleteFilesExceptUpdater();

            try
            {
                await DownloadLatestRelease(selectedVersion);
                ExtractRelease();
                labelStatus.Text = $"Updated to version: {selectedVersion}";
                UpdateCurrentVersion(selectedVersion);
                ShowCompletionIndicator();
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error during download or extraction: " + ex.Message;
            }
            finally
            {
                buttonDownload.Enabled = true;
                progressBarDownload.Value = 0;
            }
        }

        private async Task<List<string>> GetAvailableVersions()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(repoApiUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                JArray releases = JArray.Parse(responseBody);
                return releases.Select(release => release["tag_name"].ToString()).OrderByDescending(v => new Version(v)).ToList();
            }
        }

        private string GetCurrentVersion()
        {
            string versionFilePath = Path.Combine(Directory.GetCurrentDirectory(), versionFileName);

            if (!File.Exists(versionFilePath))
            {
                return string.Empty;
            }

            return File.ReadAllText(versionFilePath).Trim();
        }

        private void UpdateCurrentVersion(string version)
        {
            string versionFilePath = Path.Combine(Directory.GetCurrentDirectory(), versionFileName);
            File.WriteAllText(versionFilePath, version);
        }

        private void DeleteFilesExceptUpdater()
        {
            labelStatus.Text = "Deleting old files...";
            string folderPath = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(folderPath);

            foreach (FileInfo file in directory.GetFiles())
            {
                if (!file.Name.Equals(updaterFileName, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        file.Attributes = FileAttributes.Normal;
                        file.Delete();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting file: {file.Name}. {ex.Message}", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting folder: {dir.Name}. {ex.Message}", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task DownloadLatestRelease(string version)
        {
            string downloadUrl = $"https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Release/releases/download/{version}/{version}.zip";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                long totalBytes = response.Content.Headers.ContentLength ?? -1L;
                byte[] buffer = new byte[8192];
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), releaseZipFile);

                using (var contentStream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    long totalRead = 0L;
                    int bytesRead;
                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        totalRead += bytesRead;

                        if (totalBytes > 0)
                        {
                            int progress = (int)((totalRead * 100L) / totalBytes);
                            progressBarDownload.Value = progress;
                        }
                    }
                }
            }
        }

        private void ExtractRelease()
        {
            string zipPath = Path.Combine(Directory.GetCurrentDirectory(), releaseZipFile);
            string extractPath = Directory.GetCurrentDirectory();
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.Name.Equals(updaterFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    string destinationPath = Path.Combine(extractPath, entry.FullName);
                    if (entry.FullName.EndsWith("/"))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                        entry.ExtractToFile(destinationPath, true);
                    }
                }
            }
            File.Delete(zipPath);
        }

        private async void ShowCompletionIndicator()
        {
            labelStatus.Text = "Download Complete!";
            progressBarDownload.Value = 100;
            await Task.Delay(3000);
            progressBarDownload.Value = 0;
        }

        private async void AutoCheckForUpdates()
        {
            try
            {
                var availableVersions = await GetAvailableVersions();
                string latestVersion = availableVersions.FirstOrDefault();
                string currentVersion = GetCurrentVersion();

                if (string.IsNullOrEmpty(currentVersion))
                {
                    labelStatus.Text = "Current version not found.";
                    return;
                }

                if (new Version(latestVersion).CompareTo(new Version(currentVersion)) > 0)
                {
                    var result = MessageBox.Show(
                        $"A new version ({latestVersion}) is available. Do you want to update now?",
                        "Update Available",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );

                    if (result == DialogResult.Yes)
                    {
                        listBoxVersions.SelectedItem = latestVersion;
                        buttonDownload_Click(this, EventArgs.Empty);
                    }
                }
                else
                {
                    labelStatus.Text = "You are using the latest version.";
                }
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error checking for updates: " + ex.Message;
            }
        }
    }
}
