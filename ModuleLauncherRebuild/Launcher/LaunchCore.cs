using ModuleLauncherRebuild.Downloader;
using ModuleLauncherRebuild.Launcher.Json;
using ModuleLauncherRebuild.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
        private List<String> RemoveRepeat(List<String> src)
        {
            List<String> re = src;
            for (int i = 0; i < re.Count; i++)
            {
                string istr = re[i];
                for (int j = re.IndexOf(istr) + 1; j < re.Count; j++)
                {
                    string jstr = re[j];
                    if (istr == jstr)
                    {
                        re.Remove(jstr);
                    }
                }
            }
            return re;
        }
        public void ExtraNatives()
        {
            String MinecraftPath = $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}";
            String VersionPath = $"{MinecraftPath}\\versions\\{Global.LaunchConfiguation.MinecraftSetting.VersionJson.id}";
            String jsonName = $"{VersionPath}\\{Global.LaunchConfiguation.MinecraftSetting.VersionJson.id}.json";
            String jsonText = File.ReadAllText(jsonName);

            List<String> re = new List<string>();
            LibrariesProperty librariesProperty = JsonConvert.DeserializeObject<LibrariesProperty>(jsonText);
            foreach (var item in librariesProperty.libraries) { try { if (File.Exists($"{MinecraftPath}\\libraries\\{item.downloads.classifiers.natives_windows.path}".Replace("/", "\\"))) re.Add($"{MinecraftPath}\\libraries\\{item.downloads.classifiers.natives_windows.path}".Replace("/", "\\")); } catch { }}
            foreach (var item in librariesProperty.libraries) { try {if (File.Exists($"{MinecraftPath}\\libraries\\{item.downloads.classifiers.natives_Windows_64.path}".Replace("/", "\\"))) re.Add($"{MinecraftPath}\\libraries\\{item.downloads.classifiers.natives_Windows_64.path}".Replace("/", "\\")); } catch { } }
            foreach (var item in librariesProperty.libraries) { try {if (File.Exists($"{MinecraftPath}\\libraries\\{item.downloads.classifiers.natives_Windows_32.path}".Replace("/", "\\"))) re.Add($"{MinecraftPath}\\libraries\\{item.downloads.classifiers.natives_Windows_32.path}".Replace("/", "\\")); } catch { } }

            foreach (var item in RemoveRepeat(re))
            {
                Console.WriteLine("File name: "+ item);
                try { ZipFile.ExtractToDirectory(item, $"{VersionPath}\\{Global.LaunchConfiguation.MinecraftSetting.VersionJson.id}-natives"); } catch { }
            }
        }
        public StreamReader Launch(bool AutoExtraNatives = true)
        {
            if (AutoExtraNatives)
            {
                ExtraNatives();
            }
            String args = GenerateLaunchArgs();
            return Excute(Global.LaunchConfiguation.JavaSetting.JavaPath, args);
        }
    }
    public class LibrariesProperty
    {
        public List<DownloadProperty> libraries { get; set; }
    }
    public class DownloadProperty
    {
        public Classifiers downloads { get; set; }
    }
    public class Classifiers
    { 
        public ClassifierPropertys classifiers { get; set; }
    }
    public class ClassifierPropertys
    {
        [JsonProperty("natives-windows")]
        public NativesInfo natives_windows { get; set; }
        [JsonProperty("natives-windows-64")]
        public NativesInfo natives_Windows_64 { get; set; }
        [JsonProperty("natives-windows-32")]
        public NativesInfo natives_Windows_32 { get; set; }
    }
    public class NativesInfo
    {
        public string path { get; set; }
        public string sha1 { get; set; }
        public string size { get; set; }
        public string url { get; set; }
    }
}
