using AirportCEOModLoader.Core;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Bindings;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

[HarmonyPatch]
public static class DiagonalTaxiwaysPatch
{
	[HarmonyPatch(typeof(TaxiwayTile), MethodType.Constructor, new Type[] {typeof(Vector3), typeof(bool), typeof(byte)} )]
	[HarmonyPostfix]
	public static void SetPloPatch(TaxiwayTile __instance)
	{
		TextureRegistry.associatedTiles[__instance.taxiwayBuilderTile] = __instance;
	}

	[HarmonyPatch(typeof(TaxiwayBuilderNode), "UpdatePiece")]
	[HarmonyPrefix]
	public static bool _45DegreePatch(TaxiwayBuilderNode __instance)
	{
		try
		{
			if (__instance.foundationType == Enums.FoundationType.Grass)
			{
				// We ain't touching it then
				return true;
			}

			__instance.GetConnectors();
			__instance.connectionBottomLeft = __instance.connectors[0];
			__instance.connectionLeft = __instance.connectors[1];
			__instance.connectionTopLeft = __instance.connectors[2];
			__instance.connectionBack = __instance.connectors[3];
			__instance.connectionFront = __instance.connectors[4];
			__instance.connectionBottomRight = __instance.connectors[5];
			__instance.connectionRight = __instance.connectors[6];
			__instance.connectionTopRight = __instance.connectors[7];
			__instance.taxiwayType = Enums.TaxiwayBuilderType.Plain;
			try
			{
				for (int i = 0; i < __instance.taxiwayLines.childCount; i++)
				{
					UnityEngine.Object.Destroy(__instance.taxiwayLines.GetChild(i).gameObject);
				}
			}
			catch (Exception)
			{
			}

			if (!__instance.enableBuilder)
			{
				return true;
			}

			int count = 0;
			foreach (bool connection in __instance.connectors)
			{
				count += connection ? 1 : 0;
			}

			bool[] newConnectorsArray = new bool[12];
			ImportBasicConnections(ref newConnectorsArray, ref __instance.connectors);
			FindNewConnections(ref newConnectorsArray, __instance);

			Action<Enums.FoundationType, int, TaxiwayBuilderNode> action = null;
			Action<Enums.FoundationType, int, TaxiwayBuilderNode> actionToCompareTo = TextureRegistry.ApplyTaxiwayEdgePlain;

			for (int i = 0; i < 360; i += 90)
			{
				action = FinalAction(count, newConnectorsArray);
				if (action.Method != actionToCompareTo.Method)
				{
					action(__instance.foundationType, i, __instance);
					break;
				}
				newConnectorsArray.RotateConnections();
			}
			if (action.Method == actionToCompareTo.Method)
			{
				action(__instance.foundationType, 0, __instance);
			}
			return false;
		}
		catch (Exception ex)
		{
			AirportCEOTaxiwayImprovements.TILogger.LogError($"Error as expected {ExceptionUtils.ProccessException(ex)}");
			return true;
		}
	}

	public static Action<Enums.FoundationType, int, TaxiwayBuilderNode> FinalAction(int connections, bool[] newConnectionsArray) =>
		(connections, newConnectionsArray[0], newConnectionsArray[1], newConnectionsArray[2], newConnectionsArray[3],
		newConnectionsArray[4], newConnectionsArray[5], newConnectionsArray[6], newConnectionsArray[7],
		newConnectionsArray[8], newConnectionsArray[9], newConnectionsArray[10], newConnectionsArray[11]) switch
		{
			(7, _, _, false, _, _, _, _, _, true, true, true, true) => TextureRegistry.ApplyTaxiwayEdgeTurnEdge,
			(7, _, _, _, _, false, _, _, _, true, _, _, true) => TextureRegistry.ApplyTaxiwayEdgeStraightEdge,
			(7, _, _, false, _, _, _, _, _, true, _, _, _) => TextureRegistry.ApplyHorizontalCurveInto,
			(5, _, _, _, true, true, true, true, true, _, _, false, _ ) => TextureRegistry.ApplyVerticalCurveOut,
			(5, true, true, _, true, _, true, true, _, false, _, _, _ ) => TextureRegistry.ApplyHorizontalCurveOut,
			(5, true, true, _, true, _, true, true, _, true, _, _, true) => TextureRegistry.ApplyTaxiwayEdgeStraightEdge,
			(6, true, true, _, true, _, true, true, _, true, _, _, true) => TextureRegistry.ApplyTaxiwayEdgeStraightEdge,
			(6, true, _, _, true, true, true, true, true, _, _, false, _) => TextureRegistry.ApplyVerticalCurveOut,
			(6, true, true, _, true, _, true, true, true, false, _, _, _) => TextureRegistry.ApplyHorizontalCurveOut,
			(7, _, _, false, _, _, _, _, _, _, _, true, _) => TextureRegistry.ApplyVerticalCurveInto,
			(7, _, _, false, _, _, _, _, _, _, _, _, _) => TextureRegistry.ApplyFullDia,
			(3, _, _, _, true, _, true, true, _, _, _, _, _ ) => TextureRegistry.ApplySmallTri,
			(4, true, _, _, true, _, true, true, _, _, _, _, _ ) => TextureRegistry.ApplySmallTri,
			(4, _, _, _, true, _, true, true, true, _, _, _, _ ) => TextureRegistry.ApplySmallTri,
			(5, true, _, _, true, _, true, true, true, _, _, _, _ ) => TextureRegistry.ApplySmallTri,
			_ => TextureRegistry.ApplyTaxiwayEdgePlain,
		};

	private static bool[] RotateConnections(this bool[] newConnectionsArray)
	{
		// See humoresques magic table for why this is this way
		bool[] output = new bool[12];
		output[2] = newConnectionsArray[0];
		output[4] = newConnectionsArray[1];
		output[7] = newConnectionsArray[2];
		output[1] = newConnectionsArray[3];
		output[6] = newConnectionsArray[4];
		output[0] = newConnectionsArray[5];
		output[3] = newConnectionsArray[6];
		output[5] = newConnectionsArray[7];
		output[10] = newConnectionsArray[8];
		output[8] = newConnectionsArray[9];
		output[11] = newConnectionsArray[10];
		output[9] = newConnectionsArray[11];

        for (int i = 0; i < output.Length; i++)
		{
			newConnectionsArray[i] = output[i];
        }

		return output;
	}

	private static void ImportBasicConnections(ref bool[] newConnections, ref bool[] originalConnections)
	{
        for (int i = 0; i < originalConnections.Length; i++)
		{
			newConnections[i] = originalConnections[i];
        }
	}

	private static void FindNewConnections(ref bool[] newConnections, TaxiwayBuilderNode node)
	{
		newConnections[8] = Singleton<TaxiwayController>.Instance.TryGetBuilderNodeAtPostion(new Vector2(node.connector.position.x - 8, node.connector.position.y), out var _);
		newConnections[9] = Singleton<TaxiwayController>.Instance.TryGetBuilderNodeAtPostion(new Vector2(node.connector.position.x, node.connector.position.y - 8), out var _);
		newConnections[10] = Singleton<TaxiwayController>.Instance.TryGetBuilderNodeAtPostion(new Vector2(node.connector.position.x, node.connector.position.y + 8 ), out var _);
		newConnections[11] = Singleton<TaxiwayController>.Instance.TryGetBuilderNodeAtPostion(new Vector2(node.connector.position.x + 8, node.connector.position.y), out var _);
	}

	private static DataPlaceholderStructures DPS = SingletonNonDestroy<DataPlaceholderStructures>.Instance;
}
