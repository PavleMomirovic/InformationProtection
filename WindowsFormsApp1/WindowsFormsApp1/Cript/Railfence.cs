using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Cript
{
    class Railfence
    {
        private static char[][] InitializeMatrix(int n, int m)
        {
            char[][] matrix = new char[n][];
            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new char[m];
            }
            return matrix;
        }
        public string encrypt(string text, int key)
        {
            char[][] matrix = InitializeMatrix(key, text.Length);

            int j = 0;
            int k = 0;
            bool topOrBottom = true;

            for (int i = 0; i < text.Length; i++) 
            {                               
                if (j == 0 || j == key - 1)
                {
                    topOrBottom = !topOrBottom;
                }
                matrix[j][k++] = text[i];
                if (!topOrBottom)
                {
                    j++;
                }
                else
                {
                    j--;
                }
            }
            string encryptedtext = "";

            for (int i = 0; i < key; i++)
            {
                for (j = 0; j < text.Length; j++)
                {
                    if (matrix[i][j] != '\0')
                    {
                        encryptedtext += (matrix[i][j].ToString());
                    }
                }
            }

            return encryptedtext;
        }
        public string decrypt(string text, int key)
        {
            char[][] matrix = InitializeMatrix(key, text.Length);

            int k = 0;
            int j = 0;
            bool topOrBottom = true;
            for (int i = 0; i < text.Length; i++) // kao kod enkripcije ali umesto slova umetnuti znak
            {
                if (j == 0 || j == key - 1)
                {
                    topOrBottom = !topOrBottom;
                }
                matrix[j][k++] = '*';
                if (!topOrBottom)
                {
                    j++;
                }
                else
                {
                    j--;
                }
            }
            int c = 0;
            for (int a = 0; a < key; a++) // obilazimo matricu i gde nadjemo * stavimo tekst
            {
                for (int b = 0; b < text.Length; b++)
                {
                    if (matrix[a][b] == '*')
                    {
                        matrix[a][b] = text[c++];
                    }
                }
            }
            string decryptedText = "";
            topOrBottom = true;
            j = 0;
            for (int i = 0; i < text.Length; i++) // citamo tekst kao kod enkripcije
            {
                if (j == 0 || j == key - 1)
                {
                    topOrBottom = !topOrBottom;
                }
                decryptedText += (matrix[j][i].ToString());


                if (!topOrBottom)
                {
                    j++;
                }
                else
                {
                    j--;
                }
            }
            return decryptedText;
        }
    }
}
