using System;
using System.Diagnostics;
using FFMpegCore;
using ShellProgressBar;

namespace Discordify
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Discordify v1.0");

            string video = "";
            if (args.Length == 0)
            {
                video = GetVideoFromConsole();
            }
            else
            {
                if (File.Exists(args[0]))
                {
                    video = args[0];
                }
                else
                {
                    Console.WriteLine("Invalid video detected, exiting...");
                    Thread.Sleep(1000);
                    Environment.Exit(1);
                }
            }
            Console.WriteLine($"Converting video {video}...");
            ConvertVideo(video);
        }

        private static string GetVideoFromConsole()
        {
            while (true)
            {
                Console.WriteLine("Please input the path of the video that you want to discordify:");
                string? possibleVideo = (Console.ReadLine() ?? "").Trim('"');
                if (File.Exists(possibleVideo))
                {
                    return possibleVideo;
                }
                Console.WriteLine("That doesn't look like a file, please try again.");
            }
        }

        private static void ConvertVideo(string video)
        {
            try
            {
                var videoAnalysis = FFMpegCore.FFProbe.Analyse(video);

                if (videoAnalysis.PrimaryVideoStream == null)
                {
                    Console.WriteLine("Error encountered during conversion, most likely what you have specified isn't a video file, it's corrupted or not supported by ffmpeg.");
                    Console.WriteLine("Exiting...");
                    Thread.Sleep(1000);
                    Environment.Exit(1);
                }

                var videoDuration = videoAnalysis.Duration;
                var videoBitrate = videoAnalysis.PrimaryVideoStream.BitRate;
                int targetBitrate = (int)(75497472 / videoDuration.TotalSeconds);
                string outputVideoFile = String.Join(".", video.Split('.').Take(video.Split('.').Length - 1).ToArray()) + "-discordified.mp4";
                Console.WriteLine($"Video found!");
                Console.WriteLine($"Duration: {videoDuration}");
                Console.WriteLine($"Original bitrate: {videoBitrate / 1000}kbps");
                Console.WriteLine($"Target bitrate: {(int)(targetBitrate / 1000)}kbps ({((double)targetBitrate / videoBitrate * 100).ToString("00.00")}%)");
                var pBarOptions = new ProgressBarOptions
                {
                    ForegroundColor = ConsoleColor.White,
                    ProgressCharacter = '─',
                    ProgressBarOnBottom = true
                };
                using (var pbar = new ProgressBar((int)(videoDuration.TotalSeconds * 10), "Re-encoding...", pBarOptions))
                {
                    int currentProgress = 0;
                    FFMpegArguments
                        .FromFileInput(video)
                        .OutputToFile(outputVideoFile, false, options => options
                            .WithVideoBitrate(targetBitrate / 1000)
                            .WithFastStart())
                        .NotifyOnProgress((progress) =>
                        {
                            int newProgress = (int)(progress.TotalSeconds * 10);
                            if (newProgress > currentProgress)
                            {
                                for (int i = 0; i < newProgress - currentProgress; i++)
                                {
                                    pbar.Tick();
                                }
                                currentProgress = newProgress;
                            }
                        })
                        .ProcessSynchronously();
                }
                string argument = "/select, \"" + outputVideoFile + "\"";
                Process.Start("explorer.exe", argument);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered during conversion, it's possible you have specified something that isn't a video file, it's corrupted or the codec is not supported by ffmpeg.");
                Console.WriteLine("Exception text:");
                Console.WriteLine(e.Message);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

    }
}
