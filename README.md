# BotW.Multiplayer.Autoupdater

An open source and improved version of the classic BotW Multiplayer Autoupdater.

Just download one of the releases and run the exe in an empty folder.

If you download the Python version you need to have Python installed on your system to run it.

v1: Downloads the latest release.

![picture](https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Autoupdater/raw/branch/main/images/v1.png)

v2: Allows you to choose between the latest and any version available.

![picture](https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Autoupdater/raw/branch/main/images/v2.png)

v2-python: v2 remade in Python.

![picture](https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Autoupdater/raw/branch/main/images/Python.png)

v2-gui: v2 remade with a GUI (only has all versions/can't just choose latest version to download). Also alerts you (when you open the program) if there is a new version available and allows you to download it.

![picture](https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Autoupdater/raw/branch/main/images/GUI.png)

The autoupdater downloads the files from this repository: https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Release

Watch this video to get BotW Multiplayer set up and working: https://www.youtube.com/watch?v=j18yicimeiM

If you want to build v1, v2, or v2-python yourself I would recommend editing the files in Visual Studio 2022 then running CMD in the project folder (the one with the sln file) and running this command: *dotnet publish -r win-x64 --self-contained -p:PublishSingleFile=true -c Release*

If you want to build v2-gui yourself I would recommend editing the program in Visual Studio 2022 then running CMD in the project folder (the one with the sln file) and running this command: *dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeAllContentForSelfExtract=true*

Note: You WILL NEED the Newtonsoft.Json (all versions), and Costura.Fody (all versions except for v2-gui) NuGet packages in order for the program to work.

If you edit the source code and release is publicly you need to give credit to me (it is already right there in the code).
There is a "forked by *insert name here*" in the code for a reason. Just use it.

If you edit the v2-gui source code and release it publicly you need to give credit to me (just add *forked by ____* in the textbox in the designer).