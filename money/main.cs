using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Utils;
using Utils.Files;
using Utils.Compression;

public sealed class Table 
{
	public string name;
	public string baseCurrency;
	public Currency[] currencies;
	private List<Currency> list;

	public void Add(Currency currency) 
	{
		for (int i = 0; i < list.Count; i++) { if (list[i].type == currency.type) { list[i].value += currency.value; UpdateArray(); return; } }
		list.Add(currency);
		UpdateArray();
	}

	public void Remove(Currency currency) 
	{
		for (int i = 0; i < list.Count; i++) 
		{
			if (list[i].type == currency.type) 
			{
				list[i].value -= currency.value;
				if (list[i].value <= 0f) { list.RemoveAt(i); }
				UpdateArray();
			}
		}
	}

	public float Has(Currency currency) 
	{
		for (int i = 0; i < list.Count; i++) 
		{
			if (list[i].type == currency.type) 
			{
				float output = list[i].value - currency.value;
				if (output < 0f) { return -1f; }
				else { return output; }
			}
		}

		return -1f;
	}

	public void UpdateArray() { currencies = list.ToArray(); }
	public void UpdateList() { list = currencies.ToList(); }

	public sealed class Currency 
	{
		public float value;
		public string type;

		public override string ToString() { return this.type + ": " + this.value.ToString(); }

		public Currency(string input) 
		{
			if (string.IsNullOrEmpty(input)) { throw new NullReferenceException("Input must not be null."); }
			string output = "";
			for (int i = 0; Char.IsDigit(input[i]) || input[i] == '.'; i++) { output += input[i]; }
			this.value = float.Parse(output);
			this.type = String2.GetStringAt(input, output.Length, input.Length - 1);
		}

		public Currency() {}
		public Currency(float value, string type) 
		{
			this.value = value;
			this.type = type;
		}
	}
}

class main 
{
	static Dictionary<string, int> argc = new Dictionary<string, int> 
	{
		{ "list", 1 },
		{ "change", 3 },
		{ "new", 2 },
		{ "delete", 2 },
		{ "clear", 2 },
		{ "show", 2 },
		{ "add", 3 },
		{ "remove", 3 },
		{ "has", 3 }
	};

	static void Main(string[] args) 
	{
		if (args.Length <= 0) { Tools.Exit(ExitReason.InvalidArguments); }
		if (args[0] == "help" || args[0] == "--help" || args[0] == "-h" || args[0] == "/h" || args[0] == "/?" || args[0] == "-?") 
		{
			Console.WriteLine(
				"Usage:\n\tmoney <table> [command] <ammount>\n" + 
				"\nCommands:\n" + 
				"\tlist                           \tLists all tables.\n" + 
				"\tchange <tabe> <newBaseCurrency>\tChanges the base currency of this table.\n" + 
				"\tnew <table> <baseCurrency=EUR> \tCreates a new table.\n" + 
				"\tdelete <table>                 \tDeletes the table.\n" + 
				"\tadd <table> <ammount>          \tAdds the ammount to this table.\n" + 
				"\tremove <table> <ammount>       \tRemoves the ammount from this table.\n" + 
				"\thas <table> <ammount>          \tReturns the leftover ammount from this table. (-1 if it does not have)\n" + 
				"\tclear <table>                  \tClears the table. (removes everything)\n" + 
				"\tshow <table>                   \tShows everything from this table.\n"
			);
			Tools.Exit(ExitReason.HelpMessage);
		}

		if (args.Length != argc[args[0]] && args[0] != "new") { Tools.Exit(ExitReason.InvalidArguments); }

		string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MoneyTables\\";
		if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

		if (args.Length >= 2 && !File.Exists(path + args[1] + ".table") && args[0] != "new") 
		{
			Console.WriteLine("Table '" + args[1] + "' does not exist.");
			Tools.Exit(ExitReason.Error);
		}

		switch (args[0]) 
		{
			case "list":
				string[] tables = Directory.GetFiles(path, "*.table");
				for (int i = 0; i < tables.Length; i++) { Console.WriteLine(String2.GetStringAt(tables[i], tables[i].LastIndexOf("\\") + 1, tables[i].Length - ".table".Length - 1)); }
				break;
			case "new":
				File.WriteAllBytes(
					path + args[1] + ".table",
					Deflate.Compress(Encoding.UTF8.GetBytes("{\"name\":\"" + args[1] + "\",\"baseCurrency\":\"" + ((args.Length == 3) ? args[2] : "EUR") + "\",\"currencies\":[]}"))
				);
				break;
			case "delete":
				Console.Write("Do you want to delete this table? [Y/n] ");
				if (!Tools.YesNo(Console.ReadLine())) { Tools.Exit(ExitReason.Abort); }
				File.Delete(path + args[1] + ".table");
				break;
			case "add":
			case "remove":
			case "has":
			case "show":
			case "clear":
			case "change":
				Table table = Json.FromJson<Table>(Encoding.UTF8.GetString(Deflate.Decompress(File.ReadAllBytes(path + args[1] + ".table"))));
				table.UpdateList();

				if (args[0] == "add") { table.Add(new Table.Currency(args[2])); }
				if (args[0] == "remove") { table.Remove(new Table.Currency(args[2])); }
				if (args[0] == "has") { Console.WriteLine(table.Has(new Table.Currency(args[2]))); break; }
				if (args[0] == "change") { table.baseCurrency = args[2]; }
				if (args[0] == "clear") 
				{
					Console.Write("Do you want to clear this table? [Y/n] ");
					if (!Tools.YesNo(Console.ReadLine())) { Tools.Exit(ExitReason.Abort); }
					table.currencies = new Table.Currency[] {};
				}
				if (args[0] == "show") 
				{
					float total = 0f;
					List<string> notFound = new List<string>();
					for (int i = 0; i < table.currencies.Length; i++) 
					{
						Console.WriteLine(table.currencies[i].ToString());
						try { total += CurrencyConverter.Exchange(table.currencies[i].type, table.baseCurrency, table.currencies[i].value); }
						catch { notFound.Add(table.currencies[i].type + " and " + table.baseCurrency); }
					}

					if (notFound.Count == 0) { Console.WriteLine("\nTotal: " + total.ToString() + table.baseCurrency); }
					else 
					{
						Console.WriteLine("Exchange rates not found between:");
						for (int i = 0; i < notFound.Count; i++) { Console.WriteLine("\t" + notFound[i]); }
					}
					break;
				}

				File.WriteAllBytes(
					path + args[1] + ".table",
					Deflate.Compress(Encoding.UTF8.GetBytes(Json.ToJson(table)))
				);
				break;
			default:
				Tools.Exit(ExitReason.InvalidArguments);
				return;
		}

		Tools.Exit(ExitReason.Success);
	}
}