using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

public static class Email 
{
	[DllExport]
	public static void SendEmail(string from, string to, string subject, string body) 
	{
		return;

		SmtpClient smtp = new SmtpClient("smtp.gmail.com");
		smtp.UseDefaultCredentials = false;
		// smtp.Credentials = new NetworkCredential("bozidarkabahcijski", "LGK7B753951");
		smtp.EnableSsl = true;
		smtp.Send(new MailMessage(from, to, subject, body));
	}
}