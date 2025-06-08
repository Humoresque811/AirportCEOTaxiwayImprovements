using System;
using System.Collections.Generic;
using System.Diagnostics;
using AirportCEOModLoader;
using AirportCEOModLoader.Core;
using HarmonyLib;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

[HarmonyPatch]
public static class SmoothTaxiwayNodesPatch
{
    private const float NEWSCALEFORSPRITES = 2.4f;

    [HarmonyPatch(typeof(TaxiwayCenterBuilder), nameof(TaxiwayCenterBuilder.UpdatePiece))]
	[HarmonyPrefix]
	public static bool _SmoothTaxiwayPatch(TaxiwayCenterBuilder __instance)
	{
		// Do this just to make sure it doesn't get blown up in scale if we changed it earlier
		__instance.spriteRender.transform.localScale = new Vector3(1f, 1f, __instance.spriteRender.transform.localScale.z);
		__instance.spriteRender.sortingOrder = 50;

		if (!AirportCEOTaxiwayImprovementConfig.SmoothTaxiwayNodes.Value)
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

			__instance.GetConnectors();

			//bool[] newConnectorsArray = new bool[TaxiwayNodeImageServer.connectionArrayLength];
			//ImportBasicConnections(ref newConnectorsArray, ref __instance.connectors);
			//FindNewConnections(ref newConnectorsArray, __instance);

			Sprite spriteToSetTo = TaxiwayNodeImageServer.GetPotentialSprite(__instance.transform.position, out int rotation);

			if (spriteToSetTo != null)
			{
				__instance.spriteRender.sprite = spriteToSetTo;
				__instance.spriteRender.transform.eulerAngles = new Vector3(0, 0, rotation);
				__instance.spriteRender.transform.localScale = new Vector3(NEWSCALEFORSPRITES, NEWSCALEFORSPRITES, __instance.spriteRender.transform.localScale.z);
				__instance.spriteRender.sortingOrder = 80; // Above any non adjusted ones
				return false;
			}

			return true;
		}
		catch (Exception ex)
		{
			AirportCEOTaxiwayImprovements.TILogger.LogError($"Error in node code {ExceptionUtils.ProccessException(ex)}");
			return true;
		}
	}



	// This works the same as since our arrangement of nodes in the array (24 rather than 12 place) is different
	private static void ImportBasicConnections(ref bool[] newConnections, ref bool[] originalConnections)
	{
        for (int i = 0; i < originalConnections.Length; i++)
		{
			newConnections[i] = originalConnections[i];
        }
	}

	private static void FindNewConnections(ref bool[] newConnections, TaxiwayCenterBuilder node)
	{
		Stopwatch stopwatch = new();
		stopwatch.Start();

		newConnections[8] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 8, node.transform.position.y - 8))	!= null;
		newConnections[9] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 8, node.transform.position.y - 4))	!= null;
		newConnections[10] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 8, node.transform.position.y))		!= null;
		newConnections[11] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 8, node.transform.position.y + 4)) != null;
		newConnections[12] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 8, node.transform.position.y + 8)) != null;

		newConnections[13] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 4, node.transform.position.y - 8)) != null;
		newConnections[14] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 4, node.transform.position.y + 8)) != null;

		newConnections[15] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x, node.transform.position.y - 8))		!= null;
		newConnections[16] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x, node.transform.position.y + 8))		!= null;

		newConnections[17] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 4, node.transform.position.y - 8)) != null;
		newConnections[18] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 4, node.transform.position.y + 8)) != null;

		newConnections[19] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 8, node.transform.position.y - 8))	!= null;
		newConnections[20] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 8, node.transform.position.y - 4))	!= null;
		newConnections[21] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 8, node.transform.position.y))		!= null;
		newConnections[22] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 8, node.transform.position.y + 4)) != null;
		newConnections[23] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 8, node.transform.position.y + 8)) != null;

		// Next (even bigger) radius area

		newConnections[24] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 12, node.transform.position.y - 12))	!= null;
		newConnections[25] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 12, node.transform.position.y - 8))	!= null;
		newConnections[26] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 12, node.transform.position.y - 4))	!= null;
		newConnections[27] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 12, node.transform.position.y + 0))	!= null;
		newConnections[28] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 12, node.transform.position.y + 4))	!= null;
		newConnections[29] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 12, node.transform.position.y + 8))	!= null;
		newConnections[30] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 12, node.transform.position.y + 12))	!= null;

		newConnections[31] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 8, node.transform.position.y - 12))	!= null;
		newConnections[32] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 8, node.transform.position.y + 12))	!= null;

		newConnections[33] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 4, node.transform.position.y - 12))	!= null;
		newConnections[34] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 4, node.transform.position.y + 12))	!= null;

		newConnections[35] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 0, node.transform.position.y - 12))	!= null;
		newConnections[36] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 0, node.transform.position.y + 12))	!= null;

		newConnections[37] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 4, node.transform.position.y - 12))	!= null;
		newConnections[38] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 4, node.transform.position.y + 12))	!= null;

		newConnections[39] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 8, node.transform.position.y - 12))	!= null;
		newConnections[40] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 8, node.transform.position.y + 12))	!= null;

		newConnections[41] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 12, node.transform.position.y - 12))	!= null;
		newConnections[42] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 12, node.transform.position.y - 8))	!= null;
		newConnections[43] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 12, node.transform.position.y - 4))	!= null;
		newConnections[44] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 12, node.transform.position.y + 0))	!= null;
		newConnections[45] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 12, node.transform.position.y + 4))	!= null;
		newConnections[46] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 12, node.transform.position.y + 8))	!= null;
		newConnections[47] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 12, node.transform.position.y + 12))	!= null;

		// Outermost ones (sparse)

		newConnections[48] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 16, node.transform.position.y - 12))	!= null;
		newConnections[49] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 16, node.transform.position.y + 12))	!= null;
		newConnections[51] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 12, node.transform.position.y - 16))	!= null;
		newConnections[50] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x - 12, node.transform.position.y + 16))	!= null;
		newConnections[52] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 12, node.transform.position.y - 16))	!= null;
		newConnections[53] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 12, node.transform.position.y + 16))	!= null;
		newConnections[54] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 16, node.transform.position.y - 12))	!= null;
		newConnections[55] = Singleton<TaxiwayController>.Instance.GetNodeAtPosition(new Vector2(node.transform.position.x + 16, node.transform.position.y + 12))	!= null;

		stopwatch.Stop();
		AirportCEOTaxiwayImprovements.TILogger.LogInfo($"Time: {stopwatch.ElapsedMilliseconds}");
	}

	private static DataPlaceholderStructures DPS = SingletonNonDestroy<DataPlaceholderStructures>.Instance;

    private static readonly int scaleAdjustment = DownCompressor.GetDownscaleInt();
}
