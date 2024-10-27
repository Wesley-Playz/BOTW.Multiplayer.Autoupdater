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
            labelStatus.Text = $"Downloading version: {selectedVersion}...";
            DeleteFilesExceptUpdater();

            try
            {
                await DownloadLatestRelease(selectedVersion);
                ExtractRelease();
                labelStatus.Text = $"Updated to version: {selectedVersion}";
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error during download or extraction: " + ex.Message;
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

        private void DeleteFilesExceptUpdater()
        {
            string folderPath = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            foreach (FileInfo file in directory.GetFiles())
            {
                if (file.Name != updaterFileName)
                {
                    file.Delete();
                }
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private async System.Threading.Tasks.Task DownloadLatestRelease(string version)
        {
            string downloadUrl = $"https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Release/releases/download/{version}/{version}.zip";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(downloadUrl);
                response.EnsureSuccessStatusCode();
                byte[] data = await response.Content.ReadAsByteArrayAsync();
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), releaseZipFile);
                await File.WriteAllBytesAsync(filePath, data);
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
    }
}
