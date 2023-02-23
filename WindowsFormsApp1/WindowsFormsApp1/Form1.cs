using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Cript;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        String s1;
        bool encrypt = false;
        private string path= "C:/Users/pajap/Desktop/Files";

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            s1 = textBox1.Text;
            string v="We failed!";

            if (radioButton1.Checked)
            {
                encrypt = true;
            }
            else
            {
                encrypt = false;
            }
            

            switch (listBox1.SelectedIndex)
            {
                case (0):
                    string key0 = textBox3.Text;
                    A5_2 a52 = new A5_2(key0);
                    if (encrypt) v = a52.Encrypt(textBox1.Text);
                    else v = a52.Decrypt(textBox1.Text);
                    break;
                case (1):
                    int key1 = (int) numericUpDown1.Value;
                    Railfence rf=new Railfence();
                    if (encrypt) v = rf.encrypt(textBox1.Text, key1);
                    else v = rf.decrypt(textBox1.Text,key1);
                    break;
                case (2):
                    XTEA xtea = new XTEA(false);
                    string key2 = textBox4.Text;
                    if (encrypt) v = xtea.Encrypt(textBox1.Text, key2);
                    else v = xtea.Decrypt(textBox1.Text, key2);
                    break;
                case (3):
                    XTEA xteaPcbc = new XTEA(true);
                    string key3 = textBox4.Text;
                    if (encrypt) v = xteaPcbc.Encrypt(textBox1.Text, key3);
                    else v = xteaPcbc.Decrypt(textBox1.Text, key3);
                    break;
                default:
                    MessageBox.Show("Nije odabran ni jedan algoritam!");
                    break;
            }
            textBox2.Text = v;

            //poziv funkcije koji vraca kriptovan string
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(path))
            {
                ofd1.InitialDirectory = path;
            }
            if (ofd1.ShowDialog()==DialogResult.OK)
            {
                using(StreamReader sr= new StreamReader(ofd1.FileName))
                {
                    textBox1.Text = sr.ReadToEnd();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(Directory.Exists(path))
            {
                sfd1.InitialDirectory = path;
            }
            if(sfd1.ShowDialog()==DialogResult.OK)
            {
                using(StreamWriter sw= new StreamWriter(sfd1.FileName))
                {
                    sw.Write(textBox2.Text);
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(path))
            {
                ofd1.InitialDirectory = path;
            }
            ofd1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";

            if (ofd1.ShowDialog() == DialogResult.OK)
            {
                Bitmap image = new Bitmap(ofd1.FileName);

                int width = image.Width;
                int height = image.Height;

                byte[] imageData = new byte[width * height * 3];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixelColor = image.GetPixel(x, y);

                        int index = (y * width + x) * 3;
                        imageData[index] = pixelColor.R;
                        imageData[index + 1] = pixelColor.G;
                        imageData[index + 2] = pixelColor.B;
                    }
                }

                string key0 = textBox3.Text;
                A5_2 a52Slika = new A5_2(key0);
                byte[] cryptedImageData = a52Slika.CryptForJpg(imageData);



                Bitmap newImage = new Bitmap(width, height);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = (y * width + x) * 3;
                        Color pixelColor = Color.FromArgb(cryptedImageData[index], cryptedImageData[index + 1], cryptedImageData[index + 2]);

                        newImage.SetPixel(x, y, pixelColor);
                    }
                }

                newImage.Save("C:/Users/pajap/Desktop/Files/newImage.jpg");
            }
           

        }
    }
}
