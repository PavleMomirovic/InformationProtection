using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Cript
{
    class XTEA
    {
        private readonly uint rounds;
        private const uint delta = 0x9E3779B9;
        private bool PCBC = false;

        public XTEA(bool pCBC)
        {
            PCBC = pCBC;
        }

        public XTEA(uint rounds = 32)
        {
            this.rounds = rounds;
        }
        public string Encrypt(string src,string key)
        {
            byte[] srcBytes = EncryptBytes(Encoding.Unicode.GetBytes(src), key);

            string s2 = BitConverter.ToString(srcBytes);
            return s2;
        }
        public string Decrypt(string src, string key)
        {
            // Ovo ima specifican Save format, kako ne bi doslo do gubljenja podataka iz string u byte i obrnuto
            //dakle pored toga sto enkriptuje i dekriptuje, ima i poseban nacin prevodjenja byte[] u string u kojem bi se cuvao podatak.
            String[] tempAry = src.Split('-');
            byte[] decBytes2 = new byte[tempAry.Length];
            for (int i = 0; i < tempAry.Length; i++)
                decBytes2[i] = Convert.ToByte(tempAry[i], 16);

            byte[] srcBytes = DecryptBytes(decBytes2, key);
            string result = System.Text.Encoding.Unicode.GetString(srcBytes);
            return result;
        }

        public byte[] DecryptBytes(byte[] bytesToDecrypt, string decryptionKey)
        {
            byte[] decryptedBytes = (byte[])bytesToDecrypt.Clone();
            uint v0, v1;
            uint prevV0 = 0, prevV1 = 0;
            uint prevC0 = 0, prevC1 = 1;

            uint[] key = GenerateKey(decryptionKey);

            for (int j = 0; j < decryptedBytes.Length; j += 8)
            {
                v0 = BitConverter.ToUInt32(decryptedBytes, j);
                v1 = BitConverter.ToUInt32(decryptedBytes, j + 4);
                if (PCBC)
                {
                    prevC0 ^= prevV0;
                    prevC1 ^= prevV1;
                    v0 ^= prevC0;
                    v1 ^= prevC1;
                }
                uint sum = delta * rounds;
                for (uint i = 0; i < rounds; i++)
                {
                    v1 -= (((v0 << 4) ^ (v0 >> 5)) + v0) ^ (sum + key[(sum >> 11) & 3U]);
                    sum -= delta;
                    v0 -= (((v1 << 4) ^ (v1 >> 5)) + v1) ^ (sum + key[sum & 3U]);
                }
                Array.Copy(BitConverter.GetBytes(v0), 0, decryptedBytes, j, 4);
                Array.Copy(BitConverter.GetBytes(v1), 0, decryptedBytes, j + 4, 4);
                if (PCBC)
                {
                    prevC0 = v0;
                    prevC1 = v1;
                    prevV0 = BitConverter.ToUInt32(decryptedBytes, j);
                    prevV1 = BitConverter.ToUInt32(decryptedBytes, j + 4);
                }
            }

            return ByteStuffer.RemoveStuffedBytes(decryptedBytes, BitConverter.GetBytes((ushort)0xFFFF));
        }

        public byte[] EncryptBytes(byte[] bytesToEncrypt, string encryptionKey)
        {
            byte[] encryptedBytes = ByteStuffer.FillMissingBytes(bytesToEncrypt, 8, BitConverter.GetBytes((ushort)0xFFFF));
            uint[] key = GenerateKey(encryptionKey);
            uint v0, v1;
            uint prevV0=0, prevV1=0;
            uint prevC0=0, prevC1=1;

            for (int j = 0; j < encryptedBytes.Length; j += 8)
            {
                v0 = BitConverter.ToUInt32(encryptedBytes, j);
                v1 = BitConverter.ToUInt32(encryptedBytes, j + 4);

                if (PCBC)
                {
                    prevC0 ^= prevV0;   
                    prevC1 ^= prevV1;
                    v0 ^= prevC0; 
                    v1 ^= prevC1;
                }
                uint sum = 0;
                for (uint i = 0; i < rounds; i++)
                {
                    v0 += (((v1 << 4) ^ (v1 >> 5)) + v1) ^ (sum + key[sum & 3U]);
                    sum += delta;
                    v1 += (((v0 << 4) ^ (v0 >> 5)) + v0) ^ (sum + key[(sum >> 11) & 3U]);
                }
                Array.Copy(BitConverter.GetBytes(v0), 0, encryptedBytes, j, 4);
                Array.Copy(BitConverter.GetBytes(v1), 0, encryptedBytes, j + 4, 4);

                if (PCBC)
                {
                    prevC0 = v0; 
                    prevC1 = v1;
                    prevV0 = BitConverter.ToUInt32(encryptedBytes, j); 
                    prevV1 = BitConverter.ToUInt32(encryptedBytes, j + 4);
                }
            }

            return encryptedBytes;
        }



        private uint[] GenerateKey(string encryptionKey)
        {
            uint[] key = new uint[4];
            byte[] keyBytes = Encoding.Unicode.GetBytes(encryptionKey);

            for (int i = 0; i < 4; i++)
            {
                key[i] = BitConverter.ToUInt32(keyBytes, i * 4);
            }

            return key;
        }

    }
}