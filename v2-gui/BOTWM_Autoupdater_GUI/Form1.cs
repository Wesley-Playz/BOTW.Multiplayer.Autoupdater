using System;
using System.IO;
using System.Net.Http;
using System.IO.Compression;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace BotWMultiplayerUpdaterGUI
{
    public partial class Form1 : Form
    {
        private static readonly string repoApiUrl = "https://gitea.30-seven.cc/api/v1/repos/Wesley/BotW.Multiplayer.Release/releases";
        private static readonly string releaseZipFile = "latest_release.zip";
        private static readonly string versionFileName = "Version.txt";
        private static readonly string updaterFileName = "BOTWM_Autoupdater_GUI.exe";

        public Form1()
        {
            InitializeComponent();
            LoadVersions();
            AutoCheckForUpdates();
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

            // Disable the button while the update is in progress
            buttonDownload.Enabled = false;
            labelStatus.Text = $"Downloading version: {selectedVersion}...";
            DeleteFilesExceptUpdater();

            try
            {
                await DownloadLatestRelease(selectedVersion);
                ExtractRelease();
                labelStatus.Text = $"Updated to version: {selectedVersion}";
                UpdateCurrentVersion(selectedVersion);
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error during download or extraction: " + ex.Message;
            }
            finally
            {
                // Re-enable the button after the update process finishes or errors out
                buttonDownload.Enabled = true;

                // Reset the progress bar once everything is done
                progressBarDownload.Value = 0;
            }
        }

        private async System.Threading.Tasks.Task<List<string>> GetAvailableVersions()
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
                // Force delete all files except the updater file
                if (!file.Name.Equals(updaterFileName, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        file.Attributes = FileAttributes.Normal;  // Ensure file is not read-only
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
                    // Force delete all directories
                    dir.Delete(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting folder: {dir.Name}. {ex.Message}", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async System.Threading.Tasks.Task DownloadLatestRelease(string version)
        {
            string downloadUrl = $"https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Release/releases/download/{version}/{version}.zip";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                long totalBytes = response.Content.Headers.ContentLength ?? -1L;
                byte[] buffer = new byte[8192];
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), releaseZipFile);

                // Download the file with progress tracking
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
                            // Calculate progress and update the progress bar
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

        // Auto-update check feature
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
                        buttonDownload_Click(this, EventArgs.Empty);  // Trigger the download
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
