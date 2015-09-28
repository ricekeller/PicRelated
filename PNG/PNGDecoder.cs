using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNG
{
    public class PNGDecoder
    {
		private static byte[] s_beginning = new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };
		private Stream _stream;
		public PNGDecoder(Stream fileStream)
		{
			this._stream = fileStream;
		}
		public bool IsPNG()
		{
			_stream.Seek(0,SeekOrigin.Begin);
			for(int i=0;i<8;i++)
			{
				if(_stream.ReadByte()!=s_beginning[i])
				{
					return false;
				}
			}
			return true;
		}

		private void ProcessChunk()
		{
			//Each chunk is in either form: LENGTH,CHUNK TYPE,CHUNK DATA,CRC 
			//							or  LENGTH(=0),CHUNK TYPE,CRC
			//Length:A four-byte unsigned integer giving the number of bytes in the chunk's data field. 
			//		The length counts only the data field, not itself, the chunk type, or the CRC. 
			//		Zero is a valid length. Although encoders and decoders should treat the length as unsigned, 
			//		its value shall not exceed 231-1 bytes.
			//Chunk Type:A sequence of four bytes defining the chunk type. Each byte of a chunk type is restricted 
			//			to the decimal values 65 to 90 and 97 to 122. These correspond to the uppercase and lowercase 
			//			ISO 646 letters (A-Z and a-z) respectively for convenience in description and examination of 
			//			PNG datastreams. Encoders and decoders shall treat the chunk types as fixed binary values, 
			//			not character strings. For example, it would not be correct to represent the chunk type IDAT 
			//			by the equivalents of those letters in the UCS 2 character set.
			//Chunk Data:The data bytes appropriate to the chunk type, if any. This field can be of zero length.
			//CRC:A four-byte CRC (Cyclic Redundancy Code) calculated on the preceding bytes in the chunk, including 
			//		the chunk type field and chunk data fields, but not including the length field. The CRC can be used
			//		to check for corruption of the data. The CRC is always present, even for chunks containing no data.
		}
    }
}
