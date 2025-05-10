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
    internal static ConfigEntry<string> AlternateLoadingPath { get; private set; }

    internal static void SetUpConfig()
    {
        SmoothDiagonals = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Smooth Diagonals", true, "Determines whether to smooth diagonals out, or leave them with a " +
            "smooth line but a jagged tarmac base. Enabling this may cause very rare bugs, but nothing major.");
        AlternateLoadingPath = AirportCEOTaxiwayImprovements.ConfigReference.Bind("General", "Alternate Texture Loading Path", "", "LEAVE BLANK unless you know what you are doing. " +
            "Directory to load textures from instead of using the mod. Useful for development or non-steam users. Note: There is not much error handling in the loading code, this " +
            "may cause larger bugs if not all textures are found in the directory.");
    }
}
