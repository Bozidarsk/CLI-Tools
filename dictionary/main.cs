using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Utils;
using Utils.Files;
using Utils.Compression;

public sealed class Dictionary 
{
	public Pair[] translation;

	new public static Func<string, string, bool> Equals = (x, key) => { return x == key; };
	public static Func<string, string, bool> Contains = (x, key) => { return x.Contains(key); };

	private string file;

	public Pair[] Search(string key) { return Search(key, Dictionary.Equals); }
	public Pair[] Search(string key, Func<string, string, bool> method) 
	{ return translation.Where(x => method(x.lType, key) || method(x.rType, key)).ToArray(); }

	public void Add(string lType, string rType) { Add(new Pair(lType, rType)); }
	public void Add(Pair pair) 
	{
		Pair[] pairs = translation.Where(x => x.lType == pair.lType && x.rType == pair.rType).ToArray();
		if (pairs.Length > 0) { return; }

		translation = Array2.Join(translation, new Pair[] { pair });
	}

	public void Remove(string lType, string rType) { Remove(new Pair(lType, rType)); }
	public void Remove(Pair pair) 
	{
		Pair[] pairs = translation.Where(x => x.lType == pair.lType && x.rType == pair.rType).ToArray();
		if (pairs.Length <= 0) { return; }

		List<Pair> list = translation.ToList();
		for (int i = 0; i < list.Count; i++) 
		{
			int t = 0;
			for (; t < pairs.Length; t++) { if (list[i].Equals(pairs[t])) { list.RemoveAt(i); break; } }
			if (t < pairs.Length) { i--; }
		}

		translation = list.ToArray();
	}

	public void Save() { File.WriteAllText(file, Json.ToJson(this, true)); }
	public void Save(string file) { File.WriteAllText(file, Json.ToJson(this, true)); }

	public override string ToString() 
	{
		string output = "";
		for (int i = 0; i < translation.Length - 1; i++) { output += translation[i] + "\n"; }
		return output + ((translation.Length > 0) ? translation[translation.Length - 1].ToString() : "");
	}

	public Dictionary() {}
	public Dictionary(string file) 
	{
		Dictionary dictionary = Json.FromJsonFile<Dictionary>(file);
		this.translation = dictionary.translation;
		this.file = file;
	}

	public sealed class Pair 
	{
		public string lType;
		public string rType;

		public override string ToString() { return "'" + this.lType + "' - '" + this.rType + "'"; }
		public override bool Equals(object pair) { return this.lType == ((Pair)pair).lType && this.rType == ((Pair)pair).rType; }
		public override int GetHashCode() { return this.ToString().GetHashCode(); }

		public Pair() {}
		public Pair(string lType, string rType) 
		{
			this.lType = lType;
			this.rType = rType;
		}
	}
}

class main 
{
	static Dictionary<string, int> argc = new Dictionary<string, int> 
	{
		{ "list", 1 },
		{ "new", 2 },
		{ "delete", 2 },
		{ "clear", 2 },
		{ "show", 2 },
		{ "add", 4 },
		{ "remove", 4 },
		{ "search", 3 }
	};

	static void Main(string[] args) 
	{
		if (args.Length <= 0) { Tools.Exit(ExitReason.InvalidArguments); }
		if (args[0] == "help" || args[0] == "--help" || args[0] == "-h" || args[0] == "/h" || args[0] == "/?" || args[0] == "-?") 
		{
			Console.WriteLine(
				"Usage:\n\tdictionary <type> [command] <lType> <rType>\n" + 
				"\nCommands:\n" + 
				"\tlist                         \tLists all dictionaries.\n" + 
				"\tnew <type>                   \tCreates a new dictionary.\n" + 
				"\tdelete <type>                \tDeletes the dictionary.\n" + 
				"\tadd <type> <lType> <rType>   \tAdds the pair to this dictionary.\n" + 
				"\tremove <type> <lType> <rType>\tRemoves the pair from this dictionary.\n" + 
				"\tsearch <type> <key>          \tReturns all pairs which contains the key in this dictionary.\n" + 
				"\tclear <type>                 \tClears the dictionary. (removes everything)\n" + 
				"\tshow <type>                  \tShows everything from this dictionary.\n" + 
				"\nExample:\n" + 
				"\tdictionary en-de add \"example\" \"beispiel\"\n"
			);
			Tools.Exit(ExitReason.HelpMessage);
		}

		try { if (args.Length != argc[args[0]] && args[0] != "new") { Tools.Exit(ExitReason.InvalidArguments); } }
		catch { Tools.Exit(ExitReason.InvalidArguments); }

		string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Dictionaries\\";
		if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

		if (args.Length >= 2 && !File.Exists(path + args[1] + ".dictionary") && args[0] != "new") 
		{
			Console.WriteLine("Type '" + args[1] + "' does not exist.");
			Tools.Exit(ExitReason.Error);
		}

		switch (args[0]) 
		{
			case "list":
				string[] dictionaries = Directory.GetFiles(path, "*.dictionary");
				for (int i = 0; i < dictionaries.Length; i++) { Console.WriteLine(String2.GetStringAt(dictionaries[i], dictionaries[i].LastIndexOf("\\") + 1, dictionaries[i].Length - ".dictionary".Length - 1)); }
				break;
			case "new":
				File.WriteAllBytes(
					path + args[1] + ".dictionary",
					Deflate.Compress(Encoding.UTF8.GetBytes("{\"translation\":[]}"))
				);
				break;
			case "delete":
				Console.Write("Do you want to delete this dictionary? [Y/n] ");
				if (!Tools.YesNo(Console.ReadLine())) { Tools.Exit(ExitReason.Abort); }
				File.Delete(path + args[1] + ".dictionary");
				break;
			case "add":
			case "remove":
			case "search":
			case "show":
			case "clear":
				Dictionary dictionary = Json.FromJson<Dictionary>(Encoding.UTF8.GetString(Deflate.Decompress(File.ReadAllBytes(path + args[1] + ".dictionary"))));

				if (args[0] == "add") { dictionary.Add(args[2], args[3]); }
				if (args[0] == "remove") { dictionary.Remove(args[2], args[3]); }
				if (args[0] == "show") { Console.WriteLine(dictionary.ToString()); }
				if (args[0] == "search") 
				{
					Dictionary.Pair[] pairs = dictionary.Search(args[2]);
					for (int i = 0; i < pairs.Length; i++) { Console.WriteLine(pairs[i].ToString()); }
				}
				if (args[0] == "clear") 
				{
					Console.Write("Do you want to clear this dictionary? [Y/n] ");
					if (!Tools.YesNo(Console.ReadLine())) { Tools.Exit(ExitReason.Abort); }
					dictionary.translation = new Dictionary.Pair[] {};
				}

				File.WriteAllBytes(
					path + args[1] + ".dictionary",
					Deflate.Compress(Encoding.UTF8.GetBytes(Json.ToJson(dictionary, true)))
				);
				break;
			default:
				Tools.Exit(ExitReason.InvalidArguments);
				return;
		}

		Tools.Exit(ExitReason.Success);
	}
}