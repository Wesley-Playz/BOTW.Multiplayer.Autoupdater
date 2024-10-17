using System;
using System.IO;
using System.Net.Http;
using System.IO.Compression;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private static readonly string repoApiUrl = "https://gitea.30-seven.cc/api/v1/repos/Wesley/BotW.Multiplayer.Release/releases"; // Change this URL to your Gitea/GitHub repo
    private static readonly string releaseZipFile = "latest_release.zip";
    private static readonly string versionFileName = "Version.txt"; // Name of the version file in each release
    private static readonly string updaterFileName = "BOTWM_Autoupdater.exe"; // The name of your updater file

    static void Main(string[] args)
    {
        Console.WriteLine("*************************************************************************");
        Console.WriteLine("*        The 'better' Breath of the Wild Multiplayer Autoupdater        *");
        Console.WriteLine("*    Made from scratch by Wesley Hellewell released on Oct 11th 2024    *");
        //Console.WriteLine("*                     Forked by *insert name here                       *");
        Console.WriteLine("*      This auto updater will prepare the mod installation for you      *");
        Console.WriteLine("*          Please do not close this installer until it is done          *");
        Console.WriteLine("*************************************************************************\n");

        if (ContainsOtherFilesOrFolders())
        {
            Console.WriteLine("Other files or folders detected. The updater will not run. Please ensure the folder contains only the updater and BotW Multiplayer files.");
            Console.WriteLine("Press Enter to close the autoupdater.");
            Console.ReadLine(); // Wait for user to press Enter before closing
            return;
        }

        string currentVersion = GetCurrentVersionFromFolder();
        string latestVersion = GetLatestVersionFromGitea().Result;

        Console.WriteLine("Press 1 to update to the latest version or 2 to select a specific version:");
        var key = Console.ReadKey(true).Key;

        if (key == ConsoleKey.D1)
        {
            if (IsVersionNewer(latestVersion, currentVersion))
            {
                Console.WriteLine($"New release found: v{latestVersion}. Updating...");
                DeleteFilesExceptUpdater();
                DownloadLatestRelease(latestVersion).Wait();
                ExtractRelease();
            }
            else
            {
                Console.WriteLine("You already have the latest release. This window will close automatically.");
                System.Threading.Thread.Sleep(3000); // Pause for 3 seconds
            }
        }
        else if (key == ConsoleKey.D2)
        {
            List<string> availableVersions = GetAvailableVersions().Result;
            Console.WriteLine("Available versions:");
            for (int i = 0; i < availableVersions.Count; i++)
            {
                Console.WriteLine($"{i + 1}: v{availableVersions[i]}");
            }

            Console.WriteLine("Select a version number to download:");
            if (int.TryParse(Console.ReadLine(), out int versionChoice) && versionChoice > 0 && versionChoice <= availableVersions.Count)
            {
                string selectedVersion = availableVersions[versionChoice - 1];
                Console.WriteLine($"Downloading version: v{selectedVersion}...");
                DeleteFilesExceptUpdater();
                DownloadLatestRelease(selectedVersion).Wait();
                ExtractRelease();
            }
            else
            {
                Console.WriteLine("Invalid selection. Closing the updater.");
            }
        }
        else
        {
            Console.WriteLine("Invalid key pressed. Closing the updater.");
        }
    }

    static bool ContainsOtherFilesOrFolders()
    {
        string folderPath = Directory.GetCurrentDirectory();
        var allowedFiles = new HashSet<string>
        {
            //Current Versions
			"BOTWM_Autoupdater.exe",
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
            "version.txt"
        };

        var allowedFolders = new HashSet<string>
        {
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

        DirectoryInfo directory = new DirectoryInfo(folderPath);
        var otherFiles = directory.GetFiles().Where(file => !allowedFiles.Contains(file.Name)).ToList();
        var folders = directory.GetDirectories().Where(dir => !allowedFolders.Contains(dir.Name)).ToList();
        return otherFiles.Count > 0 || folders.Count > 0;
    }

    static string GetCurrentVersionFromFolder()
    {
        string versionFilePath = Path.Combine(Directory.GetCurrentDirectory(), versionFileName);
        if (File.Exists(versionFilePath))
        {
            return File.ReadAllText(versionFilePath).Trim();
        }
        return "0.0"; // Assume a baseline version if no version is present
    }

    static async System.Threading.Tasks.Task<string> GetLatestVersionFromGitea()
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(repoApiUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            JArray releases = JArray.Parse(responseBody);
            List<string> versionTags = releases.Select(release => release["tag_name"].ToString()).ToList();
            versionTags.Sort((v1, v2) => new Version(v2).CompareTo(new Version(v1))); // Sort versions descending
            return versionTags.First();
        }
    }

    static async System.Threading.Tasks.Task<List<string>> GetAvailableVersions()
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

    static bool IsVersionNewer(string latestVersion, string currentVersion)
    {
        Version latest = new Version(latestVersion);
        Version current = new Version(currentVersion);
        return latest.CompareTo(current) > 0;
    }

    static void DeleteFilesExceptUpdater()
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

    static async System.Threading.Tasks.Task DownloadLatestRelease(string version)
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

    static void ExtractRelease()
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
        string extractedVersion = GetCurrentVersionFromFolder();
        Console.WriteLine($"Updated to version: {extractedVersion}");
    }
}
