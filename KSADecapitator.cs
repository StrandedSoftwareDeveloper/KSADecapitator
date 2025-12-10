namespace KSADecapitator;
using StarMap.API;
using HarmonyLib;
using System.Reflection;
using Brutal.GlfwApi;
using System.Reflection.Emit;
using KSA;
using Core;
using RenderCore;
//using MonoMod.RuntimeDetour;

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
    //private static Hook _createSamplerHook;

    public static void Patch() {
        Console.WriteLine("Patching...");
        //harmony.PatchAll(typeof(DecapPatches).Assembly);

        //Harmony.DEBUG = true; 

        /*var t = AccessTools.TypeByName("MonoMod.Switches");
        var m = AccessTools.Method(t, "SetSwitchValue");
        m.Invoke(null, ["DMDType", "cecil"]);*/

        harmony.PatchAllUncategorized(typeof(DecapPatches).Assembly);

        var rendererCtorOriginal = (MethodBase)(typeof(Renderer).GetMember(".ctor", AccessTools.all)[0]);
        //Console.WriteLine(rendererCtorOriginal);
        var rendererCtorPatchPrefix = typeof(DecapPatches).GetMethod(nameof(RendererCtorPatch));
        harmony.Patch(rendererCtorOriginal, new HarmonyMethod(rendererCtorPatchPrefix));

        /*var createSamplerOrig = (MethodBase)(typeof(Brutal.VulkanApi.VkDeviceExtensions).GetMember(nameof(Brutal.VulkanApi.VkDeviceExtensions.CreateSampler), AccessTools.all)[0]);
        var createSamplerPatch = typeof(DecapPatches).GetMethod(nameof(VkDeviceExtensionsCreateSamplerPatch));
        Console.WriteLine("Making the RuntimeDetour...");
        var _createSamplerHook = new Hook(createSamplerOrig, createSamplerPatch);
        Console.WriteLine("Done with that");*/
        
        /*var createSampleOrig = (MethodBase)(typeof(Brutal.VulkanApi.VkDeviceExtensions).GetMember(nameof(Brutal.VulkanApi.VkDeviceExtensions.CreateSampler), AccessTools.all)[0]);
        Console.WriteLine(createSampleOrig);
        var createSamplePatch = typeof(DecapPatches).GetMethod(nameof(VkDeviceExtensionsCreateSamplerPatch));
        harmony.Patch(createSampleOrig, new HarmonyMethod(createSamplePatch));*/

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

    public static bool RendererCtorPatch(ref Renderer __instance, GlfwWindow window, Brutal.VulkanApi.VkFormat depthFormat, Brutal.VulkanApi.VkPresentModeKHR presentMode, Brutal.VulkanApi.Abstractions.VulkanHelpers.Api vulkanApiVersion) {
        Console.WriteLine("The Renderer constructor.");
        __instance.FrameCount = 0;
        return false;
    }

    [HarmonyPatch(typeof(KSA.Rendering.CommonBufferValues), nameof(KSA.Rendering.CommonBufferValues.Init))]
    [HarmonyPrefix]
    public static bool CommonBufferValuesInitPatch(Renderer renderer) {
        Console.WriteLine("Hi from CommonBufferValuesInit");
        return false;
    }

    [HarmonyPatch(typeof(RenderCore.Systems.RenderGlobals), MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(IVulkanContext) })]
    [HarmonyPrefix]
    public static bool RenderCoreSystemsRenderGlobalsCtor(IVulkanContext vk) {
        Console.WriteLine("RenderCore.Systems.RenderGlobals..ctor patch");
        return false;
    }

    [HarmonyPatch(typeof(RenderCore.Systems.BindlessTextureLibrary), MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(IVulkanContext), typeof(int) })]
    [HarmonyPrefix]
    public static bool RenderCoreSystemsBindlessTextureLibCtor(IVulkanContext ctx, int maxTextures) {
        Console.WriteLine("RenderCore.Systems.BindlessTextureLibrary..ctor patch");
        return false;
    }

    [HarmonyPatch(typeof(GpuTextureSystem), MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(IGlobalRenderSystem), typeof(RenderCore.Systems.BindlessTextureLibrary) })]
    [HarmonyPrefix]
    public static bool GpuTextureSystemCtor(IGlobalRenderSystem rs, RenderCore.Systems.BindlessTextureLibrary bindlessTextureLib) {
        Console.WriteLine("GpuTextureSystem..ctor patch");
        return false;
    }

    [HarmonyPatch(typeof(Core.KSADeviceContextEx), "get_Device")]
    [HarmonyPrefix]
    public static bool KSADeviceContextEx_get_DevicePatch(ref Brutal.VulkanApi.Abstractions.DeviceEx __result) {
        Console.WriteLine("Hi from KSADeviceContextEx_get_Device");
        __result = (Brutal.VulkanApi.Abstractions.DeviceEx)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(Brutal.VulkanApi.Abstractions.DeviceEx));
        GC.SuppressFinalize(__result); //Might be a memory leak, but the program crashes when the GC tries to run the Finalizer, so...
        return false;
    }

    [HarmonyPatch(typeof(GpuTextureSystem), nameof(GpuTextureSystem.AddSampler))]
    [HarmonyPrefix]
    public static bool GpuTextureSystemAddSamplerPatch() {
        Console.WriteLine("GpuTextureSystemAddSampler patch");
        return false;
    }

    [HarmonyPatch(typeof(KSA.ImGuiBackend), nameof(KSA.ImGuiBackend.Initialize))]
    [HarmonyPrefix]
    public static bool ImGuiBackendInitPatch() {
        Console.WriteLine("ImGuiBackendInit patch");
        return false;
    }

    [HarmonyPatch(typeof(KSA.Loading), nameof(KSA.Loading.OnFrame))]
    [HarmonyPrefix]
    public static bool LoadingOnFramePatch() {
        //Console.WriteLine("LoadingOnFrame patch");
        return false;
    }

    [HarmonyPatch(typeof(KSA.SelectSystem), nameof(KSA.SelectSystem.OnFrame))]
    [HarmonyPrefix]
    public static bool SelectSystemOnFramePatch(ref KSA.SelectSystem __instance) {
        Console.WriteLine("SelectSystemOnFrame patch\n");
        Console.WriteLine("Please select a system:");
        for (int i = 0; i < KSA.SelectSystem.Systems.Count; i++) {
            KSA.SystemInfo systemInfo = KSA.SelectSystem.Systems[i];
            Console.WriteLine("(" + i + ") " + systemInfo.DisplayName);
        }

        Console.Write("> ");
        string? systemSelection = Console.ReadLine();
        if (systemSelection == null || systemSelection.Length == 0) {
            return false;
        }

        int systemNum;
        if (!int.TryParse(systemSelection, out systemNum)) {
            return false;
        }

        if (systemNum < 0 || systemNum >= KSA.SelectSystem.Systems.Count) {
            return false;
        }

        KSA.SystemInfo selectedSystemInfo = KSA.SelectSystem.Systems[systemNum];
        Console.WriteLine("Selected \"" + selectedSystemInfo.DisplayName + "\"");
        KSA.SystemLibrary.Default = selectedSystemInfo;

        KSA.ConfigOnStartPopup popup = (KSA.ConfigOnStartPopup)Traverse.Create(__instance).Field("<ConfigOnStartPopup>k__BackingField").GetValue();
        popup.Active = false;

        //JankDebugger.stepMode = true;
        return false;
    }

    [HarmonyPatch(typeof(KSA.ShaderReference), "DoLoad")]
    [HarmonyPrefix]
    public static bool ShaderRefDoLoadPrefix() {
        Console.WriteLine("ShaderRefDoLoad prefix");
        return false;
    }

    [HarmonyPatch(typeof(KSA.Viewport), nameof(KSA.Viewport.BuildRenderTarget))]
    [HarmonyPrefix]
    public static bool ViewportBuildRenderTargetPrefix() {
        Console.WriteLine("ViewportBuildRenderTarget prefix");
        return false;
    }

    [HarmonyPatch(typeof(KSA.Program), "BuildRenderTargets")]
    [HarmonyPrefix]
    public static bool ProgramBuildRenderTargetsPrefix() {  
        Console.WriteLine("ProgramBuildRenderTargets prefix");
        JankDebugger.stepMode = true;
        return false;
    }

    [HarmonyPatch(typeof(KSA.ModLibrary), nameof(KSA.ModLibrary.Bind))]
    [HarmonyPrefix]
    public static bool ModLibBindPrefix() {  
        Console.WriteLine("ModLibBind prefix");
        JankDebugger.stepMode = true;
        return false;
    }

    /*[HarmonyPatch(typeof(Brutal.VulkanApi.VkDeviceExtensions), nameof(Brutal.VulkanApi.VkDeviceExtensions.CreateSampler))]
    [HarmonyPrefix]
    public static bool VkDeviceExtensionsCreateSamplerPatch() {
        Console.WriteLine("Hi from VkDeviceExtensionsCreateSampler");
        return false;
    }*/
}

