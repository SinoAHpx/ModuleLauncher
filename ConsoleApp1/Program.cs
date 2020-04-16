using ModuleLauncherRebuild;
using ModuleLauncherRebuild.Authenticator;
using ModuleLauncherRebuild.Authenticator.AuthData;
using ModuleLauncherRebuild.Downloader;
using ModuleLauncherRebuild.Launcher;
using ModuleLauncherRebuild.Launcher.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            OfflineAuthenticator mojangAuthenticator = new OfflineAuthenticator
            {
                Account = "AHpx@yandex.com",
            };
            AuthResult authResult = mojangAuthenticator.Auth();
            Console.WriteLine(authResult.PlayerName);
            Global.AuthConfiguation = authResult;
            Global.LaunchConfiguation = new LaunchConfiguation
            {
                JavaSetting = new JavaSetting
                {
                    MaxMemorySize = 10,
                    MinMemorySize = 6,
                    JavaPath = @"C:\Program Files\Java\jre1.8.0_241\bin\javaw.exe",
                },
                MinecraftSetting = new MinecraftSetting
                {
                    LauncherName = "Tets",
                    MinecraftSource = @"D:\Minecraft\Solution1\.minecraft",
                    VersionJson = JsonStorage.ParseVersionJson(@"D:\Minecraft\Solution1\.minecraft\versions\1.8.9")
                }
            };
            foreach (var item in Libraries.GetExistLibraries())
            {
                Console.WriteLine(item.DownloadUri);
                Console.WriteLine(item.FileName);
            }
            LaunchCore launchCore = new LaunchCore();
            String argss = launchCore.GenerateLaunchArgs();
            Console.WriteLine(argss);
            launchCore.ExtraNatives();
            
            

            Console.ReadLine();
        }
    }
}
