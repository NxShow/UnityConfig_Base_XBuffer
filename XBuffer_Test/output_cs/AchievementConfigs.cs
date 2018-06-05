using System;
using System.Collections.Generic;
namespace xbuffer
{
    // Auto Created AchievementConfig
    public partial class AchievementConfigs
    {
        public AchievementConfig[] _configList;
        Dictionary<int, AchievementConfig> _configs = new Dictionary<int, AchievementConfig>();

        public Dictionary<int, AchievementConfig> Configs
        {
            get { return _configs; }
        }
        
        public AchievementConfig[] ConfigList
        {
            get { return _configList; }
        }
        
        public void OnAfterDeserialize()
        {
            for (int i = 0; i < _configList.Length; ++i)
            {
                AchievementConfig config = _configList[i];
                _configs.Add(config.Id, config);
            }
        }
        
        public AchievementConfig GetConfigById(int id)
        {
            AchievementConfig config = null;
            _configs.TryGetValue(id, out config);
            if (config == null)
            {
                Console.WriteLine("Failed to find AchievementConfig config for: " + id);
            }
            return config;
        }

        public AchievementConfig FindConfigById(int id)
        {
            AchievementConfig config = null;
            _configs.TryGetValue(id, out config);
            return config;
        }
    }
}