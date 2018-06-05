﻿/**************************************************************
 *  Filename:    CompressTool.cs
 *  Copyright:   Microsoft Co., Ltd.
 *
 *  Description: CompressTool ClassFile.
 *
 *  @author:     xiaobai
 *  @version     2018/6/2 16:49:38  @Reviser  Initial Version
 **************************************************************/

using System.Collections;
using System.IO;
using System.IO.Compression;

public static class CompressTool
{
    //压缩字节
    //1.创建压缩的数据流 
    //2.设定compressStream为存放被压缩的文件流,并设定为压缩模式
    //3.将需要压缩的字节写到被压缩的文件流
    public static byte[] CompressBytes(byte[] bytes)
    {
        using (MemoryStream compressStream = new MemoryStream())
        {
            using (var zipStream = new GZipStream(compressStream, CompressionMode.Compress))
                zipStream.Write(bytes, 0, bytes.Length);
            return compressStream.ToArray();
        }
    }
    //解压缩字节
    //1.创建被压缩的数据流
    //2.创建zipStream对象，并传入解压的文件流
    //3.创建目标流
    //4.zipStream拷贝到目标流
    //5.返回目标流输出字节
    public static byte[] Decompress(byte[] bytes)
    {
        using (MemoryStream srcMs = new MemoryStream(bytes))
        {
            using (GZipStream zipStream = new GZipStream(srcMs, CompressionMode.Decompress))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] result = new byte[40960];
                    int n;
                    while ((n = zipStream.Read(result, 0, result.Length)) > 0)
                    {
                        ms.Write(result, 0, n);
                    }
                    return ms.ToArray();
                }
            }
        }
    }
}
