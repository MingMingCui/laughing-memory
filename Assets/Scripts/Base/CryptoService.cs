using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class CryptoService
{
    /// <summary>
    /// 加密KEY 256位
    /// </summary>
    private static byte[] Key = { 0xCD, 0xEF, 0x01, 0xAB, 0x01, 0x89, 0xAB, 0xCD, 0x23, 0x45, 0x67, 0x67, 0xEF, 0x23, 0xAB, 0xCD,
                                        0xCD, 0xEF, 0x01, 0xAB, 0x01, 0x89, 0xAB, 0xCD , 0xCD, 0xEF, 0x01, 0xAB, 0x01, 0x89, 0xAB, 0xCD };
    /// <summary>
    /// 初始化向量 128位 
    /// tips:长度与BlockSize取模为0;
    /// </summary>
    private static byte[] IV = { 0x01, 0x23, 0x67, 0x89, 0x45, 0x67, 0x89, 0x89, 0x01, 0x23, 0x67, 0x89, 0x45, 0x67, 0x89, 0x89 };

    /// <summary>
    /// 加密Bundle尾部附加字节 用来确定是否已经加密
    /// tips:长度与BlockSize取模为0;
    /// </summary>
    private static byte[] cryptoEnd = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
    /// <summary>
    /// 对数组进行Aes加密
    /// </summary>
    /// <param name="values">要加密的数组</param>
    /// <returns>已加密的数组</returns>
    public static byte[] CreateEncryptByte(byte[] values)
    {
        if (values.Length > cryptoEnd.Length)
        {
            int count = 0;
            for (int i = 0; i < cryptoEnd.Length; ++i)
            {
                if (cryptoEnd[i] == values[values.Length - cryptoEnd.Length + i])
                    count++;
                else
                    break;
            }
            if (count == cryptoEnd.Length)
                return values;
        }

        byte[] encrypted;
        using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            aesAlg.Padding = PaddingMode.Zeros;
            aesAlg.BlockSize = 128;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(values, 0, values.Length);

                    csEncrypt.FlushFinalBlock();
                    msEncrypt.Seek(0, SeekOrigin.End);
                    msEncrypt.Write(cryptoEnd, 0, cryptoEnd.Length);
                }
                encrypted = msEncrypt.ToArray();

            }
        }
        return encrypted;
    }
    /// <summary>
    /// 对加密包进行解密
    /// </summary>
    /// <param name="values">已加密包</param>
    /// <returns>解密结果</returns>
    public static byte[] CreateDescryptByte(byte[] values)
    {
        byte[] fromEncrypt = new byte[values.Length - cryptoEnd.Length];
        using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            aesAlg.Padding = PaddingMode.Zeros;
            aesAlg.BlockSize = 128;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(values))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                }
            }
        }
        return fromEncrypt;
    }

}

