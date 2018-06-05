namespace xbuffer
{
    public static class TargetSystemBuffer
    {
        public static TargetSystem deserialize(byte[] buffer, ref uint offset)
        {

            // null
            bool _null = boolBuffer.deserialize(buffer, ref offset);
            if (_null) return null;

			// systemType

			int _systemType = intBuffer.deserialize(buffer, ref offset);



			// param1

			int _param1 = intBuffer.deserialize(buffer, ref offset);



			// param2

			int _param2 = intBuffer.deserialize(buffer, ref offset);



			// param3

			int _param3 = intBuffer.deserialize(buffer, ref offset);
            
            TargetSystem _TargetSystem = new TargetSystem();
			_TargetSystem.systemType = _systemType;

			_TargetSystem.param1 = _param1;

			_TargetSystem.param2 = _param2;

			_TargetSystem.param3 = _param3;
            return _TargetSystem;
        }

        public static void serialize(TargetSystem value, XSteam steam)
        {

            // null
            boolBuffer.serialize(value == null, steam);
            if (value == null) return;

			// systemType

			intBuffer.serialize(value.systemType, steam);



			// param1

			intBuffer.serialize(value.param1, steam);



			// param2

			intBuffer.serialize(value.param2, steam);



			// param3

			intBuffer.serialize(value.param3, steam);
        }
    }
}
