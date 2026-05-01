//Used with `tcc -run genPrefixPatch.c $(xclip -o -selection "clipboard") | xclip -selection "clipboard"` for a tighter dev cycle

#include <stdio.h>
#include <stdint.h>
#include <string.h>
#include <stdlib.h>
#include <stdbool.h>

int main(int argc, char **argv) {
    if (argc < 2) {
        fprintf(stderr, "Usage: genPrefixPatch \"<functionPrototype(int arg1, String arg2, etc.)>\"\n");
        return -1;
    }

    size_t inputLen = strlen(argv[1]);
    char *className = calloc(inputLen, sizeof(char)); //None of these strings can be bigger than the input
    char *fullFunctionName = calloc(inputLen, sizeof(char));
    char *functionNameWithArgs = calloc(inputLen, sizeof(char));
    char *patchName = calloc(inputLen, sizeof(char));

    int numArgs = 0;
    char **argTypes = NULL;


    int openParenPos = 0;
    int lastDotPos = 0;
    for (int i=0; i<inputLen; i++) {
        if (argv[1][i] == '.') {
            lastDotPos = i;
        } else if (argv[1][i] == '(') {
            openParenPos = i;
            break;
        }
    }
    
    for (int i=0; i<lastDotPos; i++) {
        className[i] = argv[1][i];
    }
    
    for (int i=0; i<openParenPos; i++) {
        fullFunctionName[i] = argv[1][i];
    }
    
    for (int i=lastDotPos+1; i<inputLen; i++) {
        functionNameWithArgs[i-(lastDotPos+1)] = argv[1][i];
    }
    
    int patchNameIndex = 0;
    for (int i=0; i<openParenPos; i++) {
        if (argv[1][i] != '.') {
            patchName[patchNameIndex++] = argv[1][i];
        }
    }
    
    
    if (argv[1][openParenPos+1] == ')') {
        numArgs = 0;
    } else {
        numArgs = 1;
        for (int i=openParenPos+1; i<inputLen-1; i++) {
            if (argv[1][i] == ',') {
                numArgs += 1;
            }
        }
    }
    
    argTypes = calloc(numArgs, sizeof(char*));
    int index = openParenPos+1;
    for (int i=0; i<numArgs; i++) {
        argTypes[i] = calloc(inputLen, sizeof(char)); //None of these can be bigger than the input string either
        bool typeMode = true;
        for (int j=0; argv[1][index] != ',' && index < inputLen; j++) {
            //fprintf(stderr, "%d, %d, %d, %c\n", i, j, typeMode, argv[1][index]);
            if (argv[1][index] == ' ') {
                typeMode = false;
            }
            if (typeMode) {
                argTypes[i][j] = argv[1][index];
            }
            index += 1;
        }
        while (argv[1][index] == ' ' || argv[1][index] == ',') { index += 1; }
    }


    printf("[HarmonyPatch(typeof(%s), nameof(%s))]\n", className, fullFunctionName);
    printf("[HarmonyPatch(new Type[] { ");
    for (int i=0; i<numArgs; i++) {
        printf("typeof(%s)", argTypes[i]);
        if (i != numArgs-1) {
            printf(", ");
        }
    }

    printf(" })]\n");
    printf("[HarmonyPrefix]\n");
    printf("public static bool %sPfx%s {\n", patchName, argv[1]+openParenPos);
    printf("    Console.WriteLine(\"%s() prefix\");\n", fullFunctionName);
    printf("    return false;\n");
    printf("}\n");

    for (int i=0; i<numArgs; i++) {
        free(argTypes[i]);
    }
    
    free(argTypes);
    free(patchName);
    free(functionNameWithArgs);
    free(fullFunctionName);
    free(className);
    
    return 0;
}