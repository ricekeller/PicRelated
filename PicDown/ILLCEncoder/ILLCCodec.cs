using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILLC.Encoder
{
    public class ILLCCodec
    {
		private const string ENCODERNAME="ILLC";
		private const string FILETYPE = "JPG";

		public byte[] Encode(object o)
		{
			return null;
		}
		public byte[] Encode(Image i)
		{
			using(MemoryStream ms=new MemoryStream())
			{
				i.Save(ms, ImageFormat.Png);
				byte[] he=GetHeader();
				ms.Write(he, 0, he.Length);
				return ms.ToArray();
			}
		}
		public byte[] Encode(string path)
		{
			using(FileStream fs=new FileStream(path,FileMode.Open))
			{
				byte[] h=GetHeader();
				int total=h.Length+(int)fs.Length;
				byte[] b=new byte[total];
				fs.Read(b, h.Length, (int)fs.Length);
				WriteHeader(b);
				return b;
			}
		}

		private void WriteHeader(byte[] b)
		{
			byte[] tmp = GetHeader();
			for(int i=0;i<tmp.Length;i++)
			{
				b[i] = tmp[i];
			}
		}
		private byte[] GetHeader()
		{
			return Encoding.UTF8.GetBytes(ENCODERNAME + FILETYPE);
		}

		public byte[] Decode(string path)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open))
			{
				byte[] h = GetHeader();
				int total = (int)fs.Length-h.Length;
				byte[] b = new byte[total];
				fs.Seek(h.Length, SeekOrigin.Begin);
				fs.Read(b, 0, total);
				return b;
			}
		}
    }
}
