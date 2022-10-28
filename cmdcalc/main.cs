using System;
using System.Collections;
using System.Collections.Generic;
using Utils.Calculators;

class main 
{
	static void Main(string[] args) 
	{
		if (args.Length < 1) { return; }
		TrigType trigType = TrigType.Radians;
		if (Array.IndexOf(args, "-h") >= 0 || Array.IndexOf(args, "/?") >= 0 || Array.IndexOf(args, "--help") >= 0 || Array.IndexOf(args, "/help") >= 0 || Array.IndexOf(args, "-help") >= 0 || Array.IndexOf(args, "/h") >= 0 || Array.IndexOf(args, "-?") >= 0 || Array.IndexOf(args, "help") >= 0) { Console.WriteLine(BasicCalculator.Doc); if (args.Length == 1) { Environment.Exit(0); } }
		if (Array.IndexOf(args, "--deg") >= 0 || Array.IndexOf(args, "-d") >= 0 || Array.IndexOf(args, "-deg") >= 0 || Array.IndexOf(args, "/deg") >= 0 || Array.IndexOf(args, "/d") >= 0) { trigType = TrigType.Degrees; }
		if (Array.IndexOf(args, "--rad") >= 0 || Array.IndexOf(args, "-r") >= 0 || Array.IndexOf(args, "-rad") >= 0 || Array.IndexOf(args, "/rad") >= 0 || Array.IndexOf(args, "/r") >= 0) { trigType = TrigType.Radians; }

		Console.WriteLine(BasicCalculator.Solve(args[0]), trigType);
	}
}