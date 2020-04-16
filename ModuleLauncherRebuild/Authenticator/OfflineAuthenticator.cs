using ModuleLauncherRebuild.Authenticator.AuthData;
using ModuleLauncherRebuild.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild.Authenticator
{
    public class OfflineAuthenticator
    {
        public string Account { get; set; }
        public AuthResult Auth()
        {
            String rdm = StringTools.GetRandomString(32);
            AuthResult authResult = new AuthResult()
            {
                PlayerName = Account,
                PlayerUUID = rdm,
                PlayerToken = rdm
            };
            return authResult;
        }
    }
}
