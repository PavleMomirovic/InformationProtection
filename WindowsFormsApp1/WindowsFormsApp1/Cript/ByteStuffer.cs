using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Cript
{
    class ByteStuffer
    {
        public static byte[] FillMissingBytes(byte[] bytesToFill, int moduleFactor, byte[] fillBytes)
        {
            int step = fillBytes.Length;
            byte[] filledBytes;

            if (bytesToFill.Length % moduleFactor == 0) filledBytes = (byte[])bytesToFill.Clone();
            else
            {
                var missingBytes = moduleFactor - bytesToFill.Length % moduleFactor;
                var existingBytes = bytesToFill.Length;
                filledBytes = new byte[existingBytes + missingBytes];
                Array.Copy(bytesToFill, filledBytes, existingBytes);

                while (missingBytes != 0)
                {
                    Array.Copy(filledBytes, 0, filledBytes, existingBytes, step);
                    missingBytes -= step;
                    existingBytes += step;
                }
            }
            return filledBytes;
        }

        public static byte[] RemoveStuffedBytes(byte[] bytesToClean, byte[] fillBytes)
        {
            return bytesToClean.Where(byteVal => !fillBytes.Contains(byteVal)).ToArray();
        }
    }
}
