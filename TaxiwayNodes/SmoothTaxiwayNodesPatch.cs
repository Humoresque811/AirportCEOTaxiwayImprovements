using AirportCEOModLoader.Core;
using HarmonyLib;
using Nodes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements.TaxiwayNodes;

[HarmonyPatch]
internal class SmoothTaxiwayNodesPatch
{
    private const float NEWSCALEFORSPRITES = 2f;

    [HarmonyPatch(typeof(TaxiwayCenterBuilder), nameof(TaxiwayCenterBuilder.UpdatePiece))]
	[HarmonyPrefix]
	public static bool _SmoothTaxiwayPatch(TaxiwayCenterBuilder __instance)
	{
		// Do this just to make sure it doesn't get blown up in scale if we changed it earlier
		__instance.spriteRender.transform.localScale = new Vector3(1f, 1f, __instance.spriteRender.transform.localScale.z);
		__instance.spriteRender.sortingOrder = 50;

		if (!AirportCEOTaxiwayImprovementConfig.UseNewNodeSystem.Value)
		{
			return true;
		}

		try
		{
			if (__instance.foundationType == Enums.FoundationType.Grass)
			{
				// We ain't touching it then
				return true;
			}

			TaxiwayCenterBuilderSupplement supplement = __instance.gameObject.GetComponent<TaxiwayCenterBuilderSupplement>();
			if (supplement == null)
            {
				supplement = __instance.gameObject.AddComponent<TaxiwayCenterBuilderSupplement>();
				supplement.Init(__instance);
            }
			supplement.SetUpNodeAndConnectors();

            Sprite spriteToSetTo = TaxiwayNodeImageServer.GetPotentialSprite(supplement, out int rotation);

			if (spriteToSetTo == null)
			{
				return true;
			}

			__instance.spriteRender.sprite = spriteToSetTo;
			__instance.spriteRender.transform.eulerAngles = new Vector3(0, 0, rotation);
			__instance.spriteRender.transform.localScale = new Vector3(NEWSCALEFORSPRITES, NEWSCALEFORSPRITES, __instance.spriteRender.transform.localScale.z);
			__instance.spriteRender.sortingOrder = 80; // Above any non adjusted ones
			return false;

		}
		catch (Exception ex)
		{
			AirportCEOTaxiwayImprovements.TILogger.LogError($"Error in node code {ExceptionUtils.ProccessException(ex)}");
			return true;
		}
	}
}
