using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class main 
{
	static string[] Join(params string[][] array) 
	{
		List<string> list = new List<string>();
		for (int i = 0; i < array.Length; i++) 
		{
			for (int t = 0; t < array[i].Length; t++) 
			{
				list.Add(array[i][t]);
			}
		}

		return list.ToArray();
	}

	static string GetPath(string main) 
	{
		int i = main.Length - 1;
		while (i > 0 && !(main[i] == '\x2f' || main[i] == '\x5c')) { i--; }
		return main.Remove(i + ((i > 0) ? 1 : 0));
	}

	static void Main(string[] args) 
	{
		int filesCount = 0;
		int dirsCount = 0;

		int searchDir = -1;
		bool recursive = false;
		bool showPath = false;
		bool showCount = false;
		bool filesOnly = false;
		bool dirsOnly = false;
		List<string> conditions = new List<string>();

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
					Console.WriteLine("ls [condition0] [condition1] [condition...]\nLists all files that match the conditions.\n-r - searches every child directory.\n-p - shows the full path of every displayed file.\n-f - shows only files.\n-d - shows only directory.\n-c - counts how many files and directories are displayed.\n-s [directory] - searches the specified directory. (relative to the working directory)");
					return;
				case "-r":
					recursive = true;
					continue;
				case "-p":
					showPath = true;
					continue;
				case "-c":
					showCount = true;
					continue;
				case "-f":
					filesOnly = true;
					continue;
				case "-d":
					dirsOnly = true;
					continue;
				case "-s":
					if (i + 1 >= args.Length) { Console.WriteLine("The search directory is not valid."); Environment.Exit(1); }
					searchDir = i + 1;
					continue;
				default:
					if (i == searchDir) { break; }
					conditions.Add(args[i]);
					break;
			}
		}

		string path = "";
		string cdir = Directory.GetCurrentDirectory();
		if (searchDir >= 0) 
		{
			cdir += "\\" + args[searchDir];
			if (args[searchDir].Contains(":")) { cdir = args[searchDir]; }
			cdir = cdir.Replace("\\\\", "\\");
			if (!Directory.Exists(cdir)) { Console.WriteLine("The search directory does not exists."); Environment.Exit(1); }
		}

		if (filesOnly && dirsOnly) { Console.WriteLine("Can not list only files and only directories. (-f -d)\n"); Environment.Exit(1); }		
		if (conditions.Count == 0) { conditions.Add("*"); }

		string[] files = { "" };
		string[] dirs = { "" };
		for (int i = 0; i < conditions.Count; i++) 
		{
			files = Join(files, Directory.GetFiles(cdir, conditions[i], (recursive) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
			dirs = Join(dirs, Directory.GetDirectories(cdir, conditions[i], (recursive) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
		}

		Array.Sort(files, StringComparer.CurrentCultureIgnoreCase);
		Array.Sort(dirs, StringComparer.CurrentCultureIgnoreCase);


		// print only files
		Console.ForegroundColor = ConsoleColor.Green;
		for (int i = 0; i < files.Length && filesOnly; i++) 
		{
			files[i] = files[i].Replace(cdir + "\\", "");
			if (files[i] != "") 
			{
				Console.ForegroundColor = ConsoleColor.White;
				path = GetPath(files[i]);
				filesCount++;

				if (showPath) { Console.Write(cdir + "\\" + path); }

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine((path.Length != 0) ? files[i].Replace(path, "") : files[i]);
				path = "";
			}
		}


		// print only directories
		Console.BackgroundColor = ConsoleColor.Green;
		Console.ForegroundColor = ConsoleColor.DarkBlue;
		for (int i = 0; i < dirs.Length && dirsOnly; i++) 
		{
			dirs[i] = dirs[i].Replace(cdir + "\\", "");
			if (dirs[i] != "") 
			{
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.White;
				path = GetPath(dirs[i]);
				dirsCount++;

				if (showPath) { Console.Write(cdir + "\\" + path); }
			
				Console.BackgroundColor = ConsoleColor.Green;
				Console.ForegroundColor = ConsoleColor.DarkBlue;
				Console.WriteLine((path.Length != 0) ? dirs[i].Replace(path, "") : dirs[i]);
				path = "";
			}
		}


		// print files and directories
		if (!filesOnly && !dirsOnly) 
		{
			int u = 0;
			string[] array = new string[files.Length + dirs.Length];
			List<string> file = new List<string>();

			for (int i = 0; i < files.Length; i++) { array[u] = files[i].Replace(cdir + "\\", ""); file.Add(array[u]); u++; }
			for (int i = 0; i < dirs.Length; i++) { array[u] = dirs[i].Replace(cdir + "\\", ""); u++; }
			Array.Sort(array, StringComparer.CurrentCultureIgnoreCase);

			for (int i = 0; i < array.Length; i++) 
			{
				int f = 0;
				while (f < file.Count) 
				{
					if (array[i] == file[f]) { break; }
					f++;
				}

				if (array[i] != "") 
				{
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.White;
					path = GetPath(array[i]);

					if (showPath) { Console.Write(cdir + "\\" + path); }
					if (f < file.Count) 
					{
						filesCount++;
						Console.BackgroundColor = ConsoleColor.Black;
						Console.ForegroundColor = ConsoleColor.Green;
					}
					else 
					{
						dirsCount++;
						Console.BackgroundColor = ConsoleColor.Green;
						Console.ForegroundColor = ConsoleColor.DarkBlue;
					}

					Console.WriteLine((path.Length != 0) ? array[i].Replace(path, "") : array[i]);
					path = "";
				}
			}
		}

		if (showCount) 
		{
			Console.WriteLine("");
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(System.Convert.ToString(filesCount) + " file" + ((filesCount > 1) ? "s." : "."));
			Console.Write(System.Convert.ToString(dirsCount) + " director" + ((dirsCount > 1) ? "ies." : "y."));
		}

		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.Gray;
	}
}