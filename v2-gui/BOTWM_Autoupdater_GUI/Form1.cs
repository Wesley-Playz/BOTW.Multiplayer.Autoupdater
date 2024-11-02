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

        // Allowed files and folders
        private readonly List<string> allowedFilesAndFolders = new List<string>
        {
            //Files
            //Current Versions
			"BOTWM_Autoupdater.exe",
            "BOTWM_Autoupdater_GUI.exe",
            "Breath of the Wild Multiplayer.deps.json",
            "Breath of the Wild Multiplayer.dll",
            "Breath of the Wild Multiplayer.exe",
            "Breath of the Wild Multiplayer.runtimeconfig.json",
            "Newtonsoft.Json.dll",
            "Version.txt",
            //Linux
            "BOTWM_Autoupdater",
			//Old Version
			".gitignore",
            "BOTW Multiplayer.exe",
            "BOTW Multiplayer.exe.config",
            "BOTW.DedicatedServer.deps.json",
            "BOTW.DedicatedServer.dll",
            "BOTW.DedicatedServer.exe",
            "BOTW.DedicatedServer.runtimeconfig.json",
            "BOTWM.Server.dll",
            "BOTWMUpdater.py",
            "Gamemodes.json",
            "Microsoft.Bcl.AsyncInterfaces.dll",
            "Microsoft.Bcl.AsyncInterfaces.xml",
            "Newtonsoft.Json.dll",
            "System.Buffers.dll",
            "System.Buffers.xml",
            "System.Memory.dll",
            "System.Memory.xml",
            "System.Numerics.Vectors.dll",
            "System.Numerics.Vectors.xml",
            "System.Runtime.CompilerServices.Unsafe.dll",
            "System.Runtime.CompilerServices.Unsafe.xml",
            "System.Text.Encodings.Web.dll",
            "System.Text.Encodings.Web.xml",
            "System.Text.Json.dll",
            "System.Text.Json.xml",
            "System.Threading.Tasks.Extensions.dll",
            "System.Threading.Tasks.Extensions.xml",
            "System.ValueTuple.dll",
            "System.ValueTuple.xml",
            "TeleportScript.py",
            "version.txt",
            //Folders
            //Current Versions
			"Backgrounds",
            "BNPs",
            "DedicatedServer",
			//Current and Old Versions
			"Resources",
			//Old Version
			"BCML Files",
            "libs"

        };

        public Form1()
        {
            InitializeComponent();
            this.ClientSize = new Size(742, 662); // Adjust the size as needed
            this.Size = new Size(742, 662); // Adjust the size as needed
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Prevent resizing
            this.MaximizeBox = false; // Disable the maximize button
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
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

            // Add button to start multiplayer exe and exit
            Button runExeButton = new Button
            {
                Text = "Start BOTW Multiplayer",
                Location = new Point(240, 460), // Set an appropriate location
                Size = new Size(262, 66) // Set the button size as needed
            };
            runExeButton.Click += runExeButton_Click; // Attach the click event handler
            this.Controls.Add(runExeButton); // Add the button to the form

            // Set the checkbox on top of textBox1
            this.Controls.SetChildIndex(darkModeCheckbox, 0); // Ensure it's on top
        }

        // Event handler for the run EXE button
        private void runExeButton_Click(object sender, EventArgs e)
        {
            string exeFileName = "Breath of the Wild Multiplayer.exe"; // Replace with the actual executable file name
            string exeFilePath = Path.Combine(Directory.GetCurrentDirectory(), exeFileName);

            if (File.Exists(exeFilePath))
            {
                try
                {
                    System.Diagnostics.Process.Start(exeFilePath); // Start the executable
                    this.Close(); // Close the application only if the exe starts successfully
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to start {exeFileName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Executable not found: {exeFileName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                // Dark theme for textBox1 and textBox2
                textBox1.BackColor = Color.FromArgb(30, 30, 30);
                textBox1.ForeColor = Color.White;
                textBox2.BackColor = Color.FromArgb(30, 30, 30);
                textBox2.ForeColor = Color.White;

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

                // Light theme for textBox1 and textBox2
                textBox1.BackColor = Color.White;
                textBox1.ForeColor = Color.Black;
                textBox2.BackColor = Color.White;
                textBox2.ForeColor = Color.Black;

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

            // Check for disallowed files or folders
            if (!CheckAllowedFilesAndFolders())
            {
                labelStatus.Text = "The program will not run.";
                MessageBox.Show("The current directory contains disallowed files or folders.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                return "0.0.0"; // Return a default version
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
                        // Directory entry
                        Directory.CreateDirectory(destinationPath);
                    }
                    else
                    {
                        // File entry
                        entry.ExtractToFile(destinationPath, true);
                    }
                }
            }
            File.Delete(zipPath); // Clean up zip file after extraction
        }

        private bool CheckAllowedFilesAndFolders()
        {
            var existingFilesAndFolders = Directory.GetFiles(Directory.GetCurrentDirectory()).Select(Path.GetFileName).Concat(Directory.GetDirectories(Directory.GetCurrentDirectory()).Select(Path.GetFileName));

            return existingFilesAndFolders.All(item => allowedFilesAndFolders.Contains(item));
        }

        private void ShowCompletionIndicator()
        {
            labelStatus.Text = "Download completed.";
            MessageBox.Show("The update process is complete!", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void AutoCheckForUpdates()
        {
            try
            {
                // Load the latest versions
                var availableVersions = await GetAvailableVersions();
                string currentVersion = GetCurrentVersion();

                if (availableVersions.Count > 0)
                {
                    string latestVersion = availableVersions.First();

                    // Compare versions
                    if (new Version(latestVersion) > new Version(currentVersion))
                    {
                        // Notify the user about the new version
                        var result = MessageBox.Show(
                            $"A new version {latestVersion} is available. Would you like to update?",
                            "New Version Available",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            // You can call the download logic here if you want to initiate the update directly
                            if (!CheckAllowedFilesAndFolders())
                            {
                                labelStatus.Text = "The program will not run.";
                                MessageBox.Show("The current directory contains disallowed files or folders.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            DeleteFilesExceptUpdater();
                            await DownloadLatestRelease(latestVersion);
                            ExtractRelease();
                            UpdateCurrentVersion(latestVersion);
                            ShowCompletionIndicator();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking for updates: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}