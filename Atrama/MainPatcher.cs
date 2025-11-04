using HarmonyLib;
using Nautilus.Utility;
using BepInEx;


namespace Atrama
{
    public static class Logger
    {
        public static void Output(string msg)
        {
            BasicText message = new BasicText(500, 0);
            message.ShowMessage(msg, 5);
        }
    }

    [BepInPlugin("com.mikjaw.subnautica.atrama.mod", "AtramaVehicle", "2.0.0")]
    [BepInDependency("com.mikjaw.subnautica.vehicleframework.mod")]
    [BepInDependency("com.snmodding.nautilus")]

    public class MainPatcher : BaseUnityPlugin
    {
        //internal static AtramaConfig Config { get; private set; }

        public void Start()
        {
            var harmony = new Harmony("com.mikjaw.subnautica.atrama.mod");
            harmony.PatchAll();
            UWE.CoroutineHost.StartCoroutine(Atrama.Register());
        }
    }
    /*
    [Menu("Atrama Options")]
    public class AtramaConfig : ConfigFile
    {
        [Toggle("temp")]
        public bool temp = false;
    }
    */
}
