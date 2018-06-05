namespace xbuffer
{
    public static class AchievementConfigsBuffer
    {
        public static AchievementConfigs deserialize(byte[] buffer, ref uint offset)
        {
            int _AchievementConfig_length = intBuffer.deserialize(buffer, ref offset);
            AchievementConfig[] _AchievementConfig = new AchievementConfig[_AchievementConfig_length];
            for (int i = 0; i < _AchievementConfig_length; i++)
            {
                _AchievementConfig[i] = AchievementConfigBuffer.deserialize(buffer, ref offset);
            }

            AchievementConfigs _AchievementConfigs = new AchievementConfigs();
            _AchievementConfigs._configList = _AchievementConfig;
            return _AchievementConfigs;
        }

        public static void serialize(AchievementConfigs value, XSteam steam)
        {
            intBuffer.serialize(value.ConfigList.Length, steam);
            for (int i = 0; i < value.ConfigList.Length; i++)
            {
                AchievementConfigBuffer.serialize(value.ConfigList[i], steam);
            }
        }
    }
}
