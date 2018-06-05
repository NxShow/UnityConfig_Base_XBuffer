namespace xbuffer
{
    public static class AchievementConditionBuffer
    {
        public static AchievementCondition deserialize(byte[] buffer, ref uint offset)
        {

            // null
            bool _null = boolBuffer.deserialize(buffer, ref offset);
            if (_null) return null;

			// type

			int _type = intBuffer.deserialize(buffer, ref offset);



			// count

			int _count = intBuffer.deserialize(buffer, ref offset);



			// param

			int _param = intBuffer.deserialize(buffer, ref offset);



			// compareType

			int _compareType = intBuffer.deserialize(buffer, ref offset);
            
            AchievementCondition _AchievementCondition = new AchievementCondition();
			_AchievementCondition.type = _type;

			_AchievementCondition.count = _count;

			_AchievementCondition.param = _param;

			_AchievementCondition.compareType = _compareType;
            return _AchievementCondition;
        }

        public static void serialize(AchievementCondition value, XSteam steam)
        {

            // null
            boolBuffer.serialize(value == null, steam);
            if (value == null) return;

			// type

			intBuffer.serialize(value.type, steam);



			// count

			intBuffer.serialize(value.count, steam);



			// param

			intBuffer.serialize(value.param, steam);



			// compareType

			intBuffer.serialize(value.compareType, steam);
        }
    }
}
