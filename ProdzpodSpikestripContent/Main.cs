using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace ProdzpodSpikestripContent
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "_com.prodzpod";
        public const string PluginName = "ProdzpodSpikestripContent";
        public const string PluginVersion = "1.0.2";
        public static ManualLogSource Log;
        public static Harmony Harmony;
        public static PluginInfo pluginInfo;

        public void Awake()
        {
            pluginInfo = Info;
            Harmony = new Harmony(PluginGUID);
            Log = Logger;
            Log.LogInfo("hi :3");
        }
    }
}
