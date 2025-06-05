using AirportCEOModLoader.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

internal class AdditionalPrefabChanges
{
    internal static void DoModifications(SaveLoadGameDataController _)
    {
        try
        {
            // Stand large
            Transform largeStand = GameObject.Instantiate(BuildingController.Instance.aircraftStructuresPrefabs.largeStand.transform);
            largeStand.Translate(new Vector3(-10000, -10000));
            BuildingController.Instance.aircraftStructuresPrefabs.largeStand = largeStand.gameObject;

            Transform largeStandTBN = largeStand.GetChild(9);

            var newTile11 = GameObject.Instantiate(largeStandTBN.GetChild(0), largeStandTBN);
            newTile11.Translate(new Vector3(0, -4));
            newTile11.SetAsLastSibling();

            var newTile12 = GameObject.Instantiate(largeStandTBN.GetChild(15), largeStandTBN);
            newTile12.Translate(new Vector3(0, -4));
            newTile12.SetAsLastSibling();

            // Stand small
            Transform smallStand = GameObject.Instantiate(BuildingController.Instance.aircraftStructuresPrefabs.smallStand.transform);
            smallStand.Translate(new Vector3(-10000, -10000));
            BuildingController.Instance.aircraftStructuresPrefabs.smallStand = smallStand.gameObject;

            Transform smallStandTBN = smallStand.GetChild(8);

            var newTile21 = GameObject.Instantiate(smallStandTBN.GetChild(0), smallStandTBN);
            newTile21.Translate(new Vector3(0, -4));
            newTile21.SetAsLastSibling();

            var newTile22 = GameObject.Instantiate(smallStandTBN.GetChild(4), smallStandTBN);
            newTile22.Translate(new Vector3(0, -4));
            newTile22.SetAsLastSibling();


            // Normal small
            Transform runwayEntrance = GameObject.Instantiate(BuildingController.Instance.runwayEntrancesPrefabs.runwayEntrance.transform);
            runwayEntrance.Translate(new Vector3(-10000, -10000));
            BuildingController.Instance.runwayEntrancesPrefabs.runwayEntrance = runwayEntrance.gameObject;

            Transform runwayEntranceTBN = runwayEntrance.transform.GetChild(1);

            var newTile1 = GameObject.Instantiate(runwayEntranceTBN.GetChild(0), runwayEntranceTBN);
            newTile1.Translate(new Vector3(0, 4));
            newTile1.SetAsLastSibling();

            var newTile2 = GameObject.Instantiate(runwayEntranceTBN.GetChild(4), runwayEntranceTBN);
            newTile2.Translate(new Vector3(0, 4));
            newTile2.SetAsLastSibling();

            // Normal large
            Transform runwayEntranceLarge = GameObject.Instantiate(BuildingController.Instance.runwayEntrancesPrefabs.runwayEntranceLarge.transform);
            runwayEntranceLarge.Translate(new Vector3(-10000, -10000));
            BuildingController.Instance.runwayEntrancesPrefabs.runwayEntranceLarge = runwayEntranceLarge.gameObject;

            Transform runwayEntranceLargeTBN = runwayEntranceLarge.transform.GetChild(1);

            var newTile3 = GameObject.Instantiate(runwayEntranceLargeTBN.GetChild(0), runwayEntranceLargeTBN);
            newTile3.Translate(new Vector3(0, 4));
            newTile3.SetAsLastSibling();

            var newTile4 = GameObject.Instantiate(runwayEntranceLargeTBN.GetChild(6), runwayEntranceLargeTBN);
            newTile4.Translate(new Vector3(0, 4));
            newTile4.SetAsLastSibling();

            // Fast ones
            Transform runwayEntranceFast = GameObject.Instantiate(BuildingController.Instance.runwayEntrancesPrefabs.runwayFastEntrance.transform);
            runwayEntranceFast.Translate(new Vector3(-10000, -10000));
            BuildingController.Instance.runwayEntrancesPrefabs.runwayFastEntrance = runwayEntranceFast.gameObject;

            Transform runwayEntranceFastSprite = runwayEntranceFast.GetChild(0).GetChild(0);
            runwayEntranceFastSprite.localScale = new Vector3(4f * scaleAdjustment, 4f * scaleAdjustment, 1);

            runwayEntranceFast.GetChild(6).GetChild(0).GetChild(6).gameObject.SetActive(false);

            runwayEntranceFast.GetChild(0).GetChild(2).gameObject.SetActive(false);
            runwayEntranceFast.GetChild(7).Translate(new Vector3(4, 0));

            // Fast ones 2
            Transform runwayEntranceFastR = GameObject.Instantiate(BuildingController.Instance.runwayEntrancesPrefabs.runwayFastEntranceReversed.transform);
            runwayEntranceFastR.Translate(new Vector3(-10000, -10000));
            BuildingController.Instance.runwayEntrancesPrefabs.runwayFastEntranceReversed = runwayEntranceFastR.gameObject;

            Transform runwayEntranceFastRSprite = runwayEntranceFastR.GetChild(0).GetChild(0);
            runwayEntranceFastRSprite.localScale = new Vector3(4f * scaleAdjustment , 4f * scaleAdjustment, 1);

            runwayEntranceFastR.GetChild(6).GetChild(0).GetChild(6).gameObject.SetActive(false);

            runwayEntranceFastR.GetChild(0).GetChild(2).gameObject.SetActive(false);
            runwayEntranceFastR.GetChild(7).Translate(new Vector3(-4, 0));

            // Fast large ones
            Transform runwayEntranceLargeFast = GameObject.Instantiate(BuildingController.Instance.runwayEntrancesPrefabs.runwayFastEntranceLarge.transform);
            runwayEntranceLargeFast.Translate(new Vector3(-10000, -10000));
            BuildingController.Instance.runwayEntrancesPrefabs.runwayFastEntranceLarge = runwayEntranceLargeFast.gameObject;

            Transform runwayEntranceFastLargeSprite = runwayEntranceLargeFast.GetChild(0).GetChild(0);
            runwayEntranceFastLargeSprite.localPosition = Vector3.zero;
            runwayEntranceFastLargeSprite.localScale = new Vector3(4f * scaleAdjustment, 4f * scaleAdjustment, 1);

            runwayEntranceLargeFast.GetChild(6).GetChild(0).GetChild(6).gameObject.SetActive(false);

            runwayEntranceLargeFast.GetChild(7).Translate(new Vector3(4, 0));

            // Fast large ones 2
            Transform runwayEntranceLargeFastR = GameObject.Instantiate(BuildingController.Instance.runwayEntrancesPrefabs.runwayFastEntranceReversedLarge.transform);
            runwayEntranceLargeFastR.Translate(new Vector3(-10000, -10000));
            BuildingController.Instance.runwayEntrancesPrefabs.runwayFastEntranceReversedLarge = runwayEntranceLargeFastR.gameObject;

            Transform runwayEntranceFastLargeRSprite = runwayEntranceLargeFastR.GetChild(0).GetChild(0);
            runwayEntranceFastLargeRSprite.localPosition = Vector3.zero;
            runwayEntranceFastLargeRSprite.localScale = new Vector3(4f * scaleAdjustment, 4f * scaleAdjustment, 1);

            runwayEntranceLargeFastR.GetChild(6).GetChild(0).GetChild(6).gameObject.SetActive(false);

            runwayEntranceLargeFastR.GetChild(7).Translate(new Vector3(-4, 0));

            UpdateSprites(_);

            AirportCEOTaxiwayImprovements.TILogger.LogMessage($"Prefab changes have been make successfully!");
        }
        catch (Exception ex)
        {
            AirportCEOTaxiwayImprovements.TILogger.LogError($"Failed to do modifications to runway/stand prefabs. {ExceptionUtils.ProccessException(ex)}"); 
        }
    }

    internal static void UpdateSprites(SaveLoadGameDataController _)
    {
        // Sprite modifications this time!
        DataPlaceholderStructures.Instance.runwayFastExitAsphalt = TextureRegistry.AsphaltEntranceFast;
        DataPlaceholderStructures.Instance.runwayFastExitAsphaltLarge = TextureRegistry.AsphaltEntranceFastLarge;
        DataPlaceholderStructures.Instance.runwayFastExitConcrete = TextureRegistry.ConcreteEntranceFast;
        DataPlaceholderStructures.Instance.runwayFastExitConcreteLarge = TextureRegistry.ConcreteEntranceFastLarge;
    }

    private static readonly int scaleAdjustment = DownCompressor.GetDownscaleInt();
}
