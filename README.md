
# <img src="Discordify/discordify.png" width="45"> Discordify 
Discordify is a simple cli application to compress any video to a size of less than 10MB (Discord's free upload limit). It's made in C# using FFMpegCore as an FFmpeg wrapper. Discordify is not associated with discord in any way.
# Dependencies
To use discordify, [FFmpeg](https://www.ffmpeg.org/) must be installed on your system and added to the PATH enviroment variable, or the FFmpeg executable must be located in the same folder as Discordify.
# Installation
Download the latest version from the releases page, unzip it somewhere, run the executable, enjoy :)
## Optional: Add to PATH
To able able to use discordify anywhere on your system, you must add the executable to your PATH enviroment variable, therefore making it available to your shell anywhere. If you don't know how to do that, there are a ton of great tutorials available one google search away.
# Usage
There are three main ways to use this application, normally, using the "open with" menu, and from the cli. Regardless of the approach, the app produces a file named "videoname-discordified.mp4" with your discordified video. If for some reason the video ends up being over 10MB, you can just run it through the tool again.
## Normal Usage
When you start the app with no cli arguments (if you don't know what that is, don't worry), it will prompt you for the path of your video. You can easily aquire it right clicking it and pressing **"Copy as path"**. Once you paste it in, you just let it do the work, and it will open an explorer window with your converted video when it's done.
## Using the "Open with" menu
If you don't wanna manually enter the file path every time, you can right click on the video and open it with discordify (just like you would do with your video player), and the conversion will start automatically.
## Usage via cli
You can also pass the path of the video file as the first argument to Discordify to achieve the same result as converting using the "Open with" menu.
