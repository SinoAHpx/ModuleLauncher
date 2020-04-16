using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild.Tools
{
    public static class StringTools
    {
        public static string GetRandomString(int length)
        {
            List<String> vs = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            String result = String.Empty;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                result += vs[random.Next(vs.Count)];
            }
            return result;
        }
    }
}
