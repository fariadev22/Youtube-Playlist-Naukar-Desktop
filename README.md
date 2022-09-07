# Youtube Playlist Naukar
This repository contains source coude for a Windows Form desktop application for managing YouTube playlists. 

## Features
The application has the following features:

### Accounts

* Ability to login via any Gmail account that has a YouTube channel linked to it.
* Ability to add multiple YouTube accounts and switch between them.
* Ability to remove/forget a YouTube account from the application.

### Playlists

<img width="629" alt="Playlists Preview" src="https://user-images.githubusercontent.com/20472892/188941119-377ca699-761a-42a3-b611-ad8f893a7107.PNG">

* Ability to view and manage a list of playlists owned by the user (both private and public).
* Ability to manage playlists that the user is not an owner of but just a contributor. To manage such playlists the user will have to explicitly add the playlist entry to the application. The entries can be removed if required.
* The ability to refresh playlists data if needed.
* The ability to preview details about a playlist e.g. its title, description, thumbnail, URL, owner, privacy status, date it was created on, videos count, etc. 

### Playlist Videos
<img width="960" alt="Playlist Videos Preview" src="https://user-images.githubusercontent.com/20472892/188940499-543c9a3e-71b4-4051-8623-430b3337984d.PNG">

* Ability to view list of videos associated with a playlist.
* Ability to search videos in a playlist (fuzzy search).
* Ability to add one or more than one videos to the playlist in one go.
* Ability to delete a video from the playlist.
* Ability to filter the videos list to show only duplicates.
* Ability to filter the videos list to show videos that have been removed or made private by the owner.
* Ability to refresh data of videos from a playlist.
* Ability to preview details about a playlist video e.g. title, description, thumbnail, duration, URL, owner, privacy status, date it was added to playlist on, position in playlist, details of who added the video to playlist, etc.

## How to Use
* Create a client Id and client secret by registering the application on Google Cloud. See [link](https://support.google.com/cloud/answer/6158849?hl=en) for more details. 
* Download the JSON file and rename it to "client_secrets.json". Place it in the `Youtube Playlist Naukar Windows` directory and set its Build action to "Content".
* Get the application verified from Google.
* Build the application.
* Run the application by opening the generated .exe file.
