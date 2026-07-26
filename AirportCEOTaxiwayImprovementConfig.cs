using BepInEx.Configuration;

namespace AirportCEOTaxiwayImprovements;

internal static class AirportCEOTaxiwayImprovementConfig
{
    internal static ConfigEntry<bool> SmoothDiagonals { get; private set; }
    internal static ConfigEntry<string> AlternateLoadingPath { get; private set; }
    internal static ConfigEntry<bool> AutomaticallyTurnModOn { get; private set; }
    internal static ConfigEntry<bool> ImproveRoadMarkings { get; private set; }
    internal static ConfigEntry<bool> UseNewNodeSystem { get; private set; }

    internal static void SetUpConfig()
    {
        SmoothDiagonals = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Smooth Diagonals", true, "Determines whether to smooth diagonals out, or leave them with a " +
            "smooth line but a jagged tarmac base. Enabling this may cause very rare bugs, but nothing major.");
        AlternateLoadingPath = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Alternate Texture Loading Path", "", "LEAVE BLANK unless you know what you are doing. " +
            "Directory to load textures from instead of using the mod. Useful for development or non-steam users. Note: There is not much error handling in the loading code, this " +
            "may cause larger bugs if not all textures are found in the directory.");
        AutomaticallyTurnModOn = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Automatically Turn Mod On", true, "Automatically turn on (enable) the ACEO mod portion" +
            " of the mod loader so it works properly. You should not have to touch this setting.");
        UseNewNodeSystem = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Use Node Smoothing System", false, "Smooths the taxiway nodes in game slightly better than " +
            "the default game. Experimental. Requires reload to fully take effect.");

        ImproveRoadMarkings = AirportCEOTaxiwayImprovements.ConfigReference.Bind("Other", "Improve Road Markings", true, "Allows you to place one way road arrows before " +
            "taxiway crossings to add a little vehicle stop line and aircraft taxiway crossing warning sprite (without having to use stickers).");
    }
}
