using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;

public static class Email 
{
	// [RGiesecke.DllExport.DllExport]
	[Obsolete("Not working.")]
	public static void SendEmail(string password, string from, string to, string subject, string body, string smtp = "smtp.gmail.com", int port = 587) 
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		SmtpClient client = new SmtpClient(smtp, port);
		client.EnableSsl = true;
		client.Credentials = new NetworkCredential(from, password);
		client.Send(new MailMessage(from, to, subject, body));
	}
}