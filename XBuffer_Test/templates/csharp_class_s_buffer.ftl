namespace xbuffer
{
    public static class #CLASS_NAME#sBuffer
    {
        public static #CLASS_NAME#s deserialize(byte[] buffer, ref uint offset)
        {
            int _#CLASS_NAME#_length = intBuffer.deserialize(buffer, ref offset);
            #CLASS_NAME#[] _#CLASS_NAME# = new #CLASS_NAME#[_#CLASS_NAME#_length];
            for (int i = 0; i < _#CLASS_NAME#_length; i++)
            {
                _#CLASS_NAME#[i] = #CLASS_NAME#Buffer.deserialize(buffer, ref offset);
            }

            #CLASS_NAME#s _#CLASS_NAME#s = new #CLASS_NAME#s();
            _#CLASS_NAME#s._configList = _#CLASS_NAME#;
            return _#CLASS_NAME#s;
        }

        public static void serialize(#CLASS_NAME#s value, XSteam steam)
        {
            intBuffer.serialize(value.ConfigList.Length, steam);
            for (int i = 0; i < value.ConfigList.Length; i++)
            {
                #CLASS_NAME#Buffer.serialize(value.ConfigList[i], steam);
            }
        }
    }
}
