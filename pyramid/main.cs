using System;
using System.Collections;
using System.Collections.Generic;

class main 
{
	static string FillString(int count, string str) 
    {
        if (count <= 0) { return ""; }

        string output = "";
        while (count > 0) 
        {
            output += str;
            count--;
        }

        return output;
    }

    static void Start(float width) 
    {
		float heigth = 0;
		int floorWidth = 0;
		int i = 0;
		int t = 0;

		if (width % 2 == 0) 
		{
			t = 2;
			heigth = width / 2f;
			floorWidth = System.Convert.ToInt32(width / 2f);
			while (i < heigth) 
			{
				Console.WriteLine(FillString(floorWidth - i - 1, " ") + FillString(t, "*"));
				t += 2;
				i++;
			}
		}
		else 
		{
			t = 1;
			heigth = (width + 1f) / 2f;
			floorWidth = System.Convert.ToInt32((width - 1f) / 2f);
			while (i < heigth) 
			{
				Console.WriteLine(FillString(floorWidth - i, " ") + FillString(t, "*"));
				t += 2;
				i++;
			}
		}

		Console.WriteLine("");
    }

	static void Main(string[] args) 
	{
		bool running = true;
		string input = "";

		if (args.Length == 1) 
		{
			Start(float.Parse(System.Convert.ToString(args[0])));
		}

		while (running) 
		{
			Console.Write("Enter pyramid size: ");
			input = Console.ReadLine();

			if (input == "exit") { running = false; break; }
			else { Start(float.Parse(input)); }
		}
	}
}