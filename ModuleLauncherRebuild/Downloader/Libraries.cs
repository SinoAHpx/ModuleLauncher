using ModuleLauncherRebuild.Launcher.Json;
using ModuleLauncherRebuild.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild.Downloader
{
    public class LibrariesInfo
    {
        public string DownloadUri { get; set; }
        public string FileName { get; set; }
    }
    public static class Libraries
    {
        public static string GetLibraries()
        {
            VersionJson versionJson = Global.LaunchConfiguation.MinecraftSetting.VersionJson;
            switch (GetClientType(versionJson))
            {
                case 0:
                    return GetLibraries_Vanilla();
                case 1:
                    return GetLibraries_Forge();
                case 2:
                    return GetLibraries_VanillaHigh();
                case 3:
                    return GetLibraries_Optifine();
                default:
                    return null;
            }
        }
        public static List<LibrariesInfo> GetMissLibraries()
        {
            VersionJson versionJson = Global.LaunchConfiguation.MinecraftSetting.VersionJson;
            List<String> libs = new List<String>();
            List<LibrariesInfo> re = new List<LibrariesInfo>();
            switch (GetClientType(versionJson))
            {
                case 0:
                    libs = GetLibraries_Vanilla().Split(';').ToList();
                    break;
                case 1:
                    libs = GetLibraries_Forge().Split(';').ToList();
                    break;
                case 2:
                    libs = GetLibraries_VanillaHigh().Split(';').ToList();
                    break;
                case 3:
                    libs = GetLibraries_Optifine().Split(';').ToList();
                    break;
            }
            foreach (var item in libs)
            {
                Console.WriteLine(item);
                if (!File.Exists(item))
                {
                    String uriPart = item.Replace($"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\libraries\\", String.Empty).Replace("\\", "/");
                    if (!String.IsNullOrEmpty(uriPart))
                    {
                        re.Add(new LibrariesInfo
                        {
                            DownloadUri = $"https://bmclapi2.bangbang93.com/maven/{uriPart}",
                            FileName = item
                        });
                    }
                }
            }
            return re;
        }
        public static List<LibrariesInfo> GetExistLibraries()
        {
            VersionJson versionJson = Global.LaunchConfiguation.MinecraftSetting.VersionJson;
            List<String> libs = new List<String>();
            List<LibrariesInfo> re = new List<LibrariesInfo>();
            switch (GetClientType(versionJson))
            {
                case 0:
                    libs = GetLibraries_Vanilla().Split(';').ToList();
                    break;
                case 1:
                    libs = GetLibraries_Forge().Split(';').ToList();
                    break;
                case 2:
                    libs = GetLibraries_VanillaHigh().Split(';').ToList();
                    break;
                case 3:
                    libs = GetLibraries_Optifine().Split(';').ToList();
                    break;
            }
            foreach (var item in libs)
            {
                Console.WriteLine(item);
                if (!File.Exists(item))
                {
                    String uriPart = item.Replace($"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\libraries\\", String.Empty).Replace("\\", "/");
                    if (String.IsNullOrEmpty(uriPart))
                    {
                        re.Add(new LibrariesInfo
                        {
                            DownloadUri = $"https://bmclapi2.bangbang93.com/maven/{uriPart}",
                            FileName = item
                        });
                    }
                }
            }
            return re;
        }
        private static string GetLibraries_Vanilla()
        {
            String re = String.Empty;
            VersionJson versionJson = Global.LaunchConfiguation.MinecraftSetting.VersionJson;
            for (int i = 0; i < versionJson.libraries.Count; i++)
            {
                try
                {
                    re += $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\libraries\\{versionJson.libraries[i].downloads.artifact.path};".Replace("/", "\\");
                }
                catch (Exception)
                {
                    try
                    {
                        re += $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\libraries\\{versionJson.libraries[i].downloads.classifiers.natives_windows.path};".Replace("/", "\\");
                    }
                    catch (Exception)
                    {

                        if (SystemTools.Isx64()) re += $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\libraries\\{versionJson.libraries[i].downloads.classifiers.natives_windows_64.path};".Replace("/", "\\");
                        else re += $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\libraries\\{versionJson.libraries[i].downloads.classifiers.natives_windows_32.path};".Replace("/", "\\");
                    }
                }
            }
            return re;
        }
        private static string GetLibraries_VanillaHigh()
        {
            VersionJson versionJson = Global.LaunchConfiguation.MinecraftSetting.VersionJson;
            String re = String.Empty;
            for (int i = 0; i < versionJson.libraries.Count; i++)
            {
                try
                {
                    re += $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\libraries\\{versionJson.libraries[i].downloads.artifact.path};".Replace("/", "\\");
                    re += $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\libraries\\{versionJson.libraries[i].downloads.classifiers.natives_windows.path};".Replace("/", "\\");
                }
                catch (Exception) { }
            }
            return re;
        }
        private static string GetLibraries_Forge()
        {
            VersionJson versionJson = Global.LaunchConfiguation.MinecraftSetting.VersionJson;
            VersionJson versionJsonVanilla = JsonStorage.ParseVersionJson($"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\versions\\{versionJson.inheritsFrom}");
            String re = String.Empty;
            for (int i = 0; i < versionJson.libraries.Count; i++)
            {
                String[] split = $"{versionJson.libraries[i].name}".Split(':');
                String tempPath = $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\libraries\\{split[0].Replace(".", "\\")}\\{split[1]}\\{split[2]}";
                Console.WriteLine("==========================TEMP" + tempPath);
                foreach (String item in Directory.GetFiles(tempPath))
                {
                    re += $"{item};";
                }
            }
            return GetLibraries_Vanilla() + re;
        }
        private static string GetLibraries_Optifine()
        {
            VersionJson versionJson = Global.LaunchConfiguation.MinecraftSetting.VersionJson;
            VersionJson versionJsonVanilla = JsonStorage.ParseVersionJson($"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\versions\\{versionJson.inheritsFrom}");
            String re = String.Empty;
            for (int i = 0; i < versionJson.libraries.Count; i++)
            {
                String[] split = $"{versionJson.libraries[i].name}".Split(':');
                String tempPath = $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\libraries\\{split[0].Replace(".", "\\")}\\{split[1]}\\{split[2]}";
                Console.WriteLine("==========================TEMP" + tempPath);
                foreach (String item in Directory.GetFiles(tempPath))
                {
                    re += $"{item};";
                }
            }
            Console.WriteLine(GetClientType(versionJsonVanilla));
            switch (GetClientType(versionJsonVanilla))
            {
                case 0:
                    return GetLibraries_Vanilla() + re;
                case 2:
                    return GetLibraries_VanillaHigh() + re;
                default:
                    return re;
            }

        }

        /// <summary>
        /// 0:Vanilla Low 1:Forge 2:VanillaHigh 3:Optifine
        /// </summary>
        /// <param name="versionJson"></param>
        /// <returns></returns>
        public static int GetClientType(VersionJson versionJson)
        {

            if (Convert.ToInt32(versionJson.id.Split('.')[1]) >= 13 && !versionJson.id.ToLower().Contains("optifine") && !versionJson.id.ToLower().Contains("forge"))
            {
                return 2;
            }
            else if (versionJson.id.ToLower().Contains("optifine"))
            {
                return 3;
            }
            else
            {
                return versionJson.id.ToLower().Contains("forge") ? 1 : 0;
            }
        }
    }
}
