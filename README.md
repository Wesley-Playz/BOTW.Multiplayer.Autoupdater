# BOTW Multiplayer Autoupdater

An open source and improved version of the original BotW Multiplayer Autoupdater.

Just download one of the releases and run the program in an empty folder.

&nbsp;

If you download the Python version you need to have Python installed on your system to run it. On Linux just open a terminal and run: `sudo apt install python3` then run: `python3 BOTWM_Autoupdater_Python.py` in the directory where BOTWM_Autoupdater_Python.py was saved to. (This will make more sense when the mod goes open source and someone makes a version for Linux/MacOS nativly (it could be me)).

&nbsp;

v1: Downloads the latest release.

![version 1](https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Autoupdater/raw/branch/main/images/v1.png)

v2: Allows you to choose between the latest and any version available.

![version 2](https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Autoupdater/raw/branch/main/images/v2.png)

v2-python: v2 remade in Python.

![version 2 in python](https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Autoupdater/raw/branch/main/images/Python.png)

v2-gui: v2 remade with a GUI (only has all versions/can't just choose latest version to download). Also alerts you (when you open the program) if there is a new version available and allows you to download it.

![version 2 with a gui](https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Autoupdater/raw/branch/main/images/GUI.png)

The autoupdater downloads the files from [this repository](https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Release).

Watch [this video](https://www.youtube.com/watch?v=j18yicimeiM) to get BotW Multiplayer set up and working.

If you want to build v1, v2, or v2-python yourself I would recommend editing the files in Visual Studio 2022 then running CMD in the project folder (the one with the sln file) and running this command: `dotnet publish -r win-x64 --self-contained -p:PublishSingleFile=true -c Release`.

If you want to build v2-gui yourself I would recommend editing the program in Visual Studio 2022 then running CMD in the project folder (the one with the sln file) and running this command: `dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeAllContentForSelfExtract=true`.

&nbsp;

NOTE: You WILL NEED the [Newtonsoft.Json](https://www.nuget.org/packages/newtonsoft.json) (all versions), and [Costura.Fody](https://www.nuget.org/packages/Costura.Fody) (v1 and v2) NuGet packages in order for the program to work.

NOTE: If you edit the source code and release is publicly you need to give credit to me (it is already right there in the code).
There is a "forked by *insert name here*" in the code for a reason. Just use it.

NOTE: If you edit the v2-gui source code and release it publicly you need to give credit to me (just add *forked by ____* in the textbox in the designer).