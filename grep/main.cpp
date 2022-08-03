#include <string>
#include <iostream>
#include <stdlib.h>
#include <windows.h>
#include <deque>

std::string GetStringAt(std::string main, int startPos, int endPos) 
{
    if (main == "" || startPos < 0 || endPos <= 0 || endPos < startPos || endPos >= main.length()) { return ""; }

    std::string output = "";
    int i = startPos;
    int t = 0;
    while (t < (endPos - startPos) + 1) 
    {
        output += main[i];
        i++;
        t++;
    }

    return output;
}

int IndexOf(std::string main, std::string target, int startI) 
{
	int mlen = main.length();
	int tlen = target.length();
	if (mlen <= 0 || tlen <= 0 || tlen > mlen || startI < 0 || startI >= mlen) { return -1; }

	int i = startI;
	int t = 0;
	while (i < mlen) 
	{
		t = 0;
		while (t < tlen) 
		{
			if (main[i + t] != target[t]) { break; }
			t++;
		}

		if (t >= tlen) { return i; }
		i++;
	}

	return -1;
}

bool IsNumber(std::string x) 
{
	for (int i = 0; i < x.length(); i++) { if (!(x[i] >= 0x30 && x[i] <= 0x39)) { return false; } }
	return true;
}

int ToInt(std::string x) 
{
	int result = 0;
	bool isNegative = false;

	for (int i = 0; i < x.length(); i++) 
	{
		if (x[0] == '-') { isNegative = true; continue; }
		if (!(x[i] >= 0x30 && x[i] <= 0x39)) { return result; }
		result += x[i] - 0x30;
		result *= 10;
	}

	result /= 10;
	return (isNegative) ? 0xffffffff - (result - 1) : result;
}

bool Contains(std::deque<int> positions, int position) 
{
	for (int i = 0; i < positions.size(); i++) { if (positions[i] == position) { return true; } }
	return false;
}

int main(int argc, char** argv) 
{
	int maxCount = -1;

	std::string input = "";
	HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
	std::deque<int> positions;
	bool running = true;
	bool printThis = false;

	std::string args[argc];
	for (int i = 0; i < argc; ++i) { std::string currentArg = argv[i]; args[i] = currentArg; }
	for (int i = 0; i < argc; i++) 
	{
		if (args[i] == "help" || args[i] == "--help" || args[i] == "-h" || args[i] == "/?" || args[i] == "-?") { std::cout << "grep [string0] [string1] [string...]\nChecks if the output of a program contains any of these strings.\n-n [number] - limits the max ammount of results.\n"; running = false; }
		if (args[i] == "-n") 
		{
			if (!IsNumber(args[i + 1]) || args[i + 1][0] == '-') { std::cout << "maxCount must be a positive number.\n"; return 0; }
			maxCount = ToInt(args[i + 1]);
		}
	}

	while (getline(std::cin, input)) 
	{
		for (int i = 1; i < argc && running; i++) 
		{
			int startI = 0;
			int index = IndexOf(input, args[i], 0);
			if (args[i] == "-n" || args[i - 1] == "-n") { continue; }
			if (index < 0) { printThis = false; }

			while (index >= 0) 
			{
				printThis = true;
				startI += args[i].length();
				for (int p = 0; p < args[i].length(); p++) { positions.push_back(index + p); }
				index = IndexOf(input, args[i], startI);
			}
		}

		if (printThis) { maxCount--; }
		if (maxCount == 0) { running = false; }

		for (int t = 0; t < input.length() && printThis; t++) 
		{
			SetConsoleTextAttribute(h, (Contains(positions, t)) ? 14 : 7);
			std::cout << input[t] << ((t >= input.length() - 1) ? "\n" : "");
		}

		if (maxCount == 0) { printThis = false; }
		while (positions.size() > 0) { positions.pop_back(); }
	}

	SetConsoleTextAttribute(h, 7);
	return 0;
}