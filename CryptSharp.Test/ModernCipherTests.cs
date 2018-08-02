﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptSharp.Ciphers.Modern;

namespace CryptSharp.Test
{
    [TestClass]
    public class ModernCipherTests
    {
        [TestMethod]
        public void Modern_DESTest()
        {
            byte[] key = BitConverter.GetBytes(0x133457799BBCDFF1);
            byte[] clear = BitConverter.GetBytes(0x0123456789ABCDEF);

            DES d = new DES();
            d.Mode = Mode.ElectronicCodeBook;
            d.Key = key;
            d.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] cipherText = d.Encrypt(clear);

            ulong output = BitConverter.ToUInt64(cipherText, 0);

            Assert.AreEqual(0x85E813540F0AB405, output);

            output = BitConverter.ToUInt64(d.Decrypt(cipherText), 0);
            Assert.AreEqual((ulong)0x0123456789ABCDEF, output);

            d.Mode = Mode.ChainBlockCoding;
            d.Key = key;
            d.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            cipherText = d.Encrypt(clear);

            output = BitConverter.ToUInt64(cipherText, 0);

            Assert.AreEqual(0x85E813540F0AB405, output);

            d.Mode = Mode.ChainBlockCoding;
            d.Key = key;
            d.IV = clear;
            cipherText = d.Encrypt(clear);

            output = BitConverter.ToUInt64(cipherText, 0);

            Assert.AreEqual(0x948A43F98A834F7E, output);

            output = BitConverter.ToUInt64(d.Decrypt(cipherText), 0);
            Assert.AreEqual((ulong)0x0123456789ABCDEF, output);
        }

        [TestMethod]
        public void Modern_DESTest_Kryptos()
        {
            byte[] key = new byte[8];

            ulong baseKey = (ulong)'S' | (ulong)'O' << 8 | (ulong)'T' << 16 | (ulong)'P' << 24 | (ulong)'Y' << 32 | (ulong)'R' << 40 | (ulong)'K' << 48;
            //ulong baseKey = (ulong)'K' | (ulong)'R' << 8 | (ulong)'Y' << 16 | (ulong)'P' << 24 | (ulong)'T' << 32 | (ulong)'O' << 40 | (ulong)'S' << 48;
            ulong expandedKey = baseKey & 0x7F;
            expandedKey |= ((baseKey >> 7) & 0x7F) << 8;
            expandedKey |= ((baseKey >> 14) & 0x7F) << 16;
            expandedKey |= ((baseKey >> 21) & 0x7F) << 24;
            expandedKey |= ((baseKey >> 28) & 0x7F) << 32;
            expandedKey |= ((baseKey >> 35) & 0x7F) << 40;
            expandedKey |= ((baseKey >> 42) & 0x7F) << 48;
            expandedKey |= ((baseKey >> 49) & 0x7F) << 56;

            System.Security.Cryptography.DES des = System.Security.Cryptography.DES.Create();

            des.Key = BitConverter.GetBytes(expandedKey);
            des.Mode = System.Security.Cryptography.CipherMode.ECB;

            System.Security.Cryptography.ICryptoTransform xform = des.CreateDecryptor();

            byte[] data = new byte[8];
            data[0] = (byte)'O';
            data[1] = (byte)'K';
            data[2] = (byte)'R';
            data[3] = (byte)'U';
            data[4] = (byte)'O';
            data[5] = (byte)'X';
            data[6] = (byte)'O';

            byte[] output = new byte[8];
            xform.TransformBlock(data, 0, 8, output, 0);

            for (int i = 0; i < 8; i++)
            {
                output[i] = (byte)(output[i] % 26);
            }


        }

        [TestMethod]
        public void Modern_StreamTest_LFSR()
        {
            Stream str = new Stream(Utility.EnglishAlphabetAsStrings());

            str.Registers = new Components.LinearFeedbackShiftRegister(0xACE1, 16);//key is effectively 0xACE1
            string[] cipher = str.Encrypt("A B C D E F".Split(' '));

            CollectionAssert.AreNotEqual("A B C D E F".Split(' '), cipher);

            str.Registers = new Components.LinearFeedbackShiftRegister(0xACE1, 16);//key is effectively 0xACE1
            string[] clear = str.Decrypt(cipher);

            CollectionAssert.AreEqual("A B C D E F".Split(' '), clear);
        }

        [TestMethod]
        public void Modern_StreamTest_Key()
        {
            Stream str = new Stream(Utility.EnglishAlphabetAsStrings());
            str.Key = new byte[] { 0xC1, 0xF2, 0x03, 0xA4, 0x05, 0x06, 0xB7, 0x08 };

            string[] cipher = str.Encrypt("A B C D E F".Split(' '));

            CollectionAssert.AreNotEqual("A B C D E F".Split(' '), cipher);

            string[] clear = str.Encrypt(cipher);

            CollectionAssert.AreEqual("A B C D E F".Split(' '), clear);
        }

        [TestMethod]
        public void Modern_Trivium()
        {
            byte[] Key = new byte[10];
            byte[] IV = new byte[10];
            for(int i=0; i<10; i++)
            {
                Key[i] = 0xEF;
                IV[i] = 0xEF;
            }
            Trivium tri = new Trivium(Key, IV);

            byte[] cipher = tri.Encrypt(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

            CollectionAssert.AreNotEqual(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, cipher);

            tri.ResetState();
            byte[] clear = tri.Decrypt(cipher);

            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, clear);
        }
    }
}