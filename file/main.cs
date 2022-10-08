using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Attributes 
{
    private List<FileAttributes> A = new List<FileAttributes>();
    private List<string> Str = new List<string>();

    public int Count 
    {
    	get { return Math.Min(A.Count, Str.Count); }
    }

	public string Get(FileAttributes _A) 
    {
    	return Str[A.IndexOf(_A)];
    }

    public FileAttributes Get(string _str) 
    {
    	return A[Str.IndexOf(_str)];
    }

    public Attributes() 
    {
        List<FileAttributes> _A = new List<FileAttributes>();
    	_A.Add(FileAttributes.Hidden);
    	_A.Add(FileAttributes.ReadOnly);
    	_A.Add(FileAttributes.Normal);
    	_A.Add(FileAttributes.Temporary);
    	_A.Add(FileAttributes.Temporary);
    	_A.Add(FileAttributes.Temporary);
    	_A.Add(FileAttributes.System);

    	List<string> _Str = new List<string>();
    	_Str.Add("hidden");
    	_Str.Add("readonly");
    	_Str.Add("normal");
    	_Str.Add("tmp");
    	_Str.Add("temp");
    	_Str.Add("temporary");
    	_Str.Add("system");

    	A = _A;
    	Str = _Str;
    }
}

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

    static string GetToParentFolder(string dir) 
    {
    	if (dir.IndexOf("\\") <= -1) { return dir; }

    	if (dir.LastIndexOf(".") < dir.LastIndexOf("\\")) 
    	{	
	    	while (true) 
	    	{
	    		dir = dir.Remove(dir.Length - 1, 1);
	    		if (dir.Length <= 0) { break; }
	    		if (System.Convert.ToString(dir[dir.Length - 1]) == "\\") { break; }
	    	}

	    	return dir.Remove(dir.Length - 1, 1);
    	}

    	if (dir.LastIndexOf(".") > dir.LastIndexOf("\\")) 
    	{	
	    	bool startRemoving = false;
	    	int i = dir.Length - 1;

	    	while (i >= 0) 
	    	{
	    		if (startRemoving) { dir = dir.Remove(dir.Length - 1, 1); }
	    		if (dir.Length <= 0) { break; }
	    		if (System.Convert.ToString(dir[dir.Length - 1]) == "\\" && startRemoving) { break; }
	    		i--;

	    		if (System.Convert.ToString(dir[i]) == "\\") { startRemoving = true; }
	    	}

	    	return dir.Remove(dir.Length - 1, 1);
    	}

    	return dir;
    }

    static void MoveFile(string src, string dist) 
    {
    	if (src == "" || dist == "" || src == dist) { return; }
    	string content = File.ReadAllText(src);
		File.Delete(src);

		string fullDist = dist + "\\" + src;
    	if (dist == "." || dist == ".\\") { src = ""; dist = Directory.GetCurrentDirectory(); fullDist = dist; }

		if (fullDist.Contains("\\\\\\")) { fullDist = fullDist.Replace("\\\\\\", "\\"); }
		if (fullDist.Contains("\\\\")) { fullDist = fullDist.Replace("\\\\", "\\"); }
		Console.WriteLine(content);

		var file = File.Create(fullDist); file.Close();
		File.WriteAllText(fullDist, content);
    }

    static void CopyFile(string src, string dist) 
    {
    	if (src == "" || dist == "") { return; }
    	string content = File.ReadAllText(src);
    	if (src == dist) { src += " (copy)"; }

		string fullDist = dist + "\\" + src;

		if (fullDist.Contains("\\\\\\")) { fullDist = fullDist.Replace("\\\\\\", "\\"); }
		if (fullDist.Contains("\\\\")) { fullDist = fullDist.Replace("\\\\", "\\"); }

		File.WriteAllText(fullDist, content);
    }

	static void Main(string[] args) 
	{
		if (args.Length <= 0) { return; }
		if (args[0] == "help" || args[0] == "--help" || args[0] == "-h" || args[0] == "/h" || args[0] == "/?" || args[0] == "-?") 
		{
			Console.WriteLine(
				"Usage:\n\tfile \"filename\" [option] [arg]\n\nOptions:\n" + 
				"\t-wt \"text\"\tOverrides the file with text.\n" + 
				"\t-at \"text\"\tAppends the file with text.\n" + 
				"\t-a \"attribute\"\tSets attribute to the file. (hidden | readonly | normal | system | (tmp | temp | temporary))\n" + 
				"\t-mv \"directory\"\tMoves the file to directory.\n" + 
				"\t-cp \"directory\"\tCopies the file to directory.\n" + 
				"\t-rm\tRemoves the file.\n" + 
				"\t-mkdir \"directory\"\tMakes a new directory.\n" + 
				"\t-rmdir \"directory\"\tRemoves the directory.\n"
			);
		}

		int wt = Array.IndexOf(args, "-wt");
		int at = Array.IndexOf(args, "-at");
		int a = Array.IndexOf(args, "-a");

		int mv = Array.IndexOf(args, "-mv");
		int cp = Array.IndexOf(args, "-cp");

		int rm = Array.IndexOf(args, "-rm");
		int rmdir = Array.IndexOf(args, "-rmdir");
		int mkdir = Array.IndexOf(args, "-mkdir");

		if (args.Length % 2 == 0 && rm == -1 && mkdir == -1) { return; }
		if (args[0].IndexOf(".") <= 0 && rm == -1 && mkdir == -1) { return; }

    	if (wt >= 0 && at >= 0) { return; }
    	if (mv >= 0 && cp >= 0) { return; }

		Attributes attributes = new Attributes();
		string extention = GetStringAt(args[0], args[0].IndexOf("."), args[0].Length - 1);
		string newContent = "";

		try { newContent = File.ReadAllText("C:\\Tools\\Source\\default" + extention.Remove(0, 1)); }
		catch {}

		if (wt == -1 && at == -1) { File.WriteAllText(args[0], newContent); }
		if (wt >= 0) { File.WriteAllText(args[0], args[wt + 1]); }
		if (at >= 0) { File.AppendAllText(args[0], args[at + 1]); }
		if (a >= 0) { File.SetAttributes(args[0], attributes.Get(args[a + 1].ToLower())); }
		if (mv >= 0) { MoveFile(args[0], args[mv + 1]); }
		if (cp >= 0) { CopyFile(args[0], args[cp + 1]); }
		if (rm >= 0) { File.Delete(args[rm + 1]); }
		if (rmdir >= 0) { Directory.Delete(args[rmdir + 1]); }
		if (mkdir >= 0) { Directory.CreateDirectory(args[mkdir + 1]); }
	}
}