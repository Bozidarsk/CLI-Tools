using System;
using System.Collections;
using System.Collections.Generic;

class main 
{
	static string JoinArray(string[] array, string separator) 
    {
        int i = 0;
        string newString = "";
        while (i < array.Length) { newString += array[i] + separator; i++; }
        return newString.Remove(newString.Length - separator.Length, separator.Length);
    }

    static void StartEXE(string path, string args) 
    {
        //System.Diagnostics.Process.Start("cmd.exe", "/C " + path + " " + args);
        var cmd = new System.Diagnostics.Process();
        cmd.StartInfo = new System.Diagnostics.ProcessStartInfo(path, args) 
        {
            UseShellExecute = false
        };

        cmd.Start();
        cmd.WaitForExit();
    }

    static bool EndsWith(string main, string target) 
    {
        int i = main.Length - 1;
        int t = target.Length - 1;
        bool pass = true;

        if (main.Length < target.Length) { pass = false; }

        while (i >= main.Length - target.Length && pass) 
        {
            if (main[i] != target[t]) { pass = false; }
            i--;
            t--;
        }

        return pass;
    }

    static void Main(string[] args) 
	{
		const string cscDirDefault = "C:\\Windows\\Microsoft.NET\\Framework\\v3.5\\csc.exe";
		string cscDir = "C:\\Windows\\Microsoft.NET\\Framework\\v3.5\\csc.exe";
		string helpMsg = "Compiler path: '" + cscDir + "'.\n\n" + "args[0] - source file name (if not specified the extention will be .cs).\nargs[1] - output file name (if not specified the extention will be .exe).\n\n-h - Displays this message.\n-u - Compiles with custom assemply ('C:\\Tools\\Utils.dll'). It must be the last argument!\n--changeCompilerPath [newPath] (--default - for the default path) - Changes the path of the compiler (csc.exe is the default).\n\nAnything else will be directly passed to the compiler!";

		if (args.Length == 1) { if (args[0] == "help" || args[0] == "--help" || args[0] == "/help" || args[0] == "-?" || args[0] == "/?" || args[0] == "/h" || args[0] == "-h") { Console.WriteLine(helpMsg); return; } }
		if (args.Length == 2) 
		{
			if (args[0] == "-changeCompilerPath" || args[0] == "--changeCompilerPath" || args[0] == "/changeCompilerPath") 
			{
				if (args[1] == "default" || args[1] == "-default" || args[1] == "--default" || args[1] == "/default") { Console.WriteLine("NOT WORKING BTW!!!!\nCompiler path changed to: '" + cscDirDefault + "'."); return; }
				else { Console.WriteLine("NOT WORKING BTW!!!!\nCompiler path changed to: '" + args[1] + "'."); return; }
			}
		}

		if (args.Length <= 1) { Console.WriteLine("Wrong arguments."); return; }

		if (args.Length == 2) 
		{
			if (!EndsWith(args[0], ".cs")) { args[0] += ".cs"; }
			if (!EndsWith(args[1], ".exe")) { args[1] += ".exe"; }

			StartEXE(cscDir, "/t:exe /out:\"" + args[1] + "\" \"" + args[0] + "\"");
			return;
		}

        if (args.Length == 3 && (Array.IndexOf(args, "-u") == args.Length - 1 || Array.IndexOf(args, "/u") == args.Length - 1)) 
        {
            if (!EndsWith(args[0], ".cs")) { args[0] += ".cs"; }
            if (!EndsWith(args[1], ".exe")) { args[1] += ".exe"; }

            StartEXE(cscDir, "/t:exe /out:\"" + args[1] + "\" \"C:\\Tools\\Utils.cs\" \"" + args[0] + "\"");
            return;
        }

        if (args.Length == 3 && (Array.IndexOf(args, "-l") == args.Length - 1 || Array.IndexOf(args, "/l") == args.Length - 1)) 
        {
            if (!EndsWith(args[0], ".cs")) { args[0] += ".cs"; }
            if (!EndsWith(args[1], ".dll")) { args[1] += ".dll"; }

            StartEXE(cscDir, "/target:library /out:\"" + args[1] + "\" \"" + args[0] + "\"");
            return;
        }

		StartEXE(cscDir, "\"" + JoinArray(args, "\" \"") + "\"");
	}
}