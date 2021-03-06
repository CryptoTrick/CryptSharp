﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class CipherBase<T>
    {
        public CipherBase(T[] Alphabet)
        {
            alphabet = Alphabet;
        }

        protected T[] alphabet;

        public bool IsInAlphabet(T c)
        {
            return alphabet.Contains(c);
        }

        public string GenerateRandomString(int length = 0)
        {
            StringBuilder toEncrypt = new StringBuilder();

            T[] generated = Utility.Random<T>(alphabet, 1024);
            int i = length;

            foreach (T c in generated)
            {
                if (IsInAlphabet(c))
                {
                    toEncrypt.Append(c);
                    i--;
                }
                if (length != 0 && i == 0)
                {
                    break;
                }
            }

            return toEncrypt.ToString();
        }

        public string[] GenerateRandomLetters(int length = 0)
        {
            List<T> toEncrypt = new List<T>();

            T[] generated = Utility.Random<T>(alphabet, 1024);
            int i = length;

            foreach (T c in generated)
            {
                if (IsInAlphabet(c))
                {
                    toEncrypt.Add(c);
                    i--;
                }
                if (length != 0 && i == 0)
                {
                    break;
                }
            }

            return toEncrypt.ToArray() as string[];
        }

        public T[] ScrambledAlphabet()
        {
            List<T> copy = new List<T>(alphabet);

            List<T> sb = new List<T>();
            byte[] tokenData = new byte[2];
            using (System.Security.Cryptography.RandomNumberGenerator rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                for (int i = 0; i < alphabet.Length; i++)
                {
                    rng.GetBytes(tokenData);

                    int value = (int)(BitConverter.ToUInt16(tokenData, 0) % copy.Count);

                    sb.Add(copy[value]);

                    copy.RemoveAt(value);
                }
            }
            return sb.ToArray();
        }
    }
}
