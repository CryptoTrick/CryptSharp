﻿using CryptSharp.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Modern
{
    public class Stream : CipherBase<string>
    {
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();

        public Stream(string[] Alphabet) : base(Alphabet)
        {
            alphabet = Alphabet;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }

        public byte[] Key { get; set; }
        public LinearFeedbackShiftRegister Registers { get; set; }

        public string[] Encrypt(string[] clearText)
        {
            List<string> cipher = new List<string>();
            int bit = 1;
            int k = 0;
            foreach (string s in clearText)
            {
                byte[] clear = UnicodeEncoding.Unicode.GetBytes(s);

                for (int i = 0; i < clear.Length; i++)
                {
                    if (Registers != null)
                    {
                        //it's easier to block up the stream in 8 bit chunks

                        bit = 0;
                        for(int j=0; j<8; j++)
                            bit |= Registers.Shift(new byte[] { 11, 9, 1 }) << j;

                        clear[i] = (byte)(clear[i] ^ bit);

                        //clear[i] = (byte)(clear[i] ^ Key[((k++) % Key.Length)]);
                    }
                    else
                    {
                        clear[i] = (byte)(clear[i] ^ Key[((k++) % Key.Length)]);
                    }
                }

                cipher.Add(UnicodeEncoding.Unicode.GetString(clear));

            }

            return cipher.ToArray();
        }
        public string[] Encrypt(string clearText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public string[] Decrypt(string[] cipherText)
        {
            List<string> clearText = new List<string>();
            int bit = 1;
            int k = 0;
            foreach (string s in cipherText)
            {
                byte[] clear = UnicodeEncoding.Unicode.GetBytes(s);

                for (int i = 0; i < clear.Length; i++)
                {
                    if (Registers != null)
                    {
                        //it's easier to block up the stream in 8 bit chunks

                        bit = 0;
                        for (int j = 0; j < 8; j++)
                            bit |= Registers.Shift(new byte[] { 11, 9, 1 }) << j;

                        clear[i] = (byte)(clear[i] ^ bit);

                        //clear[i] = (byte)(clear[i] ^ Key[((k++) % Key.Length)]);
                    }
                    else
                    {
                        clear[i] = (byte)(clear[i] ^ Key[((k++) % Key.Length)]);
                    }
                }

                clearText.Add(UnicodeEncoding.Unicode.GetString(clear));

            }

            return clearText.ToArray();
        }
        public string[] Decrypt(string cipherText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }
        public void DecryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }
    }
}
