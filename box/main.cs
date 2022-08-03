using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

class main 
{
	[DllImport("user32.dll")]
	static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

	static void Main(string[] args) 
	{
		ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);

		int argc = args.Length;
		string content = args[0];
		string title = args[1];
		MessageBoxButtons buttons = MessageBoxButtons.OK;
		MessageBoxIcon icon = MessageBoxIcon.Information;
		MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1;

		if (args[2] == "abortretryignore" || args[2] == "\"abort retry ignore\"") 
		{
			buttons = MessageBoxButtons.AbortRetryIgnore;
		}
		/*if (args[2] == "cancletrycontinue" || args[2] == "\"cancle try continue\"") 
		{
			buttons = MessageBoxButtons.CancelTryContinue;
		}*/
		if (args[2] == "ok" || args[2] == "\"ok\"") 
		{
			buttons = MessageBoxButtons.OK;
		}
		if (args[2] == "okcancel" || args[2] == "\"ok cancle\"") 
		{
			buttons = MessageBoxButtons.OKCancel;
		}
		if (args[2] == "retrycancel" || args[2] == "\"retry cancle\"") 
		{
			buttons = MessageBoxButtons.RetryCancel;
		}
		if (args[2] == "yesno" || args[2] == "\"yes no\"") 
		{
			buttons = MessageBoxButtons.YesNo;
		}
		if (args[2] == "yesnocancel" || args[2] == "\"yes no cancle\"") 
		{
			buttons = MessageBoxButtons.YesNoCancel;
		}


		if (args[3] == "question") 
		{
			icon = MessageBoxIcon.Question;
		}
		if (args[3] == "warning") 
		{
			icon = MessageBoxIcon.Warning;
		}
		if (args[3] == "information" || args[4] == "info") 
		{
			icon = MessageBoxIcon.Information;
		}
		if (args[3] == "error") 
		{
			icon = MessageBoxIcon.Error;
		}


		if (args[4] == "1") 
		{
			defaultButton = MessageBoxDefaultButton.Button1;
		}
		if (args[4] == "2") 
		{
			defaultButton = MessageBoxDefaultButton.Button2;
		}
		if (args[4] == "3") 
		{
			defaultButton = MessageBoxDefaultButton.Button3;
		}

		DialogResult result;
		result = MessageBox.Show(args[0], args[1], buttons, icon, defaultButton);

		if (result == DialogResult.Abort) { Environment.ExitCode = 3; }
		if (result == DialogResult.Cancel) { Environment.ExitCode = 2; }
		//if (result == DialogResult.Continue) { Environment.ExitCode = 11; }
		if (result == DialogResult.Ignore) { Environment.ExitCode = 5; }
		if (result == DialogResult.No) { Environment.ExitCode = 7; }
		if (result == DialogResult.OK) { Environment.ExitCode = 1; }
		if (result == DialogResult.Retry) { Environment.ExitCode = 10; }
		//if (result == DialogResult.TryAgain) { Environment.ExitCode = 4; }
		if (result == DialogResult.Yes) { Environment.ExitCode = 6; }
	}
}