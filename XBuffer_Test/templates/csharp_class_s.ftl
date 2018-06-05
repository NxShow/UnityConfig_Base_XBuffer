using System;
using System.Collections.Generic;
namespace xbuffer
{
    // #CLASS_COMMENT#
    public partial #CLASS_TYPE# #CLASS_NAME#s
    {
        public #CLASS_NAME#[] _configList;
        Dictionary<#KEY_TYPE#, #CLASS_NAME#> _configs = new Dictionary<#KEY_TYPE#, #CLASS_NAME#>();

        public Dictionary<#KEY_TYPE#, #CLASS_NAME#> Configs
        {
            get { return _configs; }
        }
        
        public #CLASS_NAME#[] ConfigList
        {
            get { return _configList; }
        }
        
        public void OnAfterDeserialize()
        {
            for (int i = 0; i < _configList.Length; ++i)
            {
                #CLASS_NAME# config = _configList[i];
                _configs.Add(config.#KEY_NAME#, config);
            }
        }
        
        public #CLASS_NAME# GetConfigById(#KEY_TYPE# id)
        {
            #CLASS_NAME# config = null;
            _configs.TryGetValue(id, out config);
            if (config == null)
            {
                Console.WriteLine("Failed to find #CLASS_NAME# config for: " + id);
            }
            return config;
        }

        public #CLASS_NAME# FindConfigById(#KEY_TYPE# id)
        {
            #CLASS_NAME# config = null;
            _configs.TryGetValue(id, out config);
            return config;
        }
    }
}