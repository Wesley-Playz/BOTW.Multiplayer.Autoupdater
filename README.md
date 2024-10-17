# BotW.Multiplayer.Autoupdater

An open source fixed version of the BotW Multiplayer Autoupdater.

Just download one of the releases and run the exe in an empty folder.

If you download the Python version you need to have Python installed on your system to run it.

v1 Downloads the latest release

v2 Allows you to choose between the latest and any version available

It downloads the files from this repository: https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Release

Watch this video to get BotW Multiplayer set up and working: https://www.youtube.com/watch?v=j18yicimeiM

If you want to build the program yourself I would recommend editing the files in Visual Studio 2022 then running cmd in the project folder (the one with the sln file) and running this command: *dotnet publish -r win-x64 --self-contained -p:PublishSingleFile=true -c Release*

Note: You WILL NEED the Newtonsoft.Json, and Costura.Fody NuGet packages in order for the program to work.

If you edit the source code and release is publicly you need to give credit to me (it is already right there in the code).
There is a "forked by *insert name here*" in the code for a reason. Just use it.