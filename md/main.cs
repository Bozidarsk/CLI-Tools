using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Markdig;

class main 
{
	static string ToHtml(string md) 
	{
		var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
		string html = Markdown.ToHtml(md, pipeline);

		html = 
			"<head>\n" + 
			"<meta name=\"color-scheme\" content=\"light dark\">" + 
			"<style>\n" + 
			File.ReadAllText("C:\\Tools\\Source\\githubmd.css") + "\n" + 
			"</style>\n" + 
			"</head>\n" + 
			"<body>\n" + 
			html + "\n" + 
			"</body>"
		;

		return html.Replace("<pre><code>", "<pre><code class=\"pre\">").Replace("<code>", "<code class=\"free\">");
	}

	static void Main(string[] args) 
	{
		if (args.Length != 1) { return; }
		if (args[0] == "help" || args[0] == "--help" || args[0] == "/help" || args[0] == "-?" || args[0] == "/?" || args[0] == "/h" || args[0] == "-h") 
		{ Console.WriteLine("This is using Markdig.\nThe error code is the hash code of args[0].\nHtml file is located in '%tmp%\\{HashCode}.md.html'.\nargs[0] - markdown file path"); return; }

		string html = ToHtml(File.ReadAllText(args[0]));
		string path = 
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
			"\\AppData\\Local\\Temp\\" + 
			args[0].GetHashCode().ToString() + 
			".md.html"
		;

		File.WriteAllText(path, ToHtml(File.ReadAllText(args[0])));
		System.Diagnostics.Process.Start(path);
		Environment.Exit(args[0].GetHashCode());
	}
}