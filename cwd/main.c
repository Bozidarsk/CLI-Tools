#include <stdio.h>
#include <unistd.h>
#include <limits.h>

int main() 
{
	char dir[PATH_MAX];

	if (getcwd(dir, sizeof(dir)) != NULL) { printf("%s", dir); }
	else { printf(""); return 1; }

	return 0;
}