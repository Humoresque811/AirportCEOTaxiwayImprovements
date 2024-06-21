using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

internal static class TextureRegistry
{
    internal static Dictionary<TaxiwayBuilderNode, TaxiwayTile> associatedTiles = new Dictionary<TaxiwayBuilderNode, TaxiwayTile>();

    internal static Sprite ConcreteVerticalCurveInto;
    internal static Sprite ConcreteHorizontalCurveInto;

    internal static Sprite ConcreteVerticalCurveOutOf;
    internal static Sprite ConcreteHorizontalCurveOutOf;

    internal static Sprite ConcreteDiagonalHalf;
    internal static Sprite ConcreteDiagonalFull;

    internal static Sprite AsphaltVerticalCurveInto;
    internal static Sprite AsphaltHorizontalCurveInto;

    internal static Sprite AsphaltVerticalCurveOutOf;
    internal static Sprite AsphaltHorizontalCurveOutOf;

    internal static Sprite AsphaltDiagonalHalf;
    internal static Sprite AsphaltDiagonalFull;


    internal static void Init()
    {
        ConcreteVerticalCurveInto = TextureManager.CombineTextures(TextureManager.ConcreteFull, TextureManager.VerticalCurveInto);
        ConcreteHorizontalCurveInto = TextureManager.CombineTextures(TextureManager.ConcreteFull, TextureManager.HorizontalCurveInto);

        ConcreteVerticalCurveOutOf = TextureManager.CombineTextures(TextureManager.ConcreteFull, TextureManager.VerticalCurveOutOf);
        ConcreteHorizontalCurveOutOf = TextureManager.CombineTextures(TextureManager.ConcreteFull, TextureManager.HorizontalCurveOutOf);

        ConcreteDiagonalHalf = TextureManager.CombineTextures(TextureManager.ConcreteTri, TextureManager.DiagonalHalf);
        ConcreteDiagonalFull = TextureManager.CombineTextures(TextureManager.ConcreteFull, TextureManager.DiagonalFull);

        AsphaltVerticalCurveInto = TextureManager.CombineTextures(TextureManager.AsphaltFull, TextureManager.VerticalCurveInto);
        AsphaltHorizontalCurveInto = TextureManager.CombineTextures(TextureManager.AsphaltFull, TextureManager.HorizontalCurveInto);

        AsphaltVerticalCurveOutOf = TextureManager.CombineTextures(TextureManager.AsphaltFull, TextureManager.VerticalCurveOutOf);
        AsphaltHorizontalCurveOutOf = TextureManager.CombineTextures(TextureManager.AsphaltFull, TextureManager.HorizontalCurveOutOf);

        AsphaltDiagonalHalf = TextureManager.CombineTextures(TextureManager.AsphaltTri, TextureManager.DiagonalHalf);
        AsphaltDiagonalFull = TextureManager.CombineTextures(TextureManager.AsphaltFull, TextureManager.DiagonalFull);

    }

    internal static void ApplySmallTri(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltDiagonalHalf : ConcreteDiagonalHalf;
        renderer.material = DataPlaceholderMaterials.Instance.externalSpritesMaterial;

        try
        {
            associatedTiles[node].quality = 4;
        }
        catch (Exception ex)
        {
            AirportCEOTaxiwayImprovements.TILogger.LogError($"Unable to set part. {ex.Message}");
        }
    }
    internal static void ApplyFullDia(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltDiagonalFull : ConcreteDiagonalFull;
    }
    internal static void ApplyHorizCurveInto(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltHorizontalCurveInto : ConcreteHorizontalCurveInto;
    }


    internal static void ApplyVerticalCurveOut(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);

        obj.GetComponent<SpriteRenderer>().sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltVerticalCurveOutOf : ConcreteVerticalCurveOutOf;
    }
    internal static void ApplyHorizontalCurveOut(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);

        obj.GetComponent<SpriteRenderer>().sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltHorizontalCurveOutOf : ConcreteHorizontalCurveOutOf;
    }
    internal static void ApplyVerticalCurveInto(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);

        obj.GetComponent<SpriteRenderer>().sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltVerticalCurveInto : ConcreteVerticalCurveInto;
    }
    internal static void ApplyHorizontalCurveInto(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);

        obj.GetComponent<SpriteRenderer>().sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltHorizontalCurveInto : ConcreteHorizontalCurveInto;
    }
    private static Transform GetBasicTile(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GameObject.Instantiate(SingletonNonDestroy<DataPlaceholderStructures>.Instance.GetTaxiwayEdge(Enums.FoundationType.Asphalt, Enums.TaxiwayBuilderType.StraightEdge), node.transform).transform;
        obj.eulerAngles = new Vector3(0f, 0f, rotation);
        obj.localPosition = new Vector3(0f, 0f, 0.001f);

        try
        {
            associatedTiles[node].quality = (byte)foundationType;
        }
        catch (Exception ex)
        {
            AirportCEOTaxiwayImprovements.TILogger.LogError($"Unable to set part. {ex.Message}");
        }

        return obj;
    }


    internal static void ApplyTaxiwayEdgePlain(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        // Dont actually do anything here!
        //node.InstantiateTaxiwayPiece(DPS.GetTaxiwayEdge(foundationType, Enums.TaxiwayBuilderType.Plain), rotation);
    }
    internal static void ApplyTaxiwayEdgeCornerEdge(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        node.InstantiateTaxiwayPiece(DPS.GetTaxiwayEdge(foundationType, Enums.TaxiwayBuilderType.CornerEdge), rotation);
    }
    internal static void ApplyTaxiwayEdgeTurnEdge(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        rotation -= 180;
        node.InstantiateTaxiwayPiece(DPS.GetTaxiwayEdge(foundationType, Enums.TaxiwayBuilderType.TurnEdge), rotation);
    }
    internal static void ApplyTaxiwayEdgeStraightEdge(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        rotation -= 180;
        node.InstantiateTaxiwayPiece(DPS.GetTaxiwayEdge(foundationType, Enums.TaxiwayBuilderType.StraightEdge), rotation);
    }

	private static DataPlaceholderStructures DPS = SingletonNonDestroy<DataPlaceholderStructures>.Instance;
}
