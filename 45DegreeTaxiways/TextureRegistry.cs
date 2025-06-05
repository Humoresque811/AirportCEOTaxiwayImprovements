using AirportCEOModLoader.Core;
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

    internal static Sprite AsphaltEntranceFast;
    internal static Sprite AsphaltEntranceFastLarge;

    internal static Sprite ConcreteEntranceFast;
    internal static Sprite ConcreteEntranceFastLarge;

    internal static Sprite AsphaltEndCap;
    internal static Sprite ConcreteEndCap;


    // Taxiway Nodes

    internal static Sprite Straight_45;
    internal static Sprite Straight_90;

    internal static Sprite Curve_4590_P3;
    internal static Sprite Curve_4590_P4;
    internal static Sprite CurveStraight_4590_P4;
    internal static Sprite Curve_4590_P5;
    internal static Sprite CurveStraight_4590_P5;

    internal static Sprite X_4590;
    internal static Sprite X_4590_Curves;


    internal static void Init()
    {
        ConcreteVerticalCurveInto = TextureManager.CombineTexturesNew(TextureManager.ConcreteFull, TextureManager.VerticalCurveInto);
        ConcreteHorizontalCurveInto = TextureManager.CombineTexturesNew(TextureManager.ConcreteFull, TextureManager.HorizontalCurveInto);

        ConcreteVerticalCurveOutOf = TextureManager.CombineTexturesNew(TextureManager.ConcreteFull, TextureManager.VerticalCurveOutOf);
        ConcreteHorizontalCurveOutOf = TextureManager.CombineTexturesNew(TextureManager.ConcreteFull, TextureManager.HorizontalCurveOutOf);

        ConcreteDiagonalHalf = TextureManager.CombineTexturesNew(TextureManager.ConcreteTri, TextureManager.DiagonalHalf);
        ConcreteDiagonalFull = TextureManager.CombineTexturesNew(TextureManager.ConcreteFull, TextureManager.DiagonalFull);

        AsphaltVerticalCurveInto = TextureManager.CombineTexturesNew(TextureManager.AsphaltFull, TextureManager.VerticalCurveInto);
        AsphaltHorizontalCurveInto = TextureManager.CombineTexturesNew(TextureManager.AsphaltFull, TextureManager.HorizontalCurveInto);

        AsphaltVerticalCurveOutOf = TextureManager.CombineTexturesNew(TextureManager.AsphaltFull, TextureManager.VerticalCurveOutOf);
        AsphaltHorizontalCurveOutOf = TextureManager.CombineTexturesNew(TextureManager.AsphaltFull, TextureManager.HorizontalCurveOutOf);

        AsphaltDiagonalHalf = TextureManager.CombineTexturesNew(TextureManager.AsphaltTri, TextureManager.DiagonalHalf);
        AsphaltDiagonalFull = TextureManager.CombineTexturesNew(TextureManager.AsphaltFull, TextureManager.DiagonalFull);

        AsphaltEndCap = TextureManager.CombineTexturesNew(TextureManager.AsphaltFull, TextureManager.EndCap);
        ConcreteEndCap = TextureManager.CombineTexturesNew(TextureManager.ConcreteFull, TextureManager.EndCap);


        TextureManager.AsphaltEntranceFast.wrapMode = TextureWrapMode.Clamp;
        AsphaltEntranceFast = Sprite.Create(TextureManager.AsphaltEntranceFast, new Rect(0, 0, TextureManager.AsphaltEntranceFast.width, TextureManager.AsphaltEntranceFast.height), Vector2.one / 2f, 128, 0u, SpriteMeshType.FullRect);

        TextureManager.AsphaltEntranceFastLarge.wrapMode = TextureWrapMode.Clamp;
        AsphaltEntranceFastLarge = Sprite.Create(TextureManager.AsphaltEntranceFastLarge, new Rect(0, 0, TextureManager.AsphaltEntranceFastLarge.width, TextureManager.AsphaltEntranceFastLarge.height), Vector2.one / 2f, 128, 0u, SpriteMeshType.FullRect);


        TextureManager.ConcreteEntranceFast.wrapMode = TextureWrapMode.Clamp;
        ConcreteEntranceFast = Sprite.Create(TextureManager.ConcreteEntranceFast, new Rect(0, 0, TextureManager.ConcreteEntranceFast.width, TextureManager.ConcreteEntranceFast.height), Vector2.one / 2f, 128, 0u, SpriteMeshType.FullRect);

        TextureManager.ConcreteEntranceFastLarge.wrapMode = TextureWrapMode.Clamp;
        ConcreteEntranceFastLarge = Sprite.Create(TextureManager.ConcreteEntranceFastLarge, new Rect(0, 0, TextureManager.ConcreteEntranceFastLarge.width, TextureManager.ConcreteEntranceFastLarge.height), Vector2.one / 2f, 128, 0u, SpriteMeshType.FullRect);

        TaxiwayNodeImageServer.SetSpecials();
    }

    internal static void ApplySmallTri(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);
        obj.GetChild(0).transform.Translate(new Vector3(0.75f, 0.25f));

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        //associatedTiles[node].quality = UpdateQualityWithLocalContext(node);
        //foundationType = node.foundationType;
        renderer.sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltDiagonalHalf : ConcreteDiagonalHalf;
        renderer.material = DataPlaceholderMaterials.Instance.externalSpritesMaterial;

        if (!AirportCEOTaxiwayImprovementConfig.SmoothDiagonals.Value)
        {
            return;
        }

        try
        {
            associatedTiles[node].quality = foundationType == Enums.FoundationType.Concrete ? (byte)5 : (byte)4;
            node.foundationType = foundationType;
        }
        catch
        {
            // This isn't really a bad issue so we won't log it for the sake of keeping logs clean
        }
    }
    internal static void ApplyFullDia(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);
        obj.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltDiagonalFull : ConcreteDiagonalFull;
    }
    internal static void ApplyHorizCurveInto(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);
        obj.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltHorizontalCurveInto : ConcreteHorizontalCurveInto;
    }


    internal static void ApplyVerticalCurveOut(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);
        obj.GetChild(0).Translate(new Vector3(-1, 1f));

        obj.GetComponent<SpriteRenderer>().sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltVerticalCurveOutOf : ConcreteVerticalCurveOutOf;
    }
    internal static void ApplyHorizontalCurveOut(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);
        obj.GetChild(0).Translate(new Vector3(0, 2));

        obj.GetComponent<SpriteRenderer>().sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltHorizontalCurveOutOf : ConcreteHorizontalCurveOutOf;
    }

    internal static void ApplyEndCap(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);
        obj.GetChild(0).transform.Translate(new Vector3(0, 2));

        obj.GetComponent<SpriteRenderer>().sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltEndCap : ConcreteEndCap;
    }

    internal static void ApplyVerticalCurveInto(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);
        obj.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        obj.GetComponent<SpriteRenderer>().sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltVerticalCurveInto : ConcreteVerticalCurveInto;
    }
    internal static void ApplyHorizontalCurveInto(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {
        Transform obj = GetBasicTile(foundationType, rotation, node);
        obj.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        obj.GetComponent<SpriteRenderer>().sprite = foundationType == Enums.FoundationType.Asphalt ? AsphaltHorizontalCurveInto : ConcreteHorizontalCurveInto;
    }
    private static Transform GetBasicTile(Enums.FoundationType foundationType, int rotation, TaxiwayBuilderNode node)
    {

        Transform obj = GameObject.Instantiate(SingletonNonDestroy<DataPlaceholderStructures>.Instance.GetTaxiwayEdge(Enums.FoundationType.Asphalt, Enums.TaxiwayBuilderType.StraightEdge), node.transform).transform;
        obj.eulerAngles = new Vector3(0f, 0f, rotation);
        obj.localPosition = new Vector3(0f, 0f, 0.001f);
        obj.localScale = new Vector3(scaleAdjustment, scaleAdjustment, obj.localScale.z);

        try
        {
            associatedTiles[node].quality = (byte)foundationType;
        }
        catch
        {
            // This isn't really a bad issue so we won't log it for the sake of keeping logs clean
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


	private static readonly DataPlaceholderStructures DPS = SingletonNonDestroy<DataPlaceholderStructures>.Instance;
    private static readonly int scaleAdjustment = DownCompressor.GetDownscaleInt();
}
