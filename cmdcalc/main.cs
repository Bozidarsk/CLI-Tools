using System;
using System.Collections;
using System.Collections.Generic;

static class Math2 
{
    public static float PI { get { return 3.1415926535897932384626433832795f; } }
    public static float TAU { get { return 6.283185307179586476925286766559f; } }
    public static float E { get { return 2.7182818284590452353602874713527f; } }
    public static float goldenRatio { get { return 1.6180339887498948482045868343657f; } }
    public static float DEG2RAD { get { return 0.01745329251994329576923690768489f; } }
    public static float RAD2DEG { get { return 57.295779513082320876798154814105f; } }

    public static float e(float x) { return (float)Math.Pow(E, x); }
    public static float Sqrt(float x) { return (float)Math.Sqrt((double)x); }
    public static float Abs(float x) { return (x < 0f) ? x * -1f : x; }
    public static float Ceiling(float x) { return (float)Math.Ceiling((double)x); }
    public static float Floor(float x) { return (float)Math.Floor((double)x); }
    public static float Max(float a, float b) { return (a > b) ? a : b; }
    public static float Min(float a, float b) { return (a < b) ? a : b; }
    public static float Sin(float x) { return (float)Math.Sin((double)x); }
    public static float Cos(float x) { return (float)Math.Cos((double)x); }
    public static float Tan(float x) { return (float)Math.Tan((double)x); }
    public static float Asin(float x) { return (float)Math.Asin((double)x); }
    public static float Acos(float x) { return (float)Math.Acos((double)x); }
    public static float Atan(float x) { return (float)Math.Atan((double)x); }
    public static float Atan2(float y, float x) { return (float)Math.Atan2((double)y, (double)x); }
    public static float Log(float d) { return (float)Math.Log((double)d); }
    public static float Log(float a, float x) { return (float)Math.Log((double)a, (double)x); }
    public static float Clamp(float a, float b, float x) { return Math2.Max(a, Math2.Min(x, b)); }
    public static float InverseLerp(float a, float b, float x) { return (x - a) / (b - a); }
    public static float Lerp(float a, float b, float x) { return a + (x * (b - a)); }
    public static float Pow(float a, float b) { return (float)Math.Pow((double)a, (double)b); }

    public static long Fact(int x) 
    {
        long newX = 1;
        while (x > 0) 
        {
            newX *= x;
            x--;
        }

        return newX;
    }
}

class main 
{
	static char trigType = 'r';
	static char[] allowedChars = { '.', ',', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '^', '√', '*', '/', '+', '-', '(', ')', '!', '%' };
	static char[] operators = { '^', '*', '/', '+', '-', '%', '√' };
	static string[] functions = { "&(", "sin(", "cos(", "tan(", "asin(", "acos(", "atan(", "sqrt(", "abs(", "ceiling(", "floor(", "min(", "max(", "lerp(", "inverselerp(", "log(", "clamp(" }; // '&' brackets are calculated as function, it just needs a name
	static string doc = "help - Displays this message.\nsin(x) - Returns the sine of angle 'x'.\nasin(x) - Returns the angle whose sine is 'x'.\ncos(x) - Returns the cosine of angle 'x'.\nacos(x) - Returns the angle whose cosine is 'x'.\ntan(x) - Returns the tangent of angle 'x'.\natan(x) - Returns the angle whose tangent is 'x'.\nsqrt(x) - Returns the square root of 'x'.\nabs(x) - Returns the absoulte value of 'x'.\nceiling(x) - Returns the smallest integral value less than or equal to 'x'.\nfloor(x) - Returns the largest integral value less than or equal to 'x'.\nmin(a, b) - Returns the smallest from 'a' and 'b'.\nmax(a, b) - Returns the largest from 'a' and 'b'.\nlerp(a, b, x) - Returns linear interpolation of 'a' and 'b' based on weight 'x'.\ninverselerp(a, b, x) - Returns inverse linear interpolation.\nlog(x) - Returns the natural (base e) logarithm of 'x'.\nlog(x, newBase) - Returns the logarithm of 'x' in a specified base.\nclamp(min, max, x) - Returns 'x' clamped to the inclusive range of min and max.";
	static string ERRMSG = "Invalid input.";

