namespace xbuffer
{
    public static class AchievementConfigBuffer
    {
        public static AchievementConfig deserialize(byte[] buffer, ref uint offset)
        {

            // null
            bool _null = boolBuffer.deserialize(buffer, ref offset);
            if (_null) return null;

			// Id

			int _Id = intBuffer.deserialize(buffer, ref offset);



			// Name

			int _Name = intBuffer.deserialize(buffer, ref offset);



			// Desc

			int _Desc = intBuffer.deserialize(buffer, ref offset);



			// Icon

			int _Icon = intBuffer.deserialize(buffer, ref offset);



			// Difficulty

			int _Difficulty = intBuffer.deserialize(buffer, ref offset);



			// Condition

			AchievementCondition _Condition = AchievementConditionBuffer.deserialize(buffer, ref offset);



			// RewardItem


			int _RewardItem_length = intBuffer.deserialize(buffer, ref offset);
            RewardItem[] _RewardItem = new RewardItem[_RewardItem_length];
            for (int i = 0; i < _RewardItem_length; i++)
            {
                _RewardItem[i] = RewardItemBuffer.deserialize(buffer, ref offset);
            }


			// Buff

			IdPair _Buff = IdPairBuffer.deserialize(buffer, ref offset);



			// NeedAchievement

			int _NeedAchievement = intBuffer.deserialize(buffer, ref offset);



			// TargetSystem

			TargetSystem _TargetSystem = TargetSystemBuffer.deserialize(buffer, ref offset);



			// Group

			int _Group = intBuffer.deserialize(buffer, ref offset);
            
            AchievementConfig _AchievementConfig = new AchievementConfig();
			_AchievementConfig.Id = _Id;

			_AchievementConfig.Name = _Name;

			_AchievementConfig.Desc = _Desc;

			_AchievementConfig.Icon = _Icon;

			_AchievementConfig.Difficulty = _Difficulty;

			_AchievementConfig.Condition = _Condition;

			_AchievementConfig.RewardItem = _RewardItem;

			_AchievementConfig.Buff = _Buff;

			_AchievementConfig.NeedAchievement = _NeedAchievement;

			_AchievementConfig.TargetSystem = _TargetSystem;

			_AchievementConfig.Group = _Group;
            return _AchievementConfig;
        }

        public static void serialize(AchievementConfig value, XSteam steam)
        {

            // null
            boolBuffer.serialize(value == null, steam);
            if (value == null) return;

			// Id

			intBuffer.serialize(value.Id, steam);



			// Name

			intBuffer.serialize(value.Name, steam);



			// Desc

			intBuffer.serialize(value.Desc, steam);



			// Icon

			intBuffer.serialize(value.Icon, steam);



			// Difficulty

			intBuffer.serialize(value.Difficulty, steam);



			// Condition

			AchievementConditionBuffer.serialize(value.Condition, steam);



			// RewardItem


            intBuffer.serialize(value.RewardItem.Length, steam);
            for (int i = 0; i < value.RewardItem.Length; i++)
            {
                RewardItemBuffer.serialize(value.RewardItem[i], steam);
            }


			// Buff

			IdPairBuffer.serialize(value.Buff, steam);



			// NeedAchievement

			intBuffer.serialize(value.NeedAchievement, steam);



			// TargetSystem

			TargetSystemBuffer.serialize(value.TargetSystem, steam);



			// Group

			intBuffer.serialize(value.Group, steam);
        }
    }
}
