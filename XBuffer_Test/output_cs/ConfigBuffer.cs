namespace xbuffer
{
    public static class ConfigBuffer
    {
        public static Config deserialize(byte[] buffer, ref uint offset)
        {

            // null
            bool _null = boolBuffer.deserialize(buffer, ref offset);
            if (_null) return null;

			// AchievementConfig

			AchievementConfigs _AchievementConfig = AchievementConfigsBuffer.deserialize(buffer, ref offset);
            _AchievementConfig.OnAfterDeserialize();

            Config _Config = new Config();
				_Config.AchievementConfig = _AchievementConfig;
            return _Config;
        }

        public static void serialize(Config value, XSteam steam)
        {

            // null
            boolBuffer.serialize(value == null, steam);
            if (value == null) return;

			// AchievementConfig

			AchievementConfigsBuffer.serialize(value.AchievementConfig, steam);
        }
    }
}