	static float itof(int x) { return float.Parse(System.Convert.ToString(x)); }
	static int ftoi(float x) { return System.Convert.ToInt32(x); }
	static float stof(string x) { return float.Parse(x); }
	static string ftos(float x) { return System.Convert.ToString(x); }
	static int stoi(string x) { return System.Convert.ToInt32(x); }
	static string itos(int x) { return System.Convert.ToString(x); }

	static void Error(string output) { Console.Write(ERRMSG + " (" + output + ")" + "\nPress any key to exit. "); Console.ReadKey(); Console.WriteLine(""); Environment.Exit(1); }

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

    static int CountChar(string main, char target) 
	{
		int i = 0;
		int count = 0;
		while (i < main.Length)
		{
		    if (main[i] == target) { count++; }
		    i++;
		}

		return count;
	}

	static string FormatNumber(string number) 
	{
		string result = "";

		if (!number.Contains("!") && !number.Contains("√")) { return number; }
    	else 
    	{
    		if (number.Contains("!")) 
    		{
    			if (number[number.Length - 1] != '!') { Error("invalid position of '!' -> '" + number + "'"); }
    			number = number.Remove(number.Length - 1, 1);
    			result = Math2.Fact(stoi(number)).ToString();
    		}

    		if (number.Contains("√")) 
    		{
    			if (number[0] != '√') { Error("invalid position of '√' -> '" + number + "'"); }
    			if (number[0] == '-') { Error("negative number under sqrt -> '√" + number + "'"); }
    			number = number.Remove(0, 1);
    			result = ftos(Math2.Sqrt(stof(number)));
    		}
    	}

    	return result;
	}

    static string Calculate(string content) 
    {
    	if (content.Contains("(")) { content = CheckForFunctions(content); }

    	List<string> _numbers = new List<string>();
    	List<char> _operators = new List<char>();
    	float result = 0f;
    	string number = "";
    	int i = 0;

    	while (i < content.Length) 
    	{
    		if (Array.IndexOf(operators, content[i]) < 0 || ((content[i] == '√' || content[i] == '-') && (i == 0 || Array.IndexOf(operators, content[i - 1]) >= 0))) 
    		{
    			number += content[i];
    			if (i + 1 >= content.Length) { _numbers.Add(number); }
			}
    		else 
    		{
    			_numbers.Add(number);
    			_operators.Add(content[i]);
    			number = "";
    		}

    		i++;
    	}

    	if (_numbers.Count - 1 != _operators.Count && _numbers.Count > 1) { Error("'_numbers.Count != _operators.Count - 1' -> '" + content + "'"); }

    	result = stof(FormatNumber(_numbers[0]));

    	i = 1;
    	while (i < _numbers.Count) 
    	{
    		switch (_operators[i - 1]) 
    		{
    			case '^':
    				_numbers[i] = FormatNumber(_numbers[i]);
    				result = Math2.Pow(result, stof(_numbers[i]));
    				break;
    			case '√':
    				_numbers[i] = FormatNumber(_numbers[i]);
    				if(_numbers[i][0] == '-') { Error("negative number under root -> '" + ftos(result) + "√" + _numbers[i] + "'"); }
    				result = Math2.Pow(stof(_numbers[i]), 1f / result);
    				break;
    			case '*':
    				_numbers[i] = FormatNumber(_numbers[i]);
    				result *= stof(_numbers[i]);
    				break;
    			case '/':
    				_numbers[i] = FormatNumber(_numbers[i]);
    				if (stof(_numbers[i]) == 0f) { Error("dividing by zero -> '" + ftos(result) + "/" + _numbers[i] + "'"); }
    				result /= stof(_numbers[i]);
    				break;
    			case '+':
    				_numbers[i] = FormatNumber(_numbers[i]);
    				result += stof(_numbers[i]);
    				break;
    			case '-':
    				_numbers[i] = FormatNumber(_numbers[i]);
    				result -= stof(_numbers[i]);
    				break;
    			case '%':
    				_numbers[i] = FormatNumber(_numbers[i]);
    				if (stof(_numbers[i]) == 0f) { Error("dividing by zero -> '" + ftos(result) + "/" + _numbers[i] + "'"); }
    				result = result % stof(_numbers[i]);
    				break;
    		}

    		i++;
    	}

    	return ftos(result);
    }

