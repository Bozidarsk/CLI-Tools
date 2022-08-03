using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Color 
{
	public int r { set; get; }
	public int g { set; get; }
	public int b { set; get; }
	public int a { set; get; }

	public Color() {}
	public Color(int r, int g, int b, int a) 
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}
}

public class Chunk 
{
	public int Length { set; get; }
	public byte[] Type { set; get; }
	public byte[] Data { set; get; }
	public byte[] Crc { set; get; }

	public void Print() 
	{
		Console.WriteLine("Chunk:");
		Console.WriteLine("  Length: " + System.Convert.ToString(this.Length));
		Console.WriteLine("  Type: " + System.Text.Encoding.UTF8.GetString(this.Type));

		Console.Write("  Data: { ");
		for (int i = 0; i < this.Length; i++) { string c = main.DecimalToHex((int)this.Data[i]); Console.Write(((c.Length == 1) ? "0" + c : c) + ((i >= this.Length - 1) ? " " : ", ")); }
		Console.WriteLine("}");

		Console.Write("  Crc: { ");
		for (int i = 0; i < this.Crc.Length; i++) { string c = main.DecimalToHex((int)this.Crc[i]); Console.Write(((c.Length == 1) ? "0" + c : c) + ((i >= 4 - 1) ? " " : ", ")); }
		Console.WriteLine("}");
	}

	public Chunk() {}
	public Chunk(int Length, byte[] Type, byte[] Data, byte[] Crc) 
	{
		this.Length = Length;
		this.Type = Type;
		this.Data = Data;
		this.Crc = Crc;
	}

	public static Chunk GetChunk(byte[] content, int index) 
	{
		Chunk chunk = new Chunk();
		chunk.Length = (content[index] << 24) + (content[index + 1] << 16) + (content[index + 2] << 8) + (content[index + 3] << 0);
		chunk.Type = main.GetBytesAt(content, index + 4, index + 7);
		chunk.Data = (chunk.Length > 0) ? main.GetBytesAt(content, index + 8, index + 8 + chunk.Length - 1) : new[] { (byte)0x00 };
		chunk.Crc = main.GetBytesAt(content, index + ((chunk.Length > 0) ? 9 : 8) + chunk.Length, index + ((chunk.Length > 0) ? 9 : 8) + chunk.Length + 3);
		return chunk;
	}
}

public class PNG 
{
	public readonly static string signature = "\x89PNG\x0d\x0a\x1a\x0a";
	public Chunk[] Chunks { set; get; }
	public int Width { set; get; }       // (4 bytes)
	public int Height { set; get; }      // (4 bytes)
	public int Depth { set; get; }       // (1 byte, values 1, 2, 4, 8, or 16)
	public int ColorType { set; get; }   // (1 byte, values 0, 2, 3, 4, or 6)
	public int Compression { set; get; } // (1 byte, value 0)
	public int Filter { set; get; }      // (1 byte, value 0)
	public int Interlace { set; get; }   // (1 byte, values 0 "no interlace" or 1 "Adam7 interlace") (13 data bytes total).[9]

	public void SetPixel(int x, int y, Color color) 
	{

	}

	public Color GetPixel(int x, int y) 
	{
		return new Color(0, 0, 0, 0);
	}

	public PNG() {}
	public PNG(byte[] content) 
	{
		PNG image = PNG.Decode(content);
		this.Chunks = image.Chunks;
		this.Width = image.Width;
		this.Height = image.Height;
		this.Depth = image.Depth;
		this.ColorType = image.ColorType;
		this.Compression = image.Compression;
		this.Filter = image.Filter;
		this.Interlace = image.Interlace;
	}

	public static PNG Decode(byte[] content) 
	{
		PNG image = new PNG();
		List<Chunk> chunks = new List<Chunk>();

		int i = PNG.signature.Length;
		while (i < content.Length) 
		{
			chunks.Add(Chunk.GetChunk(content, i));
			i += 4 + 4 + chunks[chunks.Count - 1].Length + 4;
		}

		image.Chunks = chunks.ToArray();
		image.Width = (chunks[0].Data[0] << 24) + (chunks[0].Data[1] << 16) + (chunks[0].Data[2] << 8) + (chunks[0].Data[3] << 0);
		image.Height = (chunks[0].Data[4] << 24) + (chunks[0].Data[5] << 16) + (chunks[0].Data[6] << 8) + (chunks[0].Data[7] << 0);
		image.Depth = chunks[0].Data[8];
		image.ColorType = chunks[0].Data[9];
		image.Compression = chunks[0].Data[10];
		image.Filter = chunks[0].Data[11];
		image.Interlace = chunks[0].Data[12];

		return image;
	}

