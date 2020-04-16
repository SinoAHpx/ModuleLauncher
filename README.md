# Module Launcher .Net

+ Module launcher is a Minecraft launch core written in C#

+ Develop environment: Visual Studio 2019 on Windows 10 1809

### Introduction

+ This library based on EPL edition [ModuleLauncher](https://www.mcbbs.net/thread-815868-1-1.html) and remake with C#.

+ Support all version Minecraft launch
+ Offline authenticator and Mojang authenticator
+ Get Assets and Libraries information
+ Auto get Java list
+ **More** in future

### Document

+ ##### Mojang Authenticator

```
MojangAuthenticator mojangAuthenticator = new MojangAuthenticator
{
	Account = "Email",
	Password = "Password"
};
AuthResult authResult = mojangAuthenticator.Auth();
```

---

+ Offline Authenticator

```c#
OfflineAuthenticator offlineAuthenticator = new OfflineAuthenticator
{
	Account = "UserName",
};
AuthResult authResult = offlineAuthenticator.Auth();
```

---

+ Initialize launch core

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

+ Launch Minecraft

```C#
LaunchCore launchCore = new LaunchCore();
launchCore.GenerateLaunchArgs();//Generate launch arguments
launchCore.Launch();
```