	static string CalculateFunction(string content, string type) 
	{
		string[] args = content.Split(',');
		float calculated = stof(Calculate(args[0]));
		switch (type) 
		{
			case "&(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				return ftos(calculated);
			case "sin(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				if (trigType == 'd') { return ftos(Math2.Sin(calculated * Math2.DEG2RAD) * Math2.RAD2DEG); }
				else { return ftos(Math2.Sin(calculated)); }
			case "cos(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				if (trigType == 'd') { return ftos(Math2.Cos(calculated * Math2.DEG2RAD) * Math2.RAD2DEG); }
				else { return ftos(Math2.Cos(calculated)); }
			case "tan(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				if (trigType == 'd') { return ftos(Math2.Tan(calculated * Math2.DEG2RAD) * Math2.RAD2DEG); }
				else { return ftos(Math2.Tan(calculated)); }
			case "asin(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				if (trigType == 'd') { return ftos(Math2.Asin(calculated * Math2.DEG2RAD) * Math2.RAD2DEG); }
				else { return ftos(Math2.Asin(calculated)); }
			case "acos(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				if (trigType == 'd') { return ftos(Math2.Acos(calculated * Math2.DEG2RAD) * Math2.RAD2DEG); }
				else { return ftos(Math2.Acos(calculated)); }
			case "atan(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				if (trigType == 'd') { return ftos(Math2.Atan(calculated * Math2.DEG2RAD) * Math2.RAD2DEG); }
				else { return ftos(Math2.Atan(calculated)); }
			case "sqrt(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				if (calculated < 0f) { Error("negative number under sqrt -> 'sqrt(" + content + ")' -> 'sqrt(" + ftos(calculated) + ")'"); }
				return ftos(Math2.Sqrt(calculated));
			case "abs(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				return ftos(Math2.Abs(calculated));
			case "ceiling(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				return ftos(Math2.Ceiling(calculated));
			case "floor(":
				if (args.Length != 1) { Error("invalid argument -> '" + type + content + ")'"); }
				return ftos(Math2.Floor(calculated));
			case "min(":
				if (args.Length != 2) { Error("invalid arguments -> '" + type + content + ")'"); }
				return ftos(Math2.Min(calculated, stof(Calculate(args[1]))));
			case "max(":
				if (args.Length != 2) { Error("invalid arguments -> '" + type + content + ")'"); }
				return ftos(Math2.Max(calculated, stof(Calculate(args[1]))));
			case "lerp(":
				if (args.Length != 3) { Error("invalid arguments -> '" + type + content + ")'"); }
				return ftos(Math2.Lerp(calculated, stof(Calculate(args[1])), stof(Calculate(args[2]))));
			case "inverselerp(":
				if (args.Length != 3) { Error("invalid arguments -> '" + type + content + ")'"); }
				return ftos(Math2.InverseLerp(calculated, stof(Calculate(args[1])), stof(Calculate(args[2]))));
			case "clamp(":
				if (args.Length != 3) { Error("invalid arguments -> '" + type + content + ")'"); }
				return ftos(Math2.Clamp(calculated, stof(Calculate(args[1])), stof(Calculate(args[2]))));
			case "log(":
				switch (args.Length) 
				{
					case 1:
						return ftos(Math2.Log(calculated));
					case 2:
						return ftos(Math2.Log(calculated, stof(Calculate(args[1]))));
					default:
						return content;
				}
		}

		return content;
	}

