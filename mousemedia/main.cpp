#define pauseKey 0xb3
#define previousKey 0xb1
#define nextKey 0xb0
#define stopKey 0xb2
#define leftAltKey 0xA4

#include <iostream>
#include <windows.h>
#include <stdio.h>

using namespace std;

HHOOK keyboardHook;
HHOOK mouseHook;

int keyboardKey = 0;
int mouseKey = 0;
int mouseKey_wParam = 512;

void CheckKeys() 
{
    HKL kbl = GetKeyboardLayout(0);
    INPUT input;
    input.type = INPUT_KEYBOARD;
    input.ki.wScan = 0;
    input.ki.time = 0;
    input.ki.dwExtraInfo = 0;
    input.ki.dwFlags = 0;

    if (keyboardKey == 164 && mouseKey_wParam == 519) 
    {        
        input.ki.wVk = pauseKey;
        SendInput(1, &input, sizeof(INPUT));
    }

    if (keyboardKey == 164 && mouseKey == 65536) 
    {
        input.ki.wVk = nextKey;
        SendInput(1, &input, sizeof(INPUT));
    }

    if (keyboardKey == 164 && mouseKey == 131072) 
    {
        input.ki.wVk = previousKey;
        SendInput(1, &input, sizeof(INPUT));
    }

    keyboardKey = 0;
    mouseKey = 0;
    mouseKey_wParam = 512;
}

LRESULT CALLBACK KeyboardHook(int nCode, WPARAM wParam, LPARAM lParam) 
{
    KBDLLHOOKSTRUCT keyboard = *((KBDLLHOOKSTRUCT*)lParam);
    keyboardKey = keyboard.vkCode;
    return CallNextHookEx(keyboardHook, nCode, wParam, lParam);
}

LRESULT CALLBACK MouseHook(int nCode, WPARAM wParam, LPARAM lParam)
{
     MSLLHOOKSTRUCT * mouse = (MSLLHOOKSTRUCT *)lParam;
     mouseKey = mouse->mouseData;
     mouseKey_wParam = wParam;
     if (mouseKey != 0 || mouseKey_wParam != 512) { CheckKeys(); }
     return CallNextHookEx(mouseHook, nCode, wParam, lParam);
}

void MessageLoop() 
{
    MSG message;
    while (GetMessage(&message, NULL, 0, 0)) 
    {
        TranslateMessage(&message);
        DispatchMessage(&message);
    }
}

DWORD WINAPI my_HotKey(LPVOID lpParm) 
{
    HINSTANCE hInstance = GetModuleHandle(NULL);
    if (!hInstance) hInstance = LoadLibrary((LPCSTR) lpParm); 
    if (!hInstance) return 1;
    keyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, (HOOKPROC)KeyboardHook, hInstance, NULL);
    mouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHook, hInstance, NULL);
    keyboardKey = 0;
    mouseKey = 0;
    mouseKey_wParam = 512;
    MessageLoop();
    UnhookWindowsHookEx(keyboardHook);
    UnhookWindowsHookEx(mouseHook);
    return 0;
}

int main(int argc, char** argv) 
{
    HANDLE hThread;
    DWORD dwThread;

    hThread = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)my_HotKey, (LPVOID)argv[0], NULL, &dwThread);

    ShowWindow(FindWindowA("ConsoleWindowClass", NULL), false);

    if (hThread) return WaitForSingleObject(hThread, INFINITE);
    else return 1;
}