/*[HarmonyPatch(typeof(RenderCore.Systems.RenderGlobals), MethodType.Constructor)]
[HarmonyPatch(new Type[] { typeof(IVulkanContext) })]
class RenderGlobalsPatch {
    [HarmonyPrefix]
    public static bool Prefix(IVulkanContext vk) {
        Console.WriteLine("Hello!!");
        return false;
    }
}*/

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
                } else if (operand?.Name == "GetMemoryProperties2") {
                    //Console.WriteLine("This little rascal");
                    codes[i].opcode = OpCodes.Pop;
                    codes.Insert(i+1, new CodeInstruction(OpCodes.Pop));      //Consume the second
                } else if (operand?.Name == "CreateSampler") {
                    codes[i].opcode = OpCodes.Pop;                            //Consume the first argument
                    codes.Insert(i+1, new CodeInstruction(OpCodes.Pop));      //Consume the second
                    codes.Insert(i+2, new CodeInstruction(OpCodes.Pop));      //Consume the second
                    codes.Insert(i+3, new CodeInstruction(OpCodes.Ldc_I4_0)); //Push the object reference that the function was supposed to return (now null)
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

        JankDebugger.Inject(codes);

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

static class JankDebugger {
    public static List<CodeInstruction> instructionsCopy;
    public static bool stepMode = false;
    public static int stepCount = 0;

    public static void Inject(List<CodeInstruction> codes) {
        instructionsCopy = new List<CodeInstruction>(codes);

        //Console.WriteLine(codes.Count);
        int index = 1;
        int numConstraned = 0;
        for (int i = 0; i < instructionsCopy.Count-numConstraned-1; i++) {
            if (codes[index-1].opcode == OpCodes.Constrained) {
                index += 1;
                numConstraned += 1;
                continue;
            }
            codes.Insert(index, new CodeInstruction(OpCodes.Ldc_I4, i+1));
            codes.Insert(index+1, new CodeInstruction(OpCodes.Call, typeof(JankDebugger).GetMethod(nameof(DebuggerFn))));
            index += 3;
        }

        for (int i = 0; i < codes.Count; i++) {
            //Console.WriteLine(i + ":" + codes[i].opcode + " " + codes[i].operand);
        }
    }

    public static void DebuggerFn(int instructionIndex) {
        if (!stepMode) {
            return;
        }

        if (stepCount > 1) {
            stepCount -= 1;
            return;
        }

        Console.Write(instructionIndex + ": ");
        PrintInstruction(instructionsCopy[instructionIndex]);

        while (true) {
            Console.Write("> ");
            string? command = Console.ReadLine();

            if (command == null || command.Length == 0) {
                continue;
            }

            string[] tokens = command.Split(" ");
            switch (tokens[0]) {
                case "l":
                case "list": {
                    int num = 5;
                    if (tokens.Length > 1) {
                        if (!int.TryParse(tokens[1], out num)) {
                            Console.WriteLine("Usage: list [number_of_instructions_to_list]");
                            break;
                        }
                    }
                    
                    for (int i=0; i<num; i++) {
                        Console.Write((instructionIndex + i) + ": ");
                        PrintInstruction(instructionsCopy[instructionIndex + i]);
                    }

                    break;
                }

                case "c":
                case "continue": {
                    stepMode = false;
                    return;
                }

                case "n":
                case "next": {
                    if (tokens.Length > 1) {
                        int num = 0;
                        if (!int.TryParse(tokens[1], out num)) {
                            Console.WriteLine("Usage: next [number_of_instructions_to_run]");
                            break;
                        }
                        stepCount = num;
                    }

                    stepMode = true;
                    return;
                }

                default: {
                    Console.WriteLine("Unknown command \"" + tokens[0] + "\"");
                    break;
                }
            }
        }
    }

    public static void PrintInstruction(CodeInstruction inst) {
        Console.WriteLine(inst.opcode + " " + inst.operand);
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