	static string CheckForFunctions(string main) 
	{
		if (!main.Contains("(")) { return main; }

		int i = 0;
		int t = 0;
		int count = 0;
		string content = "";
		while (i < functions.Length) 
		{
			while (main.Contains(functions[i])) 
			{
				count = 1;
				content = "";
				t = main.IndexOf(functions[i]) + functions[i].Length;
				while (t < main.Length) 
				{
					if (main[t] == '(') { count++; }
					if (main[t] == ')') { count--; }
					if (count == 0) { break; }
					content += main[t];
					t++;
				}

				content = (CountChar(content, '(') - CountChar(content, ')') != 0 || (!content.Contains("(") && content[content.Length - 1] == ')')) ? content.Remove(content.Length - 1, 1) : content;
				main = main.Replace(functions[i] + content + ")", CalculateFunction(content, functions[i]));
			}

			i++;
		}

		return main;
	}

	static string FormatBrackets(string main) 
	{
		int i = 0;
		while (i < main.Length) 
		{
			if (main[i] == '(' && Array.IndexOf(allowedChars, main[(i == 0) ? i : i - 1]) >= 0) 
			{
				main = GetStringAt(main, 0, i - 1) + "&" + GetStringAt(main, i, main.Length - 1);
			}

			if (main[i] == '|') 
			{
				// how?
			}

			i++;
		}

		return main;
	}

	static string ReplaceConstants(string main) 
	{
		if (main.Contains("π")) { main = main.Replace("π", ftos(Math2.PI)); }
		if (main.Contains("pi")) { main = main.Replace("pi", ftos(Math2.PI)); }

		int i = 0;
		while (i < main.Length) 
		{
			if (main[i] == 'e') 
			{
				if (Array.IndexOf(allowedChars, main[(i - 1 < 0) ? 1 : i - 1]) >= 0 && Array.IndexOf(allowedChars, main[(i + 1 >= main.Length) ? main.Length - 2 : i + 1]) >= 0) 
				{
					main = ((i - 1 < 0) ? "" : GetStringAt(main, 0, i - 1)) + ftos(Math2.E) + ((i + 1 >= main.Length) ? "" : GetStringAt(main, i + 1, main.Length - 1));
				}
			}

			i++;
		}

		return main;
	}

	static string Format(string main) 
	{
		if (main == "" || main == " " || CountChar(main, ' ') == main.Length) { Error("empty input"); }
		if (main.Contains("&")) { Error("'&' is not allowed -> " + itos(main.IndexOf("&"))); }
		if (CountChar(main, '(') != CountChar(main, ')')) { Error("'(' or ')'"); }
		if (CountChar(main, '|') % 2 != 0) { Error("'|'"); }

		main = ReplaceConstants(main);
		main = FormatBrackets(main);
		main = CheckForFunctions(main);

		int i = 0;
		while (i < main.Length) 
		{
			if (Array.IndexOf(allowedChars, main[i]) < 0) { Error("'" + System.Convert.ToString(main[i]) + "' is not allowed -> " + itos(main.IndexOf(main[i]))); }
			i++;
		}

		return main;
	}

	static void Main(string[] args) 
	{
		if (args.Length < 1) { return; }
		if (Array.IndexOf(args, "-h") >= 0 || Array.IndexOf(args, "/?") >= 0 || Array.IndexOf(args, "--help") >= 0 || Array.IndexOf(args, "/help") >= 0 || Array.IndexOf(args, "-help") >= 0 || Array.IndexOf(args, "/h") >= 0 || Array.IndexOf(args, "-?") >= 0 || Array.IndexOf(args, "help") >= 0) { Console.WriteLine(doc); if (args.Length == 1) { Environment.Exit(0); } }
		if (Array.IndexOf(args, "--deg") >= 0 || Array.IndexOf(args, "-d") >= 0 || Array.IndexOf(args, "-deg") >= 0 || Array.IndexOf(args, "/deg") >= 0 || Array.IndexOf(args, "/d") >= 0) { trigType = 'd'; }
		if (Array.IndexOf(args, "--rad") >= 0 || Array.IndexOf(args, "-r") >= 0 || Array.IndexOf(args, "-rad") >= 0 || Array.IndexOf(args, "/rad") >= 0 || Array.IndexOf(args, "/r") >= 0) { trigType = 'r'; }

		args[0] = args[0].Replace(" ", "").ToLower();
		Console.WriteLine(Calculate(Format(args[0])));
	}
}