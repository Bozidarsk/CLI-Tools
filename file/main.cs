using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class main 
{
	static string GetStringAt(string main, int startPos, int endPos) 
    {
        int i = startPos;
        string output = "";

        if (startPos < 0 || endPos <= 0 || endPos < startPos || endPos >= main.Length) { return null; }
        if (startPos == endPos) { return System.Convert.ToString(main[startPos]); }
        if (startPos == -1) { startPos = 0; }
        if (endPos == -1) { endPos = main.Length - 1; }

        while (i < main.Length && i <= endPos) 
        {
            output += main[i];
            i++;
        }

        return output;
    }

    static int Max(int a, int b) { return (a > b) ? a : b; }

    static FileAttributes GetAttributes(string input) 
    {
    	FileAttributes output = 0;
    	string[] tokens = input.ToLower().Replace(" ", "").Split('|');
    	for (int i = 0; i < tokens.Length; i++) { output |= attributes[tokens[i]]; }
    	return output;
    }

	static void Main(string[] args) 
	{
		if (args.Length <= 0) { return; }
		if (args[0] == "help" || args[0] == "--help" || args[0] == "-h" || args[0] == "/h" || args[0] == "/?" || args[0] == "-?") 
		{
			Console.WriteLine(
				"Usage:\n\tfile \"filename\" [option] [arg]\n" + 
				"\nOptions:\n" + 
				"\t-wt, --write-text text              \tOverrides the file with text.\n" + 
				"\t-at, --append-text text             \tAppends the file with text.\n" + 
				"\t-a, --attributes attribute          \tSets attribute to the file. (hidden | readonly | normal | system | tmp)\n" + 
				"\t-mv, --move directory               \tMoves the file to directory.\n" + 
				"\t-cp, --copy directory               \tCopies the file to directory.\n" + 
				"\t-rm, --remove file                  \tRemoves the file.\n" + 
				"\t-mkdir, --make-directory directory  \tMakes a new directory.\n" + 
				"\t-rmdir, --remove-directory directory\tRemoves the directory.\n"
			);
			return;
		}

		int wt = Max(Array.IndexOf(args, "-wt"), Array.IndexOf(args, "--write-text"));
		int at = Max(Array.IndexOf(args, "-at"), Array.IndexOf(args, "--append-text"));
		int a = Max(Array.IndexOf(args, "-a"), Array.IndexOf(args, "--attributes"));

		int mv = Max(Array.IndexOf(args, "-mv"), Array.IndexOf(args, "--move"));
		int cp = Max(Array.IndexOf(args, "-cp"), Array.IndexOf(args, "--copy"));

		int rm = Max(Array.IndexOf(args, "-rm"), Array.IndexOf(args, "--remove"));
		int rmdir = Max(Array.IndexOf(args, "-rmdir"), Array.IndexOf(args, "--remove-directory"));
		int mkdir = Max(Array.IndexOf(args, "-mkdir"), Array.IndexOf(args, "--make-directory"));

		bool create = (~wt + ~at + ~a + ~mv + ~cp + ~rm + ~rmdir + ~mkdir) == 0;

		string extention = GetStringAt(args[0], args[0].IndexOf("."), args[0].Length - 1);
		string content = "";

		try { content = File.ReadAllText("C:\\Tools\\Source\\default" + extention); }
		catch {}

		if (create) { File.WriteAllText(args[0], content); }
		if (wt >= 0) { File.WriteAllText(args[0], args[wt + 1]); }
		if (at >= 0) { File.AppendAllText(args[0], args[at + 1]); }
		if (a >= 0) { File.SetAttributes(args[0], GetAttributes(args[a + 1])); }
		if (mv >= 0) { File.Move(args[0], args[mv + 1]); }
		if (cp >= 0) { File.Copy(args[0], args[cp + 1]); }
		if (rm >= 0) { File.Delete(args[rm + 1]); }
		if (rmdir >= 0) { Directory.Delete(args[rmdir + 1]); }
		if (mkdir >= 0) { Directory.CreateDirectory(args[mkdir + 1]); }
	}

	static Dictionary<string, FileAttributes> attributes = new Dictionary<string, FileAttributes>() 
	{
    	{ "hidden", FileAttributes.Hidden },
    	{ "readonly", FileAttributes.ReadOnly },
    	{ "normal", FileAttributes.Normal },
    	{ "tmp", FileAttributes.Temporary },
    	{ "temp", FileAttributes.Temporary },
    	{ "temporary", FileAttributes.Temporary },
    	{ "system", FileAttributes.System }
	};
}