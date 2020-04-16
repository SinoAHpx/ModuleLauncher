using ModuleLauncherRebuild.Authenticator.AuthData;
using ModuleLauncherRebuild.Launcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild
{
    public static class Global
    {
        public static AuthResult AuthConfiguation { get; set; }
        public static LaunchConfiguation LaunchConfiguation { get; set; }
    }
}
