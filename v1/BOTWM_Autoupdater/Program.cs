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
        Console.WriteLine("*  Built from the ground up Wesley Hellewell released on Oct 11th 2024  *");
        //Console.WriteLine("*                     Forked by *insert name here                       *");
        Console.WriteLine("*      This auto updater will prepare the mod installation for you      *");
        Console.WriteLine("*          Please do not close this installer until it is done          *");
        Console.WriteLine("*************************************************************************\n");
        // Change text color to red
        //Console.ForegroundColor = ConsoleColor.Red;

        // Print red text
        //Console.WriteLine("WARNING: THIS FILE WILL DELETE EVERYING IN THE CURRENT FOLDER; RUN IT IN AN EMPTY FOLDER OR YOU WILL LOSE DATA!");

        // Reset to the default color
        //Console.ResetColor();
        //Console.WriteLine("By pressing any key to continue, you acknowledge that you have read the warning and placed the file in an empty folder. I am NOT responsible for any data loss caused by failure to follow instructions, or failure to heed the warning.\n");
        //Console.WriteLine("Press any key to continue...");
        //Console.ReadKey();  // Waits for the user to press any key

        static bool ContainsOtherFilesOrFolders()
        {
            string folderPath = Directory.GetCurrentDirectory();

            // Allowed folders and files in the BotW Multiplayer structure
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

            // Get all files and directories in the folder
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            var otherFiles = directory.GetFiles().Where(file => !allowedFiles.Contains(file.Name)).ToList();
            var folders = directory.GetDirectories().Where(dir => !allowedFolders.Contains(dir.Name)).ToList();

            // If there's any unexpected files or folders, return true
            return otherFiles.Count > 0 || folders.Count > 0;
        }

        if (ContainsOtherFilesOrFolders())
        {
            Console.WriteLine("Other files or folders detected. The updater will not run. Please ensure the folder contains only the updater and BotW Multiplayer files.");
            Console.WriteLine("Press Enter to close the autoupdater.");
            Console.ReadLine(); // Wait for user to press Enter before closing
            return;
        }

        string currentVersion = GetCurrentVersionFromFolder();
        string latestVersion = GetLatestVersionFromGitea().Result;

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
            System.Threading.Thread.Sleep(3000); // Pause for 3000 milliseconds (3 seconds)
        }
    }

    static bool ContainsOtherFilesOrFolders()
    {
        string folderPath = Directory.GetCurrentDirectory();

        // Get all files and directories in the folder
        DirectoryInfo directory = new DirectoryInfo(folderPath);

        // Count all files except the updater
        var otherFiles = directory.GetFiles().Where(file => file.Name != updaterFileName).ToList();
        var folders = directory.GetDirectories();

        // If there's more than one file or any folders, return true
        return otherFiles.Count > 0 || folders.Length > 0;
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

            // Parse all releases and their tags
            JArray releases = JArray.Parse(responseBody);
            List<string> versionTags = new List<string>();

            foreach (var release in releases)
            {
                string tagName = release["tag_name"].ToString();
                versionTags.Add(tagName);
            }

            // Sort and find the latest version
            versionTags = versionTags.OrderByDescending(v => new Version(v)).ToList();
            return versionTags.First();
        }
    }

    static bool IsVersionNewer(string latestVersion, string currentVersion)
    {
        Version latest = new Version(latestVersion);
        Version current = new Version(currentVersion);
        return latest.CompareTo(current) > 0;
    }

    // This function deletes everything in the folder except for BOTWM_Autoupdater.exe
    static void DeleteFilesExceptUpdater()
    {
        string folderPath = Directory.GetCurrentDirectory();

        DirectoryInfo directory = new DirectoryInfo(folderPath);

        foreach (FileInfo file in directory.GetFiles())
        {
            // Skip the updater itself
            if (file.Name != updaterFileName)
            {
                file.Delete();
            }
        }

        foreach (DirectoryInfo dir in directory.GetDirectories())
        {
            dir.Delete(true); // true to delete subdirectories and files
        }
    }

    static async System.Threading.Tasks.Task DownloadLatestRelease(string version)
    {
        string downloadUrl = $"https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Release/releases/download/{version}/{version}.zip"; // Change this URL to your Gitea download link
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(downloadUrl);
            response.EnsureSuccessStatusCode();
            byte[] data = await response.Content.ReadAsByteArrayAsync();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), releaseZipFile);
            await File.WriteAllBytesAsync(filePath, data);
        }
    }

    // This method extracts the release but skips overwriting the updater file
    static void ExtractRelease()
    {
        string zipPath = Path.Combine(Directory.GetCurrentDirectory(), releaseZipFile);
        string extractPath = Directory.GetCurrentDirectory(); // Extract to the same folder as the updater

        using (ZipArchive archive = ZipFile.OpenRead(zipPath))
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                // Skip the updater file itself
                if (entry.Name.Equals(updaterFileName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string destinationPath = Path.Combine(extractPath, entry.FullName);

                // Create the directory for the file, if necessary
                if (entry.FullName.EndsWith("/")) // If it's a directory, ensure it's created
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                }
                else
                {
                    // Extract the file
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)); // Ensure directory exists
                    entry.ExtractToFile(destinationPath, true); // Overwrite existing files
                }
            }
        }

        File.Delete(zipPath); // Optionally delete the zip after extraction

        string extractedVersion = GetCurrentVersionFromFolder();
        Console.WriteLine($"Updated to version: {extractedVersion}");
    }
}
