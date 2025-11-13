using HarmonyLib;
using BepInEx;

namespace Atrama
{
    [BepInPlugin(PluginID, PluginName, PluginVersion)]
    [BepInDependency(VehicleFramework.PluginInfo.PLUGIN_GUID)]
    public class MainPatcher : BaseUnityPlugin
    {
        public const string PluginID = "com.mikjaw.subnautica.atrama.mod";
        public const string PluginName = "AtramaVehicle";
        public const string PluginVersion = "2.0.0";
        public void Start()
        {
            new Harmony(PluginID).PatchAll();
            UWE.CoroutineHost.StartCoroutine(Atrama.Register());
        }
    }
}