namespace KSADecapitator;
using StarMap.API;
using HarmonyLib;

[StarMapMod]
public class KSADecapitator {
    [StarMapBeforeMain]
    public void preMain() {
        Console.WriteLine("[KSADecapitator]: Hi from before main!");
        ProgramPatches.Patch();
    }
}

[HarmonyPatch]
public class ProgramPatches {
    private static Harmony harmony = new Harmony("KSADecapitator");

    public static void Patch() {
        Console.WriteLine("Patching...");
        harmony.PatchAll(typeof(ProgramPatches).Assembly);
    }

    public static void Unload() {
        harmony.UnpatchAll(harmony?.Id);
    }

    [HarmonyPatch(typeof(Brutal.GlfwApi.Glfw), nameof(Brutal.GlfwApi.Glfw.Init))]
    [HarmonyPrefix]
    public static bool initPatch(ref bool __result) {
        Console.WriteLine("Hi from GlfwInit");
        __result = true;
        return false;
    }
}
