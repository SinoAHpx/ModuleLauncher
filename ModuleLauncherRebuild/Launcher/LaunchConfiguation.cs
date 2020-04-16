using ModuleLauncherRebuild.Launcher.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild.Launcher
{
    public class LaunchConfiguation
    {
        public JavaSetting JavaSetting { get; set; }
        public MinecraftSetting MinecraftSetting { get; set; }
    }

    public class JavaSetting
    {
        public string JavaPath { get; set; }
        public string JvmArgument { get; set; }
        public int MaxMemorySize { get; set; }
        public int MinMemorySize { get; set; }
    }
    public class MinecraftSetting
    {
        public string AutoConnectServer { get; set; }
        public bool IsFullscreen { get; set; }
        public int WindowHeight{ get; set; }
        public int WindowWidth { get; set; }
        /// <summary>
        /// .minecraft
        /// </summary>
        public string MinecraftSource { get; set; }
        public string LauncherName { get; set; }
        public VersionJson VersionJson { get; set; }
    }
}
