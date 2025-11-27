namespace KSADecapitator;
using StarMap.API;
using HarmonyLib;
using System.Reflection;
using Brutal.GlfwApi;
using System.Reflection.Emit;
using KSA;
using Core;

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

        var rendererCtorOriginal = (MethodBase)(typeof(Renderer).GetMember(".ctor", AccessTools.all)[0]);
        //Console.WriteLine(rendererCtorOriginal);
        var rendererCtorPatchPrefix = typeof(DecapPatches).GetMethod(nameof(RendererCtorPatch));
        harmony.Patch(rendererCtorOriginal, new HarmonyMethod(rendererCtorPatchPrefix));

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
    public static bool GameSettingsApplyToPatch(GlfwWindow inWindow) {
        Console.WriteLine("Hi from GameSettingsApplyTo");
        return false;
    }

    [HarmonyPatch(typeof(KSA.GameSettings), nameof(KSA.GameSettings.PopulateSupportedResolutions))]
    [HarmonyPrefix]
    public static bool GameSettingsPopulateSupportedResolutionsPatch(GlfwWindow window) {
        Console.WriteLine("Hi from GameSettingsPopulateSupportedResolutions");
        return false;
    }

    public static bool RendererCtorPatch(GlfwWindow window, Brutal.VulkanApi.VkFormat depthFormat, Brutal.VulkanApi.VkPresentModeKHR presentMode, Brutal.VulkanApi.Abstractions.VulkanHelpers.Api vulkanApiVersion) {
        Console.WriteLine("The Renderer constructor.");
        return false;
    }
}

[HarmonyPatch(typeof(KSA.Program), MethodType.Constructor)]
[HarmonyPatch(new Type[] { typeof(System.Collections.Generic.IReadOnlyList<string>) })]
class ConstructorPatch {
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        Console.WriteLine("Hi from the transpiler!");

        List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
        for (int i = 0; i < codes.Count; i++) {
            if (codes[i].opcode == OpCodes.Call) {
                MethodInfo operand = codes[i].operand as MethodInfo;
                //Console.WriteLine(operand.FullDescription());
                if (operand.FullDescription().Contains("get_GetInstanceProcAddr()")) { //We need this patch, otherwise Harmony (actually MonoMod) gets mad about an "Unexpected null"
                    codes[i] = new CodeInstruction(OpCodes.Call, typeof(ConstructorPatch).GetMethod(nameof(get_GetInstanceProcAddr_replace)));
                }
            } else if (codes[i].opcode == OpCodes.Newobj) {
                ConstructorInfo operand = codes[i].operand as ConstructorInfo;
                //Console.WriteLine(operand.FullDescription());
                if (ContainsAny(operand.FullDescription(), ["GlfwWindowCloseCallback::.ctor", "GlfwKeyCallback::.ctor",
                                                            "GlfwMouseButtonCallback::.ctor", "GlfwCursorPosCallback::.ctor",
                                                            "GlfwScrollCallback::.ctor"])) {
                    Console.WriteLine("Here's the things");
                    codes[i].opcode = OpCodes.Pop;                            //Consume the first argument
                    codes.Insert(i+1, new CodeInstruction(OpCodes.Pop));      //Consume the second
                    codes.Insert(i+2, new CodeInstruction(OpCodes.Ldc_I4_0)); //Push the object reference that the function was supposed to return (now null)
                }
            } else if (codes[i].opcode == OpCodes.Callvirt) {
                MethodInfo operand = codes[i].operand as MethodInfo;
                //Console.WriteLine(operand.Name);
                if (ContainsAny(operand.Name, ["add_OnClose", "add_OnKey", "add_OnMouseButton",
                                               "add_OnCursorPos", "add_OnScroll"])) {
                    codes[i].opcode = OpCodes.Pop;
                    codes.Insert(i+1, new CodeInstruction(OpCodes.Pop));
                    codes.Insert(i+2, new CodeInstruction(OpCodes.Call, typeof(ConstructorPatch).GetMethod(nameof(printer))));
                }
            }
        }

        Console.WriteLine("Bye from the transpiler!");
        return codes.AsEnumerable();
    }

    private static bool ContainsAny(string str, string[] list) {
        for (int i=0; i<list.Length; i++) {
            if (str.Contains(list[i])) {
                return true;
            }
        }

        return false;
    }

    public static void printer() {
        Console.WriteLine("Howdy5");
    }

    public static int get_GetInstanceProcAddr_replace() {
        return 1;
    }
}

//[HarmonyPatch]
//[HarmonyPatchCategory("postNativeLoad")]
/*public class PostNativeLoadPatches {
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
}*/
