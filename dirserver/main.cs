using System;
using System.Collections;
using System.Collections.Generic;
using Utils;
using Utils.Web;

class main 
{
	public static void Main(string[] args) 
	{
		if (args.Length == 1) 
		{
			if (args[0] == "-h" || args[0] == "--help" || args[0] == "help" || args[0] == "/h" || args[0] == "/help" || args[0] == "/?") 
			{
				Console.WriteLine("args[0] - path to folder (can be relative)\nargs[1] - port (default is 8000)\nargs[2] - show/hide window (true/false) (optional)");
				return;
			}
		}

		string dir = (args.Length >= 1) ? args[0].TrimStart('\\').TrimEnd('\\') : ".";
		int port = (args.Length >= 2) ? Convert.ToInt32(args[1]) : 8000;
		bool showWindow = (args.Length >= 3) ? ((args[2] == "true") ? true : false) : true;
		string directory = (!dir.Contains(":")) ? Tools.FormatDirectory(System.IO.Directory.GetCurrentDirectory() + "\\" + dir) : dir;

		HTTPServer server = new HTTPServer(WebTools.GetLocalIP(), port, new HTTPServer.ResponseMethod(HTTPServer.Response.GenerateDirectoryResponse), directory);
		Console.Title = "Directory Listing for: '" + directory + "'";
		server.Start();
	}
}