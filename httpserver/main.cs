using System;
using System.Collections;
using System.Collections.Generic;
using Utils.Web;

class main 
{
	public static void Main(string[] args) 
	{
		if (args.Length == 1) 
		{
			if (args[0] == "-h" || args[0] == "--help" || args[0] == "help" || args[0] == "/h" || args[0] == "/help" || args[0] == "/?") 
			{
				Console.WriteLine("args[0] - port (default is 8000)\nargs[1] - server name (optional)");
				return;
			}
		}

		int port = (args.Length >= 1) ? Convert.ToInt32(args[0]) : 8000;
		string name = (args.Length >= 2) ? args[1] : null;

		HTTPServer server = new HTTPServer(WebTools.GetLocalIP(), port, new HTTPServer.ResponseMethod(HTTPServer.Response.GenerateHTTPResponse), (name == null) ? "HTTP Server" : name);
		Console.Title = server.Name;
		server.Start();
	}
}