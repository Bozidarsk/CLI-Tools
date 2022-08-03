using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class main 
{
	static string DecimalToHex(int dec) 
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

	static void Main(string[] args) 
	{
		string cdir = Directory.GetCurrentDirectory();
		List<string> files = new List<string>();
		bool useBytes = false;

		for (int i = 0; i < args.Length; i++) 
		{
			switch (args[i]) 
			{
				case "-h":
				case "--help":
				case "/h":
				case "/help":
				case "help":
				case "/?":
					Console.WriteLine("cat [filename0] [filename1] [filename...] - displays the content of the files.\nAdd ' > [outputfilename]' to display the content of the files in specified file.\n-b - changes everything to hex bytes.");
					return;
				case "-b":
					useBytes = true;
					continue;
				default:
					files.Add(args[i]);
					continue;
			}
		}

		for (int i = 0; i < files.Count; i++) 
		{
			if (Directory.Exists(files[i])) { Console.WriteLine("'" + files[i] + "' is a directory, not a file."); continue; }
			if (!useBytes) { Console.WriteLine(File.ReadAllText(cdir + "\\" + files[i]).Replace("\t", "    ")); }
			else 
			{
				byte[] bytes = File.ReadAllBytes(cdir + "\\" + files[i]);
				for (int b = 0; b < bytes.Length; b++) 
				{
					string result = DecimalToHex((int)bytes[b]);
					Console.Write((result.Length == 1) ? "0" + result : result);
					Console.Write((((b & 0xf) % 15 == 0 && (b & 0xf) != 0) || b >= bytes.Length - 1) ? "\n" : " ");
					Console.Write(((b & 0xf) == 7) ? " " : "");
				}
			}
		}
	}
}