using System;
using System.Collections;
using System.Collections.Generic;

class main 
{
	static void Main(string[] args) 
	{
		if (args.Length < 1) { return; }
		if (args[0] == "help" || args[0] == "--help" || args[0] == "-h" || args[0] == "/h" || args[0] == "/?" || args[0] == "-?") 
		{
			Console.WriteLine(
				"args[0] - password\n" + 
				"args[1] - from email\n" + 
				"args[2] - to email\n" + 
				"args[3] - subject\n" + 
				"args[4] - body" + 
				"args[5] - smtp server (optional)"
			);
			return;
		}

		if (args.Length < 4) { return; }
		try { Email.SendEmail(args[0], args[1], args[2], args[3], args[4], args[5]); }
		catch { Email.SendEmail(args[0], args[1], args[2], args[3], args[4]); }
	}
}