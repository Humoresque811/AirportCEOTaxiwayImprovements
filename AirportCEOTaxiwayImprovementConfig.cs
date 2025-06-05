using AirportCEOTaxiwayImprovements._45DegreeTaxiways;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportCEOTaxiwayImprovements;

internal static class AirportCEOTaxiwayImprovementConfig
{
    internal static ConfigEntry<bool> SmoothDiagonals { get; private set; }
    internal static ConfigEntry<bool> SmoothTaxiwayNodes { get; private set; }
    internal static ConfigEntry<string> AlternateLoadingPath { get; private set; }
    internal static ConfigEntry<bool> AutomaticallyTurnModOn { get; private set; }
    internal static ConfigEntry<DownCompressor.DownscaleLevel> DownscaleLevel { get; private set; }
    internal static ConfigEntry<bool> UseTaxiwayNodeCache { get; private set; }

    internal static void SetUpConfig()
    {
        SmoothDiagonals = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Smooth Diagonals", true, "Determines whether to smooth diagonals out, or leave them with a " +
            "smooth line but a jagged tarmac base. Enabling this may cause very rare bugs, but nothing major.");
        SmoothTaxiwayNodes = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Smooth Taxiway Nodes", false, "Determines whether to make taxiway nodes more smooth by " +
            "enabling larger radius turns for center lines. Since this is a complicated system, this might not work for all layouts. Currently an experimental feature.");
        AlternateLoadingPath = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Alternate Texture Loading Path *", "", "*RELOAD GAME REQUIRED \n LEAVE BLANK unless " +
            "you know what you are doing. Directory to load textures from instead of using the mod. Useful for development or non-steam users. Note: There is not much error handling " +
            "in the loading code, this may cause larger bugs if not all textures are found in the directory.");
        AutomaticallyTurnModOn = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Automatically Turn Mod On", true, "Automatically turn on (enable) the ACEO mod portion" +
            " of the mod loader so it works properly. You should not have to touch this setting.");
        DownscaleLevel = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Downscaling Level *", DownCompressor.DownscaleLevel.Original,
            "*RELOAD GAME REQUIRED \n" +
            "Amount to downscale the texture by (per axis, so 2x means you get 4x less quality, 2x less VRAM/RAM usage). Only use if you have very little memory (RAM/VRAM)");
        UseTaxiwayNodeCache = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Use Taxiway Node Cache", true, "Use a caching system for taxiway node sprites? " +
            "It is highly recommended to keep this option on, it is mostly just useful in development.");
    }
}
