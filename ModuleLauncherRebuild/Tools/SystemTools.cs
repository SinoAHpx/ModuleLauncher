using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild.Tools
{
    public static class SystemTools
    {
        public static bool Isx64()
        {
            return Directory.Exists(@"C:\Program Files (x86)");
        }
        public static List<JavaInfo> GetJavaInfos()
        {
            List<JavaInfo> re = new List<JavaInfo>();
            foreach (var item in GetJavaStringList())
            {
                re.Add(new JavaInfo { JavaPath = GetJava(item), JavaBitType = item.Contains("x86") });
            }

            return re;
        }
        public static String[] GetJavaStringList()
        {
            bool x64 = Isx64();
            if (x64)
            {
                if (Directory.Exists(@"C:\Program Files\Java") && !Directory.Exists(@"C:\Program Files (x86)\Java"))
                {
                    return Directory.GetDirectories(@"C:\Program Files\Java");
                }
                else if (Directory.Exists(@"C:\Program Files (x86)\Java") && !Directory.Exists(@"C:\Program Files\Java"))
                {
                    return Directory.GetDirectories(@"C:\Program Files (x86)\Java");
                }
                else if (Directory.Exists(@"C:\Program Files (x86)\Java") && Directory.Exists(@"C:\Program Files\Java"))
                {
                    String[] arr1 = Directory.GetDirectories(@"C:\Program Files\Java");
                    String[] arr2 = Directory.GetDirectories(@"C:\Program Files (x86)\Java");

                    return arr1.Concat(arr2).ToArray();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (Directory.Exists(@"C:\Program Files\Java"))
                {
                    return Directory.GetDirectories(@"C:\Program Files\Java");
                }
                else
                {
                    return null;
                }
            }
        }
        public static String GetJava(String text)
        {
            String re = text;
            if (re.EndsWith("\\"))
            {
                re = re.Remove(re.Length - 1);
            }
            if (!String.IsNullOrWhiteSpace(re))
            {
                if (re.Contains("jre") || re.Contains("jdk"))
                {
                    if (!re.EndsWith("javaw.exe") && !re.EndsWith("java.exe"))
                    {
                        if (!re.EndsWith("bin"))
                        {
                            re += "\\bin\\javaw.exe";
                        }
                        else
                        {
                            re += "\\javaw.exe";
                        }
                    }
                    else
                    {
                        return re;
                    }
                }
            }
            else
            {
                return null;
            }
            return re;
        }
    }
    public class JavaInfo
    {
        /// <summary>
        /// javaw.exe
        /// </summary>
        public string JavaPath { get; set; }
        public bool JavaBitType { get; set; }
    }
}
