using AirportCEOTaxiwayImprovements._45DegreeTaxiways;
using HarmonyLib;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements.RoadMarkings;

[HarmonyPatch]
internal class RoadMarkingManager
{
    [HarmonyPatch(typeof(PlaceableRoad), nameof(PlaceableRoad.SetAsOneWay))]
    [HarmonyPostfix]
    internal static void RoadCrossingUpdater(PlaceableRoad __instance, float rotation)
    {
        if (!AirportCEOTaxiwayImprovementConfig.ImproveRoadMarkings.Value)
        {
            return;
        }

        Vector3 offset;

        if (rotation == 0)
        {
            offset = new Vector3(4, 0);
        }
        else if (rotation == 90)
        {
            offset = new Vector3(0, 4);
        }
        else if (rotation == 180)
        {
            offset = new Vector3(-4, 0);
        }
        else
        {
            offset = new Vector3(0, -4);
        }

        if (TiledObjectsManager.Instance.GetTileablesFromPosition(new Vector3[] { __instance.Position + offset }, Enums.TileType.Taxiway).Count > 0)
        {
            __instance.oneWayArrowSprite.sprite = TextureRegistry.planeCrossing;
        }
    }
}
