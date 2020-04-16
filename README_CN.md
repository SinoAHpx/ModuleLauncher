# Module Launcher .Net

+ Module launcher是一个使用C#编写的Minecraft启动核心

+ 开发环境: Visual Studio 2019 在Windows 10 1809上

### 介绍

+ 本库基于易语言版[ModuleLauncher](https://www.mcbbs.net/thread-815868-1-1.html)使用C#重写

+ 支持所有的Minecraft版本
+ 正版账号验证
+ 获取资源文件和库文件信息
+ 自动获取Java列表
+ 以后会**更多**

### 文档

+ ##### 正版账户验证

```
MojangAuthenticator mojangAuthenticator = new MojangAuthenticator
{
	Account = "Email",
	Password = "Password"
};
AuthResult authResult = mojangAuthenticator.Auth();
```

---

+ 离线账户验证r

```c#
OfflineAuthenticator offlineAuthenticator = new OfflineAuthenticator
{
	Account = "UserName",
};
AuthResult authResult = offlineAuthenticator.Auth();
```

---

+ 初始化启动核心

```c#
Global.LaunchConfiguation = new LaunchConfiguation
{
	JavaSetting = new JavaSetting
    {
     	MaxMemorySize = GB,
        MinMemorySize = GB,
        JavaPath = javaw.exe absolute path
    },
    MinecraftSetting = new MinecraftSetting
    {
    	LauncherName = your launcher name,
        MinecraftSource = .minecraft,
        VersionJson = JsonStorage.ParseVersionJson(FullMinecraftPVersionPath)
    }
};
```

+ 启动Minecraft

```C#
LaunchCore launchCore = new LaunchCore();
launchCore.GenerateLaunchArgs();//Generate launch arguments
launchCore.Launch();
```

