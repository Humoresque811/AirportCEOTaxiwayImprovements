using AirportCEOTaxiwayImprovements._45DegreeTaxiways;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace AirportCEOTaxiwayImprovements;

[BepInPlugin("org.airportceotaxiwayimprovements.humoresque", "AirportCEO Taxiway Improvements", PluginInfo.PLUGIN_VERSION)]
[BepInDependency("org.airportceomodloader.humoresque")]
public class AirportCEOTaxiwayImprovements : BaseUnityPlugin
{
    public static AirportCEOTaxiwayImprovements Instance { get; private set; }
    internal static Harmony Harmony { get; private set; }
    internal static ManualLogSource TILogger { get; private set; }
    internal static ConfigFile ConfigReference {  get; private set; }

    private void Awake()
    {
        Harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        Harmony.PatchAll(); 

        // Plugin startup logic
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        Instance = this;
        TILogger = Logger;
        ConfigReference = Config;

        // Config
        //Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is setting up config.");
        //AirportCEOCustomBuildablesConfig.SetUpConfig();
        //Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} finished setting up config.");

        TextureManager.LoadTextures();
    }
}
