﻿/*
 * File Name:               intBuffer.cs
 *
 * Description:             基本类型处理
 * Author:                  lisiyu <576603306@qq.com>
 * Create Date:             2017/10/25
 */

using System;

namespace xbuffer
{
    public class intBuffer
    {
        private static readonly uint size = sizeof(int);
        //private static readonly int defaultValue = 0;

        public unsafe static int deserialize(byte[] buffer, ref uint offset)
        {
            //bool isDefaultValue = boolBuffer.deserialize(buffer, ref offset);
            //if (isDefaultValue)
            //{
            //    return defaultValue;
            //}
            //else
            {
                fixed (byte* ptr = buffer)
                {
                    var value = *(int*)(ptr + offset);
                    offset += size;
                    return BitConverter.IsLittleEndian ? value : (int)utils.toLittleEndian((uint)value);
                }
            }
        }

        public unsafe static void serialize(int value, XSteam steam)
        {
            //boolBuffer.serialize(value == defaultValue, steam);
            //if (value != defaultValue)
            {
                steam.applySize(size);
                fixed (byte* ptr = steam.contents[steam.index_group])
                {
                    *(int*)(ptr + steam.index_cell) = BitConverter.IsLittleEndian ? value : (int)utils.toLittleEndian((uint)value);
                    steam.index_cell += size;
                }
            }
        }
    }
}