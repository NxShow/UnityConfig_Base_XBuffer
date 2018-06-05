using System.Collections.Generic;
using System.IO;

namespace CommonLib
{
    public class Config
    {
        public static Dictionary<string, string> _configs = new Dictionary<string, string>();

        public static string Get(string key)
        {
            return _configs[key];
        }

        public static bool Load()
        {
            string path_1 = System.Environment.CurrentDirectory;
            int a_1 = path_1.IndexOf("GameConfig");
            string path_2 = path_1.Substring(0, a_1);

            StreamReader sr = new StreamReader(path_1 + "./config.txt");
            if (sr == null) return false;
            do
            {
                var line = sr.ReadLine();
                if (line == null) break;

                var strs = line.Split('=');
                if (strs.Length < 2)
                    continue;

                var key = strs[0];
                var value = strs[1];

                _configs.Add(key, value);

            } while (true);

            return true;
        }
    }
}
