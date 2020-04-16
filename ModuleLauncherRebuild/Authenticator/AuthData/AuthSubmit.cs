using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild.Authenticator.AuthData
{
    public class AuthSubmit
    {
        public AuthAgent agent { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string clientToken { get; set; }
    }
    public class AuthAgent
    {
        public string name { get; set; }
        public int version { get; set; }
    }
    public class AuthReturn
    {
        public string accessToken { get; set; }
        public string clientToken { get; set; }
        public AuthSelectedProfile selectedProfile { get; set; }
        public List<AuthAvailableProfiles> availableProfiles { get; set; }
    }
    public class AuthSelectedProfile
    {
        /// <summary>
        /// //不含-的uuid
        /// </summary>
        public string id { get; set; }
        public string name { get; set; }
    }
    public class AuthAvailableProfiles
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
