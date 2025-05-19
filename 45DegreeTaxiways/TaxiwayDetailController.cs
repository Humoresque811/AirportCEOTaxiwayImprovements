using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Enums;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

[HarmonyPatch]
public class TaxiwayDetailController
{
    //[HarmonyPatch(typeof(PlaceableObject), nameof(PlaceableObject.IsLegalTiles))]
    //[HarmonyPostfix]
    //public static void AllowPlacementOfTarmac(PlaceableObject __instance, Vector3[] positionArray, int wLayer, ref bool __result)
    //{
    //    for (int i = 0; i < positionArray.Length; i++)
    //    {
    //        Vector4 vector = positionArray[i].SetW(wLayer);
    //        if (Singleton<TiledObjectsManager>.Instance.TryGetTileable(vector, out var tileable) || Singleton<TiledObjectsManager>.Instance.TryGetCenterTile(vector, out tileable))
    //        {
    //            if (tileable is not TaxiwayTile && tileable is not DecorationTile && tileable is not PlaceableTile)
    //            {
    //                return; // Don't change result
    //            }
    //        }
    //    }

    //    __result = true;
    //}

    //[HarmonyPatch(typeof(PlacementUtils), nameof(PlacementUtils.InstantiateTiles))]
    //[HarmonyPrefix]
    //public static void AllowPlacementOfTarmac2(PlacementUtils __instance, PlaceableObject referencePlo, int floor)
    //{
    //    AirportCEOTaxiwayImprovements.TILogger.LogMessage("In the allow placement 2 func " + referencePlo.GetType().Name);
    //    if (referencePlo is not PlaceableTile)
    //    {
    //        return;
    //    }


	   // if (TiledObjectsManager.Instance.TryGetTileable(referencePlo.Position.SetZ(floor).SetW((referencePlo as ITileSource).WLayer), out var taxiwayTileableHopefully))
	   // {
    //        if (taxiwayTileableHopefully.Quality >= 4) // we know its one of the small tris then
    //        {
    //            AirportCEOTaxiwayImprovements.TILogger.LogMessage("we here now!");
    //            taxiwayTileableHopefully.Quality -= 3;
    //            taxiwayTileableHopefully.UpdateTile();
    //            return;
    //        }
    //    }
    //}
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
