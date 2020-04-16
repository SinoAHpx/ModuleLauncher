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
    public class MojangAuthenticator
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public AuthResult Auth()
        {
            AuthSubmit submit = new AuthSubmit() { agent = new AuthAgent() { name = "Minecraft", version = 1 }, username = Account, password = Password, clientToken = "" };
            AuthReturn authReturn = JsonConvert.DeserializeObject<AuthReturn>(NetTools.PostHttp(new Uri("https://authserver.mojang.com/authenticate"), JsonConvert.SerializeObject(submit)));
            AuthResult authResult = new AuthResult()
            {
                PlayerName = authReturn.selectedProfile.name,
                PlayerUUID = authReturn.selectedProfile.id,
                PlayerToken = authReturn.accessToken
            };
            return authResult;
        }
        public async Task<AuthResult> AuthAsync()
        {
            String submit = JsonConvert.SerializeObject(new AuthSubmit
            {
                agent = new AuthAgent
                {
                    name = "Minecraft",
                    version = 1
                },
                username = Account,
                password = Password,
                clientToken = String.Empty
            });
            AuthReturn authReturn = JsonConvert.DeserializeObject<AuthReturn>(await NetTools.PostHttpAsync(new Uri("https://authserver.mojang.com/authenticate"), submit));

            return new AuthResult
            {
                PlayerName = authReturn.selectedProfile.name,
                PlayerUUID = authReturn.selectedProfile.id,
                PlayerToken = authReturn.accessToken
            };
        }
        public string GetAvatar(String uuidOrName, String widthAndHeight = "100")
        {
            return String.IsNullOrEmpty(widthAndHeight) ? String.Format("https://minotar.net/helm/{0}", uuidOrName) : String.Format("https://minotar.net/helm/{0}/{1}.png", uuidOrName, widthAndHeight);
        }

        /// <summary>
        /// 下载文件，然后返回保存路径
        /// </summary>
        /// <param name="uuidOrName"></param>
        /// <param name="savePath"></param>
        /// <param name="userAvatarType">avatar:表示单头像 cube:表示整个头 body:表示整个皮肤正面 helm:表示带帽子的头像 bust:表示半身像</param>
        /// <param name="widthAndHeight"></param>
        /// <returns></returns>
        public async Task<string> GetAvatarAsync(String uuidOrName, String savePath, UserAvatarType userAvatarType = UserAvatarType.HELM, String widthAndHeight = "100")
        {
            String AvatarType = "helm";
            switch (userAvatarType)
            {
                case UserAvatarType.AVATAR:
                    AvatarType = "AVATAR".ToLower();
                    break;
                case UserAvatarType.CUBE:
                    AvatarType = "CUBE".ToLower();
                    break;
                case UserAvatarType.BODY:
                    AvatarType = "BODY".ToLower();
                    break;
                case UserAvatarType.BUST:
                    AvatarType = "BUST".ToLower();
                    break;
                case UserAvatarType.HELM:
                    AvatarType = "HELM".ToLower();
                    break;
                default:
                    break;
            }
            Uri uri = String.IsNullOrEmpty(widthAndHeight) ? new Uri($"https://minotar.net/{AvatarType}/{uuidOrName}")
                : new Uri($"https://minotar.net/{AvatarType}/{uuidOrName}/{widthAndHeight}.png");
            await NetTools.DownloadFileAsync(uri, savePath);
            return savePath;
        }
    }
    public enum UserAvatarType
    {
        AVATAR, CUBE, BODY, BUST, HELM
    }
}
