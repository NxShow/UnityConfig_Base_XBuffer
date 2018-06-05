namespace xbuffer
{
    public static class IdPairBuffer
    {
        public static IdPair deserialize(byte[] buffer, ref uint offset)
        {

            // null
            bool _null = boolBuffer.deserialize(buffer, ref offset);
            if (_null) return null;

			// id

			int _id = intBuffer.deserialize(buffer, ref offset);



			// value

			int _value = intBuffer.deserialize(buffer, ref offset);
            
            IdPair _IdPair = new IdPair();
			_IdPair.id = _id;

			_IdPair.value = _value;
            return _IdPair;
        }

        public static void serialize(IdPair value, XSteam steam)
        {

            // null
            boolBuffer.serialize(value == null, steam);
            if (value == null) return;

			// id

			intBuffer.serialize(value.id, steam);



			// value

			intBuffer.serialize(value.value, steam);
        }
    }
}
