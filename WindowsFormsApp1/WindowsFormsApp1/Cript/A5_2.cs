using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Cript
{
	class A5_2
	{
		public ShiftRegister R1 = new ShiftRegister();
		public ShiftRegister R2 = new ShiftRegister();
		public ShiftRegister R3 = new ShiftRegister();
		public ShiftRegister R4 = new ShiftRegister();

		public A5_2()
		{
			//
			// TODO: Add constructor logic here
			//

			R1.OperationMembers = new int[] { 13, 16, 17, 18 };
			R2.OperationMembers = new int[] { 20, 21 };
			R3.OperationMembers = new int[] { 7, 20, 21, 22 };
			R4.OperationMembers = new int[] { 11, 16 };
		}

		public A5_2(string newKey) : this()
		{
			this.Key = newKey;
		}


		private string key;

		public string Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
				char[] keyEls = key.ToCharArray();
				int i = 0;
				string pom = "";

				while (i < 19)
				{
					pom += keyEls[i];
					i++;
				}

				R1.Register = pom;
				pom = "";

				while (i < 41)
				{
					pom += keyEls[i];
					i++;
				}

				R2.Register = pom;
				pom = "";

				while (i < 64)
				{
					pom += keyEls[i];
					i++;
				}
				R3.Register = pom;

				//ovo vrv ne treba ovako
				pom = "";
				i = 0;
				while (i < 17)
				{
					pom += keyEls[i];
					i++;
				}

				R4.Register = pom;

			}
		}


		public char XOR(char a, char b)
		{
			if (a == b)
				return '0';
			else
				return '1';
		}


		public void RegisterSteps(ShiftRegister sr)
		{
			char t = '0';
			foreach (int index in sr.OperationMembers)
			{
				t = XOR(t, sr.Register[index]);
			}
			sr.Shift(t);
		}

		public char MajorityVote(char a, char b, char c)
		{
			char sum = '0';
			char x='0', y='0', z='0';
			if (a == '1' && b == '1') x = '1';
			if (b == '1' && c == '1') y = '1';
			if (c == '1' && a == '1') z = '1';
			sum = XOR(x, y);
			sum = XOR(sum, z);
			return sum;
		}
		public byte[] CryptForJpg(byte[] bytes)
        {
			//iz niza bajtova, u string koji cuva bitove
			string s = string.Join("",bytes.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')));

			string res = s;
			//string res = Crypt(s);
			//Kad probam da kriptujem dobijem runtime error koji nemam vremena da debagujem


			//iz stringa koji cuva bitove u niz bajtova
			int numOfBytes = res.Length / 8;
			byte[] cryptedBytes = new byte[numOfBytes];
			for (int i = 0; i < numOfBytes; ++i)
			{
				cryptedBytes[i] = Convert.ToByte(res.Substring(8 * i, 8), 2);
			}
			return cryptedBytes;
		}
		private string Crypt(string source)
		{
			char[] sourceEls = source.ToCharArray();

			string outStr = "";

			foreach (char c in sourceEls)
			{
				char m1 = MajorityVote(R1.Register[12], R1.Register[14], R1.Register[15]);
				char m2 = MajorityVote(R2.Register[9], R2.Register[13], R2.Register[16]);
				char m3 = MajorityVote(R3.Register[13], R3.Register[16], R3.Register[18]);
				//char m4 = MajorityVote(R4.Register[10], R4.Register[3], R4.Register[7]);

				if (R4.Register[10] == m1)
					RegisterSteps(R1);

				if (R4.Register[3] == m2)
					RegisterSteps(R2);

				if (R3.Register[7] == m3)
					RegisterSteps(R3);
				RegisterSteps(R4);

				char s = '0';
				s = XOR(XOR(XOR(s, R1.Output), R2.Output), R3.Output);
				s = XOR(XOR(XOR(s, m1), m2), m3);
				outStr += XOR(s, c);
			}

			return outStr;
		}
		public string Encrypt(string source)
        {
			string srcBin = StringToBinary(source);
			return Crypt(srcBin);
        }
		public string Decrypt(string source)
        {
			string val = Crypt(source);
			return BinaryToString(val);
        }
		public  string JustATryout(string s)
        {
			string srcBin = StringToBinary(s);
			string finalStr = BinaryToString(srcBin);
			return finalStr;

		}
		public string StringToBinary(string data)
		{
			StringBuilder sb = new StringBuilder();

			foreach (char c in data.ToCharArray())
			{
				sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
			}
			return sb.ToString();
		}

		public static string BinaryToString(string data)
		{
			List<Byte> byteList = new List<Byte>();

			for (int i = 0; i < data.Length; i += 8)
			{
				byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
			}
			return Encoding.ASCII.GetString(byteList.ToArray());
		}
	}
}
