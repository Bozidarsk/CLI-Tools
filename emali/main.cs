using System;
using System.Collections;
using System.Collections.Generic;

class main 
{
	static void Main(string[] args) 
	{
		Console.WriteLine("NOT WORKING.");
		return;

		if (args.Length < 1) { return; }
		if (args[0] == "help" || args[0] == "--help" || args[0] == "-h" || args[0] == "/h" || args[0] == "/?" || args[0] == "-?") 
		{
			Console.WriteLine(
				"args[0] - from email\n" + 
				"args[1] - to email\n" + 
				"args[2] - subject\n" + 
				"args[3] - body"
			);
			return;
		}

		if (args.Length != 4) { return; }
		Email.SendEmail(args[0], args[1], args[2], args[3]);
	}
}