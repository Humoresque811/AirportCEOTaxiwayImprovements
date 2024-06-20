using Epic.OnlineServices.AntiCheatClient;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

internal class TextureManager
{
    internal static Texture2D VerticalCurveInto;
    internal static Texture2D HorizontalCurveInto;

    internal static Texture2D VerticalCurveOutOf;
    internal static Texture2D HorizontalCurveOutOf;

    internal static Texture2D DiagonalHalf;
    internal static Texture2D DiagonalFull;

    internal static Texture2D ConcreteFull;
    internal static Texture2D ConcreteTri;

    internal static Texture2D AsphaltFull;
    internal static Texture2D AsphaltTri;

    private static readonly string basePath = "C:\\My Stuff\\ACEO Texture Work\\Airport CEO Textures\\Rebuilt 45 Degree Mod\\AirporCEOTaxiwayImprovements";

    internal static void LoadTextures()
    {
        VerticalCurveInto = LoadTexture(Path.Combine(basePath, "VerticalCurveInto.png"));
        HorizontalCurveInto = LoadTexture(Path.Combine(basePath, "HorizontalCurveInto.png"));

        VerticalCurveOutOf = LoadTexture(Path.Combine(basePath, "VerticalCurveOutOf.png"));
        HorizontalCurveOutOf = LoadTexture(Path.Combine(basePath, "HorizontalCurveOutOf.png"));

        DiagonalHalf = LoadTexture(Path.Combine(basePath, "DiagonalHalf.png"));
        DiagonalFull = LoadTexture(Path.Combine(basePath, "DiagonalFull.png"));

        ConcreteFull = LoadTexture(Path.Combine(basePath, "ConcreteFull.png"));
        ConcreteTri = LoadTexture(Path.Combine(basePath, "ConcreteTri.png"));

        AsphaltFull = LoadTexture(Path.Combine(basePath, "AsphaltFull.png"));
        AsphaltTri = LoadTexture(Path.Combine(basePath, "AsphaltTri.png"));

        TextureRegistry.Init();
        AirportCEOTaxiwayImprovements.TILogger.LogMessage("Completed texture loading successfully!");
    }

    private static Texture2D LoadTexture(string filePath)
    {
        Texture2D result = null;
	    if (File.Exists(filePath))
	    {
		    byte[] data = File.ReadAllBytes(filePath);
		    Texture2D texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, false)
		    {
			    filterMode = FilterMode.Bilinear
		    };
		    texture2D.LoadImage(data);
		    if (GameSettingManager.CompressImages)
		    {
			    texture2D.Compress(highQuality: true);
		    }
            result = texture2D;
	    }
        else
        {
            AirportCEOTaxiwayImprovements.TILogger.LogError("File not found!");
        }
	    return result;
    }

    private static Dictionary<Texture2D, Dictionary<Texture2D, Sprite>> combineSpriteCache = new Dictionary<Texture2D, Dictionary<Texture2D, Sprite>>();

    internal static Sprite CombineTextureCache(ref Texture2D baseTex, ref Texture2D topTex)
    {
        try
        {
            if (combineSpriteCache.ContainsKey(baseTex))
            {
                if (combineSpriteCache[baseTex].ContainsKey(topTex))
                {
                    return combineSpriteCache[baseTex][topTex];
                }
            }
        }
        catch
        {
            // Keep goin
        }

        Sprite sprite = CombineTextures(baseTex, topTex);
        combineSpriteCache[baseTex] = new Dictionary<Texture2D, Sprite>();
        combineSpriteCache[baseTex][topTex] = sprite;
        return sprite;
    }

    internal static Sprite CombineTextures(Texture2D baseTex, Texture2D topTex)
    {
        Resources.UnloadUnusedAssets();

        if (baseTex.width != topTex.width || baseTex.height != topTex.height)
        {
            return Sprite.Create(baseTex, new Rect(0, 0, baseTex.width, baseTex.height), Vector2.one / 2f, 100, 0u, SpriteMeshType.FullRect);
        }

        Texture2D emptyTex = new Texture2D(baseTex.width, baseTex.height);
        for (int x1 = 0; x1 < emptyTex.width; x1++)
        {
            for (int y1 = 0; y1 < emptyTex.height; y1++)
            {
                emptyTex.SetPixel(x1, y1, Color.clear);
            }
        }


        for (int x = 0; x < baseTex.width; x++)
        {
            for (int y = 0; y < baseTex.height; y++)
            {
                if (topTex.GetPixel(x, y).a != 0)
                {
                    emptyTex.SetPixel(x, y, topTex.GetPixel(x, y));
                    continue;
                }
                if (baseTex.GetPixel(x, y).a != 0)
                {
                    emptyTex.SetPixel(x, y, baseTex.GetPixel(x, y));
                    continue;
                }

                emptyTex.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }

        emptyTex.Apply(true, true);
        return Sprite.Create(emptyTex, new Rect(0, 0, emptyTex.width, emptyTex.height), Vector2.one / 2f, 256, 0u, SpriteMeshType.FullRect);
    }
}
