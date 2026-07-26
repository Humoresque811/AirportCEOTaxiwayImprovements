using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

[HarmonyPatch]
public class SaveLoadPatches
{
    [HarmonyPatch(typeof(MergedTile), nameof(MergedTile.GetSerializable))]
    [HarmonyPostfix]
    public static void FixSave(MergedTile __instance, ref MergedTileSerializeable __result)
    {
        if (__result.tileType != Enums.TileType.Taxiway)
        {
            return;
        }

        // Change special enum numbers back to their proper values for save load
        if (__result.quality == 5)
        {
            __result.quality = 2;
        }
        if (__result.quality == 4)
        {
            __result.quality = 1;
        }
    }
}