	public static byte[] Encode(PNG image) 
	{
		List<byte> content = new List<byte>();
		for (int b = 0; b < PNG.signature.Length; b++) { content.Add((byte)(PNG.signature[b] & 0xff)); }

		content.Add((byte)0x00);
		content.Add((byte)0x00);
		content.Add((byte)0x00);
		content.Add((byte)0x0d);
		content.Add((byte)0x49);
		content.Add((byte)0x48);
		content.Add((byte)0x44);
		content.Add((byte)0x52);
		for (int b = 3; b >= 0; b--) { content.Add((byte)((image.Width >> (b * 8)) & 0xff)); }
		for (int b = 3; b >= 0; b--) { content.Add((byte)((image.Height >> (b * 8)) & 0xff)); }
		content.Add((byte)image.Depth);
		content.Add((byte)image.ColorType);
		content.Add((byte)image.Compression);
		content.Add((byte)image.Filter);
		content.Add((byte)image.Interlace);
		content.Add((byte)0x8d);
		content.Add((byte)0x32);
		content.Add((byte)0xcf);
		content.Add((byte)0xbd);

		for (int i = 1; i < image.Chunks.Length; i++) 
		{
			for (int b = 3; b >= 0; b--) { content.Add((byte)((image.Chunks[i].Length >> (b * 8)) & 0xff)); }
			for (int b = 0; b < image.Chunks[i].Type.Length; b++) { content.Add(image.Chunks[i].Type[b]); }
			for (int b = 0; b < image.Chunks[i].Data.Length; b++) { content.Add(image.Chunks[i].Data[b]); }
			for (int b = 0; b < image.Chunks[i].Crc.Length; b++) { content.Add(image.Chunks[i].Crc[b]); }
		}

		return content.ToArray();
	}
}

public class main 
{
	public static void Error(string message) { Console.WriteLine(message); Environment.Exit(1); }
	public static string DecimalToHex(int dec) 
	{
		string[] hexBy1 = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
		int num = (int)Math.Floor((double)dec / 16);
		string hex = hexBy1[dec % 16];

		while (num > 0) 
		{
			hex = hexBy1[num % 16] + hex;
			num = (int)Math.Floor((double)num / 16);
		}

		return hex;
	}

	public static byte[] GetBytesAt(byte[] _main, int startPos, int endPos) 
	{
	    if (_main == null || startPos < 0 || endPos <= 0 || endPos < startPos || endPos >= _main.Length) { return _main; }

	    List<byte> output = new List<byte>((endPos - startPos) + 1);
	    int i = startPos;
	    int t = 0;
	    while (t < (endPos - startPos) + 1) 
	    {
	        output.Add(_main[i]);
	        i++;
	        t++;
	    }

	    return output.ToArray();
	}

	static void Main(string[] args) 
	{
		PNG image = new PNG(File.ReadAllBytes("Untitled-2.png"));

		// if (GetBytesAt(image.content, 0, PNG.signature.Length - 1) != PNG.signature) { Error("Wrong signature."); }
		// if (image.Chunks[0].Type != "IHDR") { Error("First chunk is not IHDR."); }

		for (int i = 0; i < image.Chunks.Length; i++) 
		{
			Console.WriteLine(System.Text.Encoding.UTF8.GetString(image.Chunks[i].Type));
		}

		for (int i = 0; i < image.Chunks.Length; i++) 
		{
			switch (System.Text.Encoding.UTF8.GetString(image.Chunks[i].Type)) 
			{
				case "IDAT":
					for (int b = 0; b < image.Chunks[i].Data.Length; b++) { Console.Write(image.Chunks[i].Data[b]); Console.Write(" "); }
					Console.WriteLine("");
					continue;
			}
		}

		File.WriteAllBytes("output.png", PNG.Encode(image));
	}
}

/*

89 50 4e 47 0d 0a 1a 0a

CRC of chunk's type and content (but not length)

IHDR must be the first chunk; it contains (in this order) the image's
width (4 bytes)
height (4 bytes)
bit depth (1 byte, values 1, 2, 4, 8, or 16)
color type (1 byte, values 0, 2, 3, 4, or 6)
compression method (1 byte, value 0)
filter method (1 byte, value 0)
interlace method (1 byte, values 0 "no interlace" or 1 "Adam7 interlace") (13 data bytes total).[9]


Length	 Chunk type   	 Chunk data	    CRC
4 bytes	 4 bytes	     $Length bytes	4 bytes



ColorType:
0 (0002)	grayscale
2 (0102)	red, green and blue: rgb/truecolor
3 (0112)	indexed: channel containing indices into a palette of colors
4 (1002)	grayscale and alpha: level of opacity for each pixel
6 (1102)	red, green, blue and alpha

*/