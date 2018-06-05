namespace xbuffer
{
    public static class RewardItemBuffer
    {
        public static RewardItem deserialize(byte[] buffer, ref uint offset)
        {

            // null
            bool _null = boolBuffer.deserialize(buffer, ref offset);
            if (_null) return null;

			// itemId

			int _itemId = intBuffer.deserialize(buffer, ref offset);



			// itemCount

			int _itemCount = intBuffer.deserialize(buffer, ref offset);
            
            RewardItem _RewardItem = new RewardItem();
			_RewardItem.itemId = _itemId;

			_RewardItem.itemCount = _itemCount;
            return _RewardItem;
        }

        public static void serialize(RewardItem value, XSteam steam)
        {

            // null
            boolBuffer.serialize(value == null, steam);
            if (value == null) return;

			// itemId

			intBuffer.serialize(value.itemId, steam);



			// itemCount

			intBuffer.serialize(value.itemCount, steam);
        }
    }
}
