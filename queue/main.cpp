#include <string>
#include <windows.h>

int main() 
{
	std::string arg = "printui.dll,PrintUIEntry /o /n \"Canon TS6200 series\"";
	ShellExecute(NULL, "open", "C:\\Windows\\System32\\rundll32.exe", arg.c_str(), NULL, SW_SHOWDEFAULT);
	return 0;
}