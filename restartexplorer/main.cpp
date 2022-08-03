#define TITLE " explorer.exe"
#define CONTENT "Restart explorer.exe?"

#include <windows.h>
#include <stdlib.h>

int WinMain(HINSTANCE hInstance,
            HINSTANCE hPrevInstance, 
            LPTSTR    lpCmdLine, 
            int       cmdShow) 
{
	::ShowWindow(::GetConsoleWindow(), SW_HIDE);
	int button = MessageBox(NULL, CONTENT, TITLE, MB_ICONQUESTION | MB_YESNO | MB_DEFBUTTON2);

	if (button == 6) 
	{
		system("cd C:\\Windows & taskkill /f /im explorer.exe & start explorer.exe");
	}

	return 0;
}