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
		//if (!Singleton<TiledObjectsManager>.Instance.ContainsTileable(__instance.worldPosition))
		//{
		//	Singleton<TiledObjectsManager>.Instance.AddTileable(TextureRegistry.associatedTiles[__instance]);
		//}

		if (__instance.taxiwayType != (Enums.TaxiwayBuilderType)4)
		{
			if (__instance.foundationType == Enums.FoundationType.Grass)
			{
				// We ain't touching it then
				return true;
			}
		}

		__instance.GetConnectors();
		__instance.connectionBottomLeft =  __instance.connectors[0];
		__instance.connectionLeft =        __instance.connectors[1];
		__instance.connectionTopLeft =     __instance.connectors[2];
		__instance.connectionBack =        __instance.connectors[3];
		__instance.connectionFront =       __instance.connectors[4];
		__instance.connectionBottomRight = __instance.connectors[5];
		__instance.connectionRight =       __instance.connectors[6];
		__instance.connectionTopRight =    __instance.connectors[7];
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

		if (count == 0 || count == 1 || count == 2 || count == 8)
		{
			// Its blank, nothing needs to be done!
			return true;
		}
		// Start Test Block

		if (count == 3)
		{

			if (__instance.connectionFront && __instance.connectionTopLeft && __instance.connectionLeft)
			{
				__instance.taxiwayType = Enums.TaxiwayBuilderType.CornerEdge;
				__instance.InstantiateTaxiwayPiece(DPS.GetTaxiwayEdge(__instance.foundationType, __instance.taxiwayType), 90);
			}
			else if (__instance.connectionBack && __instance.connectionBottomLeft && __instance.connectionLeft)
			{
				__instance.taxiwayType = Enums.TaxiwayBuilderType.CornerEdge;
				__instance.InstantiateTaxiwayPiece(DPS.GetTaxiwayEdge(__instance.foundationType, __instance.taxiwayType), 180);
			}
			else if (__instance.connectionBack && __instance.connectionBottomRight && __instance.connectionRight)
			{
				__instance.taxiwayType = Enums.TaxiwayBuilderType.CornerEdge;
				__instance.InstantiateTaxiwayPiece(DPS.GetTaxiwayEdge(__instance.foundationType, __instance.taxiwayType), 270);
			}
			else
			{
				__instance.taxiwayType = Enums.TaxiwayBuilderType.CornerEdge;
				__instance.InstantiateTaxiwayPiece(DPS.GetTaxiwayEdge(__instance.foundationType, __instance.taxiwayType), 0);
			}
			return false;
		}
		if (count == 4)
		{
			if ((__instance.connectionBack && __instance.connectionBottomLeft && __instance.connectionBottomRight && __instance.connectionRight) ||
				(__instance.connectionBack && __instance.connectionTopRight && __instance.connectionBottomRight && __instance.connectionRight))
			{
				__instance.taxiwayType = (Enums.TaxiwayBuilderType)4;
				TextureRegistry.ApplySmallTri(__instance.foundationType, 0, __instance);
			}
			else if ((__instance.connectionBack && __instance.connectionBottomLeft && __instance.connectionBottomRight && __instance.connectionLeft) || 
				(__instance.connectionBack && __instance.connectionBottomLeft && __instance.connectionLeft && __instance.connectionTopLeft))
			{
				__instance.taxiwayType = (Enums.TaxiwayBuilderType)4;
				TextureRegistry.ApplySmallTri(__instance.foundationType, 270, __instance);
			}
			else if ((__instance.connectionLeft && __instance.connectionTopLeft && __instance.connectionFront && __instance.connectionTopRight) || 
				(__instance.connectionLeft && __instance.connectionTopLeft && __instance.connectionFront && __instance.connectionBottomLeft))
			{
				__instance.taxiwayType = (Enums.TaxiwayBuilderType)4;
				TextureRegistry.ApplySmallTri(__instance.foundationType, 180, __instance);
			}
			else if ((__instance.connectionTopRight && __instance.connectionFront && __instance.connectionTopLeft && __instance.connectionRight) || 
				(__instance.connectionTopRight && __instance.connectionFront && __instance.connectionBottomRight && __instance.connectionRight))
			{
				__instance.taxiwayType = (Enums.TaxiwayBuilderType)4;
				TextureRegistry.ApplySmallTri(__instance.foundationType, 90, __instance);
			}

			return false;
		}
		if (count == 5)
		{
			if (!__instance.connectionLeft && !__instance.connectionTopLeft && !__instance.connectionFront)
			{
				__instance.taxiwayType = (Enums.TaxiwayBuilderType)5;
				TextureRegistry.ApplySmallTri(__instance.foundationType, 0, __instance);
				return false;
			}
		}
		if (count == 7)
		{
			int rotation = 0;
			if (!__instance.connectionTopLeft) { rotation = 0; }
			if (!__instance.connectionTopRight) { rotation = 90; }
			if (!__instance.connectionBottomRight) { rotation = 180; }
			if (!__instance.connectionBottomLeft) { rotation = 270; }

			Vector2 tester = new Vector2(__instance.transform.position.x - 8, __instance.transform.position.y);

			if (Singleton<TaxiwayController>.Instance.TryGetBuilderNodeAtPostion(tester, out var _))
			{
				__instance.taxiwayType = (Enums.TaxiwayBuilderType)5;
				TextureRegistry.ApplyHorizCurveInto(__instance.foundationType, rotation, __instance);
			}
			else
			{
				__instance.taxiwayType = (Enums.TaxiwayBuilderType)5;
				TextureRegistry.ApplyFullDia(__instance.foundationType, rotation, __instance);
			}

			return false;
		}


		return true;

	}

	public static Action<Enums.FoundationType, int, TaxiwayBuilderNode> FinalAction(int connections, bool[] newConnectionsArray) =>
		(connections, newConnectionsArray[0], newConnectionsArray[1], newConnectionsArray[2], newConnectionsArray[3],
		newConnectionsArray[4], newConnectionsArray[5], newConnectionsArray[6], newConnectionsArray[7],
		newConnectionsArray[8], newConnectionsArray[9], newConnectionsArray[10], newConnectionsArray[11]) switch
		{
			(0, _, _, _, _, _, _, _, _, _, _, _, _ ) => TextureRegistry.ApplyTaxiwayEdgePlain,
			_ => TextureRegistry.ApplyTaxiwayEdgePlain,
		};

	private static bool[] RotateConnections(this bool[] newConnectionsArray)
	{
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

		return output;
	}

	private static DataPlaceholderStructures DPS = SingletonNonDestroy<DataPlaceholderStructures>.Instance;
}
