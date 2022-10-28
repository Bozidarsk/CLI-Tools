using System;
using System.Collections;
using System.Collections.Generic;
using Utils.Calculators;

class main 
{
	static void Main(string[] args) 
	{
		if (args.Length != 1) { return; }
		if (args[0] == "help" || args[0] == "--help" || args[0] == "-h" || args[0] == "/h" || args[0] == "/?" || args[0] == "-?") 
		{
			Console.WriteLine("args[0] - string input\n\nexample: \"x^6 -3x^5 -4x^4 +26x^3 -39x^2 +25x -6 < 0\" is equal to \"+1-3-4+26-39+25-6<\"\n" + PolynomialCalculator.Doc);
			return;
		}

		string result, simplified, table;
		result = PolynomialCalculator.Solve(args[0], out simplified, out table);
		try { Console.WriteLine(table + "\n" + simplified + "\n\n" + result); }
		catch { Console.WriteLine("Null output. (probably x = {})"); }
	}
}