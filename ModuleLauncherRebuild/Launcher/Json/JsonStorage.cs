using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild.Launcher.Json
{
    public static class JsonStorage
    {
        /// <summary>
        /// Absolute path like D:\Minecraft\Solution1\.minecraft\versions\1.8
        /// </summary>
        /// <param name="versionName"></param>
        /// <returns></returns>
        public static VersionJson ParseVersionJson(String versionName)
        {
            String versionPath = $"{versionName}\\{Path.GetFileName(versionName)}.json";
            if (File.Exists(versionPath))
            {
                String VersionJsonText = File.ReadAllText(versionPath);

                return JsonConvert.DeserializeObject<VersionJson>(VersionJsonText);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }

    public class VersionJson
    {
        public VerJson_assetIndex assetIndex { get; set; }
        public string assets { get; set; }
        public VerJson_downloads downloads { get; set; }
        public string id { get; set; }
        public List<VerJson_libraries> libraries { get; set; }
        public VerJson_logging_client logging { get; set; }
        public string mainClass { get; set; }
        public string minecraftArguments { get; set; }
        public string minimumLauncherVersion { get; set; }
        public string releaseTime { get; set; }
        public string time { get; set; }
        public string type { get; set; }
        public string inheritsFrom { get; set; }
        public string jar { get; set; }
    }
    public class VerJson_assetIndex
    {
        public string id { get; set; }
        public string sha1 { get; set; }
        public string size { get; set; }
        public string totalSize { get; set; }
        public string url { get; set; }
    }
    public class VerJson_downloads
    {
        public VerJson_downloads_items client { get; set; }
        public VerJson_downloads_items server { get; set; }
        public VerJson_downloads_items windows_server { get; set; }
    }
    public class VerJson_downloads_items
    {
        public string sha1 { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }
    public class VerJson_libraries
    {
        public string url { get; set; }
        public List<string> checksums { get; set; }
        public bool serverreq { get; set; }
        public bool clientreq { get; set; }
        public VerJson_libraries_downloads downloads { get; set; }
        public VerJson_libraries_extract extract { get; set; }
        public string name { get; set; }
        public VerJson_libraries_natives natives { get; set; }
        public List<VerJson_libraries_downloads_rule> rules { get; set; }
    }
    public class VerJson_libraries_downloads
    {
        public VerJson_libraries_downloads_artifact artifact { get; set; }
        [JsonIgnore]
        public VerJson_libraries_classifiers classifiers { get; set; }
    }
    public class VerJson_libraries_downloads_artifact
    {
        public string path { get; set; }
        public string sha1 { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }
    public class VerJson_libraries_downloads_rule
    {
        public string action { get; set; }
        public VerJson_libraries_downloads_rule_os os { get; set; }
    }
    public class VerJson_libraries_downloads_rule_os
    {
        public string name { get; set; }
    }
    public class VerJson_libraries_classifiers
    {
        [JsonProperty("natives-linux")]
        public VerJson_libraries_classifiers_items natives_linux { get; set; }
        [JsonProperty("natives-osx")]
        public VerJson_libraries_classifiers_items natives_osx { get; set; }
        [JsonProperty("natives-windows")]
        public VerJson_libraries_classifiers_items natives_windows { get; set; }
        [JsonProperty("natives-windows-32")]
        public VerJson_libraries_classifiers_items natives_windows_32 { get; set; }
        [JsonProperty("natives-windows-64")]
        public VerJson_libraries_classifiers_items natives_windows_64 { get; set; }
        public VerJson_libraries_classifiers_items javadoc { get; set; }
        public VerJson_libraries_classifiers_items sources { get; set; }
    }
    public class VerJson_libraries_classifiers_items
    {
        public string path { get; set; }
        public string sha1 { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }
    public class VerJson_libraries_extract
    {
        public List<string> exclude { get; set; }
    }
    public class VerJson_libraries_natives
    {
        public string linux { get; set; }
        public string osx { get; set; }
        public string windows { get; set; }
    }
    public class VerJson_logging_client
    {
        public string argument { get; set; }
        public VerJson_logging_client_file file { get; set; }
        public string type { get; set; }
    }
    public class VerJson_logging_client_file
    {
        public string id { get; set; }
        public string sha1 { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }
}
