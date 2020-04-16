using ModuleLauncherRebuild.Downloader;
using ModuleLauncherRebuild.Launcher.Json;
using ModuleLauncherRebuild.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild.Launcher
{
    public class LaunchCore
    {
        public string GenerateLaunchArgs()
        {
            LaunchConfiguation launchConfiguation = Global.LaunchConfiguation;
            VersionJson versionJson = Global.LaunchConfiguation.MinecraftSetting.VersionJson; ;
            
            int ClientType = Libraries.GetClientType(versionJson);
            String VersionPath = launchConfiguation.MinecraftSetting.MinecraftSource + $"\\versions\\{launchConfiguation.MinecraftSetting.VersionJson.id}";
            String VersionName = launchConfiguation.MinecraftSetting.VersionJson.id;
            String MinecraftSource = launchConfiguation.MinecraftSetting.MinecraftSource;

            String LauncherArg_NativesPath = VersionPath.Contains("Roaming") ? $"\"{launchConfiguation}\\$natives\"" : $"{VersionPath}\\{VersionName}-natives";
            String LauncherArg_JarPath = ClientType == 1 ? $"{MinecraftSource}\\versions\\{versionJson.inheritsFrom}\\{versionJson.inheritsFrom}.jar" : $"{VersionPath}\\{VersionName}.jar";

            String LauncherArgs = launchConfiguation.JavaSetting.JvmArgument;
            LauncherArgs += "-XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump ";
            if (ClientType == 1)
                LauncherArgs += $"-Dminecraft.client.jar=\"{MinecraftSource}\\versions\\{versionJson.inheritsFrom}\\{versionJson.inheritsFrom}.jar\" ";
            LauncherArgs += $"-Xss1M -Djava.library.path={LauncherArg_NativesPath} ";
            LauncherArgs += $"-Dminecraft.launcher.brand={launchConfiguation.MinecraftSetting.LauncherName } -Dminecraft.launcher.version=ML.net ";

            LauncherArgs += $"-Xmx{launchConfiguation.JavaSetting.MaxMemorySize}G -Xmn{launchConfiguation.JavaSetting.MinMemorySize}G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 ";
            LauncherArgs += $"-XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M ";
            LauncherArgs += $"-cp \"{Libraries.GetLibraries()}{LauncherArg_JarPath}\" {versionJson.mainClass} ";

            LauncherArgs += $"--username {Global.AuthConfiguation.PlayerName} ";
            LauncherArgs += $"--version \"{launchConfiguation.MinecraftSetting.LauncherName}\" ";

            LauncherArgs += $"--gameDir {launchConfiguation.MinecraftSetting.MinecraftSource} ";
            LauncherArgs += $"--assetsDir {launchConfiguation.MinecraftSetting.MinecraftSource}\\assets ";
            LauncherArgs += ClientType == 3 ? $"--assetIndex {versionJson.inheritsFrom.Split('.')[0]}.{versionJson.inheritsFrom.Split('.')[1]} " : $"--assetIndex {versionJson.assets} ";

            LauncherArgs += $"--uuid {Global.AuthConfiguation.PlayerUUID} ";
            LauncherArgs += $"--accessToken {Global.AuthConfiguation.PlayerToken} ";
            LauncherArgs += $"--userType mojang ";

            if (!String.IsNullOrEmpty(launchConfiguation.MinecraftSetting.AutoConnectServer))
            {
                LauncherArgs += $"--server {launchConfiguation.MinecraftSetting.AutoConnectServer} --port 25565 ";
            }
            if (launchConfiguation.MinecraftSetting.WindowWidth != 0 && launchConfiguation.MinecraftSetting.WindowHeight != 0)
            {
                LauncherArgs += $"--width {launchConfiguation.MinecraftSetting.WindowWidth} --height {launchConfiguation.MinecraftSetting.WindowHeight} ";
            }

            LauncherArgs += "--userProperties {}";
            switch (ClientType)
            {
                case 1:
                    LauncherArgs += " --tweakClass net.minecraftforge.fml.common.launcher.FMLTweaker";
                    break;
                case 3:
                    LauncherArgs += " --tweakClass optifine.OptiFineTweaker";
                    break;
                default:
                    break;
            }

            return LauncherArgs;
        }
        private StreamReader Excute(String javaPath, String launchArgs)
        {
            Process process = new Process
            { 
                StartInfo = new ProcessStartInfo(javaPath, launchArgs) 
                { 
                    WorkingDirectory = Global.LaunchConfiguation.MinecraftSetting.MinecraftSource, 
                    UseShellExecute = false, 
                    RedirectStandardInput = false, 
                    RedirectStandardOutput = true, 
                    CreateNoWindow = false
                }
            };
            process.Start();

            return process.StandardOutput;
        }
        public StreamReader Launch()
        {
            String args = GenerateLaunchArgs();
            return Excute(Global.LaunchConfiguation.JavaSetting.JavaPath, args);
        }
    }
}
