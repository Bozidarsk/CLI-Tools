using System;
using System.Collections;
using System.Collections.Generic;

class main 
{
	[STAThread]
	static void Main(string[] args) 
	{
		if (args[0] == "clear" && args.Length == 1) 
		{
			System.Windows.Forms.Clipboard.Clear();
			return;
		}

		if (args[0] == "-txt" && args.Length == 2) 
		{
			System.Windows.Forms.Clipboard.SetText(args[1]);
			return;
		}
	}
}