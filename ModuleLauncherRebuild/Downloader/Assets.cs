using ModuleLauncherRebuild.Launcher.Json;
using ModuleLauncherRebuild.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild.Downloader
{
    public static class Assets
    {
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
        public static async Task<List<AssetsInfo>> GetAssetsInfosAsync()
        {
            List<AssetsModule> assetsModules = await ParseAssetsAsync();
            List<AssetsInfo> re = new List<AssetsInfo>();
            foreach (var item in assetsModules)
            {
                String prefix = item.hash.Substring(0, 2);
                re.Add(new AssetsInfo 
                { 
                    DownloadUri = $"https://bmclapi2.bangbang93.com/assets/{prefix}/{item.hash}", 
                    FileName = $@"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\assets\objects\{prefix}\{item.hash}"
                });
            }

            return re;
        }
        public static async Task<List<AssetsInfo>> GetMissAssetsInfosAsync()
        {
            List<AssetsInfo> assetsInfos = await GetAssetsInfosAsync();
            List<AssetsInfo> re = new List<AssetsInfo>();
            foreach (var item in assetsInfos)
            {
                if (!File.Exists(item.FileName))
                {
                    re.Add(item);
                }
            }

            return re;
        }
        public static async Task<List<AssetsModule>> ParseAssetsAsync()
        {
            VersionJson versionName = Global.LaunchConfiguation.MinecraftSetting.VersionJson;
            String jsonName;

            VersionJson versionJson;
            int clientType = GetClientType(Global.LaunchConfiguation.MinecraftSetting.VersionJson);
            if (clientType == 1 || clientType == 3)
            {
                versionJson = JsonStorage.ParseVersionJson(Global.LaunchConfiguation.MinecraftSetting.VersionJson.inheritsFrom);
                jsonName = $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\assets\\indexes\\{versionJson.assetIndex.id}.json";
                if (!File.Exists(jsonName))
                {
                    await NetTools.DownloadFileAsync(new Uri($"https://bmclapi2.bangbang93.com/v1/packages/{versionJson.assetIndex.sha1}/{versionJson.assetIndex.id}.json"), jsonName);
                }
                
            }
            else
            {
                versionJson = Global.LaunchConfiguation.MinecraftSetting.VersionJson;
                jsonName = $"{Global.LaunchConfiguation.MinecraftSetting.MinecraftSource}\\assets\\indexes\\{versionName.assetIndex.id}.json";
                if (!File.Exists(jsonName))
                {
                    await NetTools.DownloadFileAsync(new Uri($"https://bmclapi2.bangbang93.com/v1/packages/{versionJson.assetIndex.sha1}/{versionJson.assetIndex.id}.json"), jsonName);
                }
            }

            String jsonText = File.ReadAllText(jsonName);
            List<AssetsModule> re = new List<AssetsModule>();
            foreach (DictionaryEntry item in AnalayJson(jsonText))
            {
                foreach (DictionaryEntry sub in AnalayJson(item.Value.ToString()))
                {
                    re.Add(JsonConvert.DeserializeObject<AssetsModule>(sub.Value.ToString()));
                }
            }

            return re;
        }
        private static Hashtable AnalayJson(String jsonText)
        {
            Hashtable hashtable = new Hashtable();
            if (!String.IsNullOrEmpty(jsonText))
            {
                JObject jObject = JsonConvert.DeserializeObject(jsonText) as JObject;
                foreach (var item in jObject)
                {
                    hashtable.Add(item.Key,item.Value);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return hashtable;
        }
    }

    public class AssetsModule
    {
        public string hash { get; set; }
        public int size { get; set; }
    }
    public class AssetsInfo
    {
        public string DownloadUri { get; set; }
        public string FileName { get; set; }
    }
}
