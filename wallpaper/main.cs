using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.Win32;

class main 
{
	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

	static void SetWallpaper(string file, int style)
	{
	    // Tiled = 0
	    // Centered = 1
	    // Stretched = 2

	    string tempPath = Path.Combine(
	    	Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
			"\\AppData\\Local\\Temp\\",
			"wallpaper_" + file.GetHashCode().ToString() + ".bmp"
	    );

	    Image.FromFile(file).Save(tempPath, ImageFormat.Bmp);
	    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

	    switch (style) 
	    {
	    	case 0:
		    	key.SetValue(@"WallpaperStyle", 1.ToString());
		        key.SetValue(@"TileWallpaper", 1.ToString());
	    		break;
	    	default:
	    	case 1:
		    	key.SetValue(@"WallpaperStyle", 1.ToString());
		        key.SetValue(@"TileWallpaper", 0.ToString());
	    		break;
	    	case 2:
	    		key.SetValue(@"WallpaperStyle", 2.ToString());
		        key.SetValue(@"TileWallpaper", 0.ToString());
	    		break;
	    }

	    SystemParametersInfo(0x0014, 0, tempPath, 0x3);
	}

	static void GetWallpaper(string file) 
	{
		RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
		byte[] bytes = ObjectToArray<byte>(key.GetValue(@"TranscodedImageCache", 0x69));
		File.WriteAllBytes(file, bytes);
	}

	public static T[] ObjectToArray<T>(object obj) 
	{
		List<T> list = new List<T>();
		IEnumerable enumerable = list as IEnumerable;
		if (enumerable == null) { throw new ArgumentException("The object must implement IEnumerable."); }
		foreach (object item in enumerable) { list.Add((T)item); }
		return list.ToArray();
	}

	static void Main(string[] args) 
	{
		Console.WriteLine("Set is not working.");
		Environment.Exit(1);

		if (args.Length < 1) { return; }
		if (args[0] == "help" || args[0] == "--help" || args[0] == "/help" || args[0] == "-?" || args[0] == "/?" || args[0] == "/h" || args[0] == "-h") 
		{ Console.WriteLine("wallpaper set <inputfile> [optional](--tiled,-t | --stretched,-s | --centered,-c)\nwallpaper get <outputfile>"); return; }
		if (args.Length < 2) { return; }

		string t = null;
		string s = null;
		string c = null;

		try { t = args.Where(x => x == "--tiled" || x == "-t").ToArray()[0]; } catch {}
		try { s = args.Where(x => x == "--stretched" || x == "-s").ToArray()[0]; } catch {}
		try { c = args.Where(x => x == "--centered" || x == "-c").ToArray()[0]; } catch {}

		int style = (t != null) ? 0 : ((s != null) ? 2 : 1);

		if (args[0] == "set") { SetWallpaper(args[1], style); }
		if (args[0] == "get") { GetWallpaper(args[1]); }
	}
}