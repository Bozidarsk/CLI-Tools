using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Utils;
using Utils.Files;
using Utils.Compression;

public sealed class Wallet 
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

	public Wallet() {}

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
				"Usage:\n\twallet <wallet> [command] <ammount>\n" + 
				"\nCommands:\n" + 
				"\tlist                           \tLists all wallets.\n" + 
				"\tchange <wallet> <newBaseCurrency>\tChanges the base currency of this wallet.\n" + 
				"\tnew <wallet> <baseCurrency=EUR> \tCreates a new wallet.\n" + 
				"\tdelete <wallet>                 \tDeletes the wallet.\n" + 
				"\tadd <wallet> <ammount>          \tAdds the ammount to this wallet.\n" + 
				"\tremove <wallet> <ammount>       \tRemoves the ammount from this wallet.\n" + 
				"\thas <wallet> <ammount>          \tReturns the leftover ammount from this wallet. (-1 if it does not have)\n" + 
				"\tclear <wallet>                  \tClears the wallet. (removes everything)\n" + 
				"\tshow <wallet>                   \tShows everything from this wallet.\n"
			);
			Tools.Exit(ExitReason.HelpMessage);
		}

		try { if (args.Length != argc[args[0]] && args[0] != "new") { Tools.Exit(ExitReason.InvalidArguments); } }
		catch { Tools.Exit(ExitReason.InvalidArguments); }

		string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Wallets\\";
		if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

		if (args.Length >= 2 && !File.Exists(path + args[1] + ".wallet") && args[0] != "new") 
		{
			Console.WriteLine("Wallet '" + args[1] + "' does not exist.");
			Tools.Exit(ExitReason.Error);
		}

		switch (args[0]) 
		{
			case "list":
				string[] wallets = Directory.GetFiles(path, "*.wallet");
				for (int i = 0; i < wallets.Length; i++) { Console.WriteLine(String2.GetStringAt(wallets[i], wallets[i].LastIndexOf("\\") + 1, wallets[i].Length - ".wallet".Length - 1)); }
				break;
			case "new":
				File.WriteAllBytes(
					path + args[1] + ".wallet",
					Deflate.Compress(Encoding.UTF8.GetBytes("{\"name\":\"" + args[1] + "\",\"baseCurrency\":\"" + ((args.Length == 3) ? args[2] : "EUR") + "\",\"currencies\":[]}"))
				);
				break;
			case "delete":
				Console.Write("Do you want to delete this wallet? [Y/n] ");
				if (!Tools.YesNo(Console.ReadLine())) { Tools.Exit(ExitReason.Abort); }
				File.Delete(path + args[1] + ".wallet");
				break;
			case "add":
			case "remove":
			case "has":
			case "show":
			case "clear":
			case "change":
				Wallet wallet = Json.FromJson<Wallet>(Encoding.UTF8.GetString(Deflate.Decompress(File.ReadAllBytes(path + args[1] + ".wallet"))));
				wallet.UpdateList();

				if (args[0] == "add") { wallet.Add(new Wallet.Currency(args[2])); }
				if (args[0] == "remove") { wallet.Remove(new Wallet.Currency(args[2])); }
				if (args[0] == "has") { Console.WriteLine(wallet.Has(new Wallet.Currency(args[2]))); break; }
				if (args[0] == "change") { wallet.baseCurrency = args[2]; }
				if (args[0] == "clear") 
				{
					Console.Write("Do you want to clear this wallet? [Y/n] ");
					if (!Tools.YesNo(Console.ReadLine())) { Tools.Exit(ExitReason.Abort); }
					wallet.currencies = new Wallet.Currency[] {};
				}
				if (args[0] == "show") 
				{
					float total = 0f;
					List<string> notFound = new List<string>();
					for (int i = 0; i < wallet.currencies.Length; i++) 
					{
						Console.WriteLine(wallet.currencies[i].ToString());
						try { total += CurrencyConverter.Exchange(wallet.currencies[i].type, wallet.baseCurrency, wallet.currencies[i].value); }
						catch { notFound.Add(wallet.currencies[i].type + " and " + wallet.baseCurrency); }
					}

					if (notFound.Count == 0) { Console.WriteLine("\nTotal: " + total.ToString() + wallet.baseCurrency); }
					else 
					{
						Console.WriteLine("Exchange rates not found between:");
						for (int i = 0; i < notFound.Count; i++) { Console.WriteLine("\t" + notFound[i]); }
					}
					break;
				}

				File.WriteAllBytes(
					path + args[1] + ".wallet",
					Deflate.Compress(Encoding.UTF8.GetBytes(Json.ToJson(wallet)))
				);
				break;
			default:
				Tools.Exit(ExitReason.InvalidArguments);
				return;
		}

		Tools.Exit(ExitReason.Success);
	}
}