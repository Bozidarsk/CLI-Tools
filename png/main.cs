using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Utils;
using Utils.Files;

class main 
{
	static void Main(string[] args) 
	{
		PNG image = new PNG("10x10.png");
		image.Save("out.png");
	}
}

/*

89 50 4e 47 0d 0a 1a 0a

CRC of chunk's type and content (but not length)

IHDR must be the first chunk, it contains (in this order) the image's
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