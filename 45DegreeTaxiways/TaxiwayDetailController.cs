using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

//[HarmonyPatch]
public class TaxiwayDetailController
{
    //[HarmonyPatch(typeof(MergedTile), nameof(MergedTile.SetTile))]
    //[HarmonyPostfix]
    //public static void AddColorVariation(MergedTile __instance)
    //{
    //    if (__instance.TileType != Enums.TileType.Taxiway)
    //    {
    //        return;
    //    }
    //    float amount = Utils.RandomRangeF(0, 0.5f);
    //    Color adjustColor = Color.Lerp(Color.white, Color.grey, amount);
    //    __instance.spriteRenderer.color = new Color(0,0,0.5f,1f);
    //    AirportCEOTaxiwayImprovements.TILogger.LogInfo("in the new postfix");
    //}
}
