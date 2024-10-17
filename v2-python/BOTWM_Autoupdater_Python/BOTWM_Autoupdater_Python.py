#!/usr/bin/env python


import os
import requests
import zipfile
import json
import time

repo_api_url = "https://gitea.30-seven.cc/api/v1/repos/Wesley/BotW.Multiplayer.Release/releases"
release_zip_file = "latest_release.zip"
version_file_name = "Version.txt"
updater_file_name = ["BOTWM_Autoupdater.py", "BOTWM_Autoupdater_Python.py"]

def main():
    print("*************************************************************************")
    print("*        The 'better' Breath of the Wild Multiplayer Autoupdater        *")
    print("*  Built from the ground up Wesley Hellewell released on Oct 11th 2024  *")
    print("*      This auto updater will prepare the mod installation for you      *")
    print("*          Please do not close this installer until it is done          *")
    print("*************************************************************************\n")

    if contains_other_files_or_folders():
        print("Other files or folders detected. The updater will not run. Please ensure the folder contains only the updater and BotW Multiplayer files.")
        input("Press Enter to close the autoupdater.")
        return

    current_version = get_current_version_from_folder()
    latest_version = get_latest_version_from_gitea()

    choice = input("Press 1 to update to the latest version or 2 to select a specific version: ")

    if choice == '1':
        if is_version_newer(latest_version, current_version):
            print(f"New release found: v{latest_version}. Updating...")
            delete_files_except_updater()
            download_latest_release(latest_version)
            extract_release()
        else:
            print("You already have the latest release. This window will close automatically.")
            time.sleep(3)
    elif choice == '2':
        available_versions = get_available_versions()
        print("Available versions:")
        for idx, version in enumerate(available_versions):
            print(f"{idx + 1}: v{version}")

        version_choice = input("Select a version number to download: ")
        if version_choice.isdigit() and 0 < int(version_choice) <= len(available_versions):
            selected_version = available_versions[int(version_choice) - 1]
            print(f"Downloading version: v{selected_version}...")
            delete_files_except_updater()
            download_latest_release(selected_version)
            extract_release()
        else:
            print("Invalid selection. Closing the updater.")
    else:
        print("Invalid key pressed. Closing the updater.")

def contains_other_files_or_folders():
    folder_path = os.getcwd()
    allowed_files = {
        "BOTWM_Autoupdater.py",
        "BOTWM_Autoupdater_Python.py",
        "BOTWM_Autoupdater.exe", "Breath of the Wild Multiplayer.deps.json", 
        "Breath of the Wild Multiplayer.dll", "Breath of the Wild Multiplayer.exe", 
        "Breath of the Wild Multiplayer.runtimeconfig.json", "Newtonsoft.Json.dll", 
        "Version.txt"
    }
    
    allowed_folders = {"Backgrounds", "BNPs", "DedicatedServer", "Resources"}
    
    other_files = [f for f in os.listdir(folder_path) if os.path.isfile(f) and f not in allowed_files]
    other_folders = [d for d in os.listdir(folder_path) if os.path.isdir(d) and d not in allowed_folders]

    return len(other_files) > 0 or len(other_folders) > 0

def get_current_version_from_folder():
    version_file_path = os.path.join(os.getcwd(), version_file_name)
    if os.path.exists(version_file_path):
        with open(version_file_path, 'r') as file:
            return file.read().strip()
    return "0.0"

def get_latest_version_from_gitea():
    response = requests.get(repo_api_url)
    response.raise_for_status()
    releases = response.json()
    version_tags = [release["tag_name"] for release in releases]
    version_tags.sort(key=lambda v: [int(x) for x in v.split('.')], reverse=True)
    return version_tags[0]

def get_available_versions():
    response = requests.get(repo_api_url)
    response.raise_for_status()
    releases = response.json()
    return sorted([release["tag_name"] for release in releases], reverse=True)

def is_version_newer(latest_version, current_version):
    return [int(x) for x in latest_version.split('.')] > [int(x) for x in current_version.split('.')]

def delete_files_except_updater():
    folder_path = os.getcwd()
    for item in os.listdir(folder_path):
        item_path = os.path.join(folder_path, item)
        # Skip deletion for any file in the updater_file_names list
        if item not in updater_file_name:
            if os.path.isfile(item_path):
                os.remove(item_path)
            elif os.path.isdir(item_path):
                os.rmdir(item_path)

def download_latest_release(version):
    download_url = f"https://gitea.30-seven.cc/Wesley/BotW.Multiplayer.Release/releases/download/{version}/{version}.zip"
    response = requests.get(download_url)
    response.raise_for_status()
    with open(release_zip_file, 'wb') as file:
        file.write(response.content)

def extract_release():
    with zipfile.ZipFile(release_zip_file, 'r') as zip_ref:
        zip_ref.extractall(os.getcwd())
    os.remove(release_zip_file)
    extracted_version = get_current_version_from_folder()
    print(f"Updated to version: {extracted_version}")

if __name__ == "__main__":
    main()
