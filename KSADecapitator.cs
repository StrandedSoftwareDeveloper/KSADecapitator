namespace KSADecapitator;
using StarMap.API;
using HarmonyLib;
using System.Reflection;
using Brutal.GlfwApi;

[StarMapMod]
public class KSADecapitator {
    [StarMapBeforeMain]
    public void preMain() {
        Console.WriteLine("[KSADecapitator]: Hi from before main!");
        DecapPatches.Patch();

    }
}

[HarmonyPatch]
public class DecapPatches {
    private static Harmony harmony = new Harmony("KSADecapitatorPrimary");

    public static void Patch() {
        Console.WriteLine("Patching...");
        //harmony.PatchAll(typeof(DecapPatches).Assembly);
        harmony.PatchAllUncategorized(typeof(DecapPatches).Assembly);

        /*var initOriginal = (MethodBase)(typeof(Brutal.GlfwApi.GlfwWindowCloseCallback).GetMember(".ctor", AccessTools.all)[0]);
        Console.WriteLine(initOriginal);
        var initPatchPrefix = typeof(DecapPatches).GetMethod(nameof(WindowCloseCallbackPatch));
        harmony.Patch(initOriginal, new HarmonyMethod(initPatchPrefix));

        Console.WriteLine("Done patching!");*/

        /*var initOriginal = AccessTools.Method(typeof(Brutal.GlfwApi.Glfw), nameof(Brutal.GlfwApi.Glfw.Init)); // if possible use nameof() here
        var initPatchPrefix = typeof(GlfwInitPatch).GetMethod(nameof(InitPatch));
        //var mainPostfix = typeof(MainPatch).GetMethod("Postfix");
        harmony.Patch(initOriginal, new HarmonyMethod(initPatchPrefix));*/
    }

    public static void Unload() {
        harmony.UnpatchAll(harmony?.Id);
    }

    [HarmonyPatch(typeof(Brutal.GlfwApi.Glfw), nameof(Brutal.GlfwApi.Glfw.Init))]
    [HarmonyPrefix]
    public static bool InitPatch(ref bool __result) {
        Console.WriteLine("Hi from GlfwInit");
        //PostNativeLoadPatches.Patch();
        __result = true;
        return false;
    }

    [HarmonyPatch(typeof(Brutal.GlfwApi.Glfw), "get_PrimaryMonitor")]
    [HarmonyPrefix]
    public static bool Get_PrimaryMonitorPatch(ref Brutal.GlfwApi.GlfwMonitor __result) {
        Console.WriteLine("Hi from get_PrimaryMonitor");
        //__result.
        return false;
    }

    [HarmonyPatch(typeof(Brutal.GlfwApi.GlfwMonitor), "get_VideoMode")]
    [HarmonyPrefix]
    public static bool Get_VidModePatch(ref Brutal.GlfwApi.GlfwVidMode __result) {
        Console.WriteLine("Hi from VideoMode");
        __result.Width = 1920;
        __result.Height = 1080;
        __result.RedBits = 8;
        __result.GreenBits = 8;
        __result.BlueBits = 8;
        __result.RefreshRate = 60;
        return false;
    }

    [HarmonyPatch(typeof(Brutal.GlfwApi.Glfw), nameof(Brutal.GlfwApi.Glfw.WindowHint))]
    [HarmonyPatch(new Type[] { typeof(Brutal.GlfwApi.GlfwWindowHint), typeof(int) })]
    [HarmonyPrefix]
    public static bool WindowHintPatch(Brutal.GlfwApi.GlfwWindowHint hint, int value) {
        Console.WriteLine("Hi from GlfwWindowHint");
        return false;
    }

    [HarmonyPatch(typeof(Brutal.GlfwApi.Glfw), nameof(Brutal.GlfwApi.Glfw.CreateWindow))]
    [HarmonyPrefix]
    public static bool CreateWindowPatch(ref Brutal.GlfwApi.GlfwWindow __result) {
        Console.WriteLine("Hi from GlfwCreateWindow");
        //__result = ;
        return false;
    }

    [HarmonyPatch(typeof(Brutal.GlfwApi.Glfw), "add_JoystickCallback")]
    [HarmonyPrefix]
    public static bool Add_JoystickCallbackPatch() {
        Console.WriteLine("Hi from add_JoystickCallback");
        //__result = ;
        return false;
    }

    [HarmonyPatch(typeof(Brutal.GlfwApi.GlfwMonitor), nameof(Brutal.GlfwApi.GlfwMonitor.GetMonitorWorkArea))]
    [HarmonyPrefix]
    public static bool GetMonitorWorkAreaPatch() {
        Console.WriteLine("Hi from GetMonitorWorkArea");
        return false;
    }

    [HarmonyPatch(typeof(KSA.GameSettings), nameof(KSA.GameSettings.ApplyTo))]
    [HarmonyPatch(new Type[] { typeof(Brutal.GlfwApi.GlfwWindow) })]
    [HarmonyPrefix]
    public static bool GameSettingsApplyTo(GlfwWindow inWindow) {
        Console.WriteLine("Hi from GameSettingsApplyTo");
        return false;
    }

    //[HarmonyPatch(typeof(Brutal.GlfwApi.GlfwWindow), "add_OnClose")]
    //[HarmonyPrefix]
    public static bool Add_OnClosePatch() {
        Console.WriteLine("Hi from add_OnClose");
        return true;
    }
}

//[HarmonyPatch]
//[HarmonyPatchCategory("postNativeLoad")]
public class PostNativeLoadPatches {
    private static Harmony harmony = new Harmony("KSADecapitatorSecondary");
    public static void Patch() {
        Console.WriteLine("Patching post-native-load patches...");
        var initOriginal = (MethodBase)(typeof(Brutal.GlfwApi.GlfwWindowCloseCallback).GetMember(".ctor", AccessTools.all)[0]);
        Console.WriteLine(initOriginal);
        var initPatchPrefix = typeof(PostNativeLoadPatches).GetMethod(nameof(WindowCloseCallbackPatch));
        //var mainPostfix = typeof(MainPatch).GetMethod("Postfix");
        //harmony.Patch(initOriginal, new HarmonyMethod(initPatchPrefix));
        //harmony.PatchAll(typeof(PostNativeLoadPatches).Assembly);
    }

    public static void Unload() {
        harmony.UnpatchAll(harmony.Id);
    }

    //[HarmonyPatch(typeof(Brutal.GlfwApi.GlfwWindowCloseCallback), ".ctor")]
    //[HarmonyPrefix]
    public static bool WindowCloseCallbackPatch() {
        Console.WriteLine("Hi from WindowCloseCallbackPatch");
        return true;
    }
}
