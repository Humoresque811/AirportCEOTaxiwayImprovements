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

    internal static Texture2D AsphaltEntranceFast;
    internal static Texture2D AsphaltEntranceFastLarge;

    internal static Texture2D ConcreteEntranceFast;
    internal static Texture2D ConcreteEntranceFastLarge;

    internal static Texture2D EndCap;

    internal static void LoadTextures(string directoryPath)
    {
        if (!string.IsNullOrEmpty(AirportCEOTaxiwayImprovementConfig.AlternateLoadingPath.Value))
        {
            directoryPath = AirportCEOTaxiwayImprovementConfig.AlternateLoadingPath.Value.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar).Trim(' ');
        }

        AirportCEOTaxiwayImprovements.TILogger.LogInfo($"Path to load textures {directoryPath}");

        VerticalCurveInto = LoadTexture(Path.Combine(directoryPath, "VerticalCurveInto.png"));
        HorizontalCurveInto = LoadTexture(Path.Combine(directoryPath, "HorizontalCurveInto.png"));

        VerticalCurveOutOf = LoadTexture(Path.Combine(directoryPath, "VerticalCurveOutOf.png"));
        HorizontalCurveOutOf = LoadTexture(Path.Combine(directoryPath, "HorizontalCurveOutOf.png"));

        DiagonalHalf = LoadTexture(Path.Combine(directoryPath, "DiagonalHalf.png"));
        DiagonalFull = LoadTexture(Path.Combine(directoryPath, "DiagonalFull.png"));

        ConcreteFull = LoadTexture(Path.Combine(directoryPath, "ConcreteFull.png"));
        ConcreteTri = LoadTexture(Path.Combine(directoryPath, "ConcreteTri.png"));

        AsphaltFull = LoadTexture(Path.Combine(directoryPath, "AsphaltFull.png"));
        AsphaltTri = LoadTexture(Path.Combine(directoryPath, "AsphaltTri.png"));

        EndCap = LoadTexture(Path.Combine(directoryPath, "EndCap.png"));

        AsphaltEntranceFast = LoadTexture(Path.Combine(directoryPath, "FastAsphalt.png"));
        AsphaltEntranceFastLarge = LoadTexture(Path.Combine(directoryPath, "FastAsphaltLarge.png"));
        ConcreteEntranceFast = LoadTexture(Path.Combine(directoryPath, "FastConcrete.png"));
        ConcreteEntranceFastLarge = LoadTexture(Path.Combine(directoryPath, "FastConcreteLarge.png"));

        TextureRegistry.Init();
        AirportCEOTaxiwayImprovements.TILogger.LogMessage("Completed texture loading successfully!");
    }

    private static Texture2D LoadTexture(string filePath)
    {
        Texture2D result = null;
	    if (File.Exists(filePath))
	    {
		    byte[] data = File.ReadAllBytes(filePath);
		    Texture2D texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, true)
		    {
			    filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                loadAllMips = true,
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
        emptyTex.filterMode = FilterMode.Bilinear;
        emptyTex.wrapMode = TextureWrapMode.Clamp;
        Sprite sprite = Sprite.Create(emptyTex, new Rect(0, 0, emptyTex.width, emptyTex.height), Vector2.one / 2f, 256, 0u, SpriteMeshType.FullRect);
        return sprite;
    }
}
