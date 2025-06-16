using AirportCEOModLoader.Core;
using BepInEx;
using Epic.OnlineServices.AntiCheatClient;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

internal static class TextureManager
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

    // Taxiway Nodes

    internal static Texture2D Straight_45;
    internal static Texture2D Straight_90;

    internal static Texture2D Curve_9090_P1;
    internal static Texture2D Curve_9090_P2;

    internal static Texture2D Curve_4590_P1;
    internal static Texture2D Curve_4590_P2;
    internal static Texture2D Curve_4590_P3;
    internal static Texture2D Curve_4590_P4;
    internal static Texture2D Curve_4590_P5;

    internal static Texture2D Clear;

    internal static Texture2D Node_Light_90;
    internal static Texture2D Node_Light_45;

    private static readonly Color TaxiwayNodeGold = new Color(0.906f, 0.718f, 0.247f, 1);

    // Other
    internal static Texture2D planeCrossing;

    private static string directoryPathSaved;
    internal static void SaveTexturePath(string directoryPath)
    {
        directoryPathSaved = directoryPath;
    }

    internal static void DoAllChanges(SaveLoadGameDataController _)
    {
        if (directoryPathSaved.IsNullOrWhiteSpace())
        {
            AirportCEOTaxiwayImprovements.TILogger.LogError("Textures not availible to be loaded by time they need to be?");
        }

        LoadTextures(directoryPathSaved);
        AdditionalPrefabChanges.DoModifications(null);
    }

    internal static void LoadTextures(string directoryPath)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
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

        if (AirportCEOTaxiwayImprovementConfig.SmoothTaxiwayNodes.Value)
        {
            LoadTaxiwayNodeTextures(directoryPath);
        }
        if (AirportCEOTaxiwayImprovementConfig.ImproveRoadMarkings.Value)
        {
            LoadRoadMarkingTextures(directoryPath);
        }

        TextureRegistry.Init();
        stopwatch.Stop();
        AirportCEOTaxiwayImprovements.TILogger.LogMessage($"Completed texture loading successfully, which took {stopwatch.ElapsedMilliseconds}ms. " +
            $"Downscale setting: {AirportCEOTaxiwayImprovementConfig.DownscaleLevel.Value}");
    }

    private static void LoadRoadMarkingTextures(string directoryPath)
    {
        directoryPath = "C:\\My Stuff\\ACEO Texture Work\\Airport CEO Textures\\Rebuilt 45 Degree Mod\\AirporCEOTaxiwayImprovements\\roadIcons";
        planeCrossing = LoadTexture(Path.Combine(directoryPath, "planeCrossing" + ".png"));
    }

    private static void LoadTaxiwayNodeTextures(string directoryPath)
    {
        directoryPath = "C:\\My Stuff\\ACEO Texture Work\\Airport CEO Textures\\Rebuilt 45 Degree Mod\\AirporCEOTaxiwayImprovements\\nodes";
        Straight_90 = LoadTexture(Path.Combine(directoryPath, nameof(Straight_90) + ".png")); 
        Straight_45 = LoadTexture(Path.Combine(directoryPath, nameof(Straight_45) + ".png")); 
        Curve_4590_P1 = LoadTexture(Path.Combine(directoryPath, nameof(Curve_4590_P1) + ".png")); 
        Curve_4590_P2 = LoadTexture(Path.Combine(directoryPath, nameof(Curve_4590_P2) + ".png")); 
        Curve_4590_P3 = LoadTexture(Path.Combine(directoryPath, nameof(Curve_4590_P3) + ".png")); 
        Curve_4590_P4 = LoadTexture(Path.Combine(directoryPath, nameof(Curve_4590_P4) + ".png")); 
        Curve_4590_P5 = LoadTexture(Path.Combine(directoryPath, nameof(Curve_4590_P5) + ".png")); 

        Curve_9090_P1 = LoadTexture(Path.Combine(directoryPath, nameof(Curve_9090_P1) + ".png")); 
        Curve_9090_P2 = LoadTexture(Path.Combine(directoryPath, nameof(Curve_9090_P2) + ".png"));

        Node_Light_45 = LoadTexture(Path.Combine(directoryPath, nameof(Node_Light_45) + ".png")); 
        Node_Light_90 = LoadTexture(Path.Combine(directoryPath, nameof(Node_Light_90) + ".png"));

        Clear = new Texture2D(640, 640);
        Color[] veryClear = new Color[640 * 640];
        for (int i = 0; i < veryClear.Length; i++)
        {
            veryClear[i] = Color.clear;
        }
        Clear.SetPixels(veryClear);
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
            texture2D = DownCompressor.DownscaleTextureFastGPU(texture2D);
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

    //internal static Sprite CombineTexturesNew(params SimpleTexture[] textures)
    //{
    //    return CombineTexturesNew(false, textures);
    //}

    internal static Sprite CombineTexturesNew(params SimpleTexture[] textures)
    {
        foreach (SimpleTexture simpleTexture in textures)
        {
            if (simpleTexture.width == textures[0].width && simpleTexture.height == textures[0].height && simpleTexture.texture.Length == textures[0].texture.Length)
            {
                continue;
            }

            AirportCEOTaxiwayImprovements.TILogger.LogError($"Textures not the same size in {nameof(CombineTexturesNew)}. Either I messed up or someone is messing with my textures!");
            return null;
        }

        SimpleTexture outputTexture = CombineTexturesInternal(textures);

        return outputTexture.ToSprite();
    }

    internal static Sprite CombineTexturesWithNodeLight(SimpleTexture light, SimpleTexture[] textures)
    {
        foreach (SimpleTexture simpleTexture in textures)
        {
            if (simpleTexture.width == textures[0].width && simpleTexture.height == textures[0].height && simpleTexture.texture.Length == textures[0].texture.Length)
            {
                continue;
            }

            AirportCEOTaxiwayImprovements.TILogger.LogError($"Textures not the same size in {nameof(CombineTexturesNew)}. Either I messed up or someone is messing with my textures!");
            return null;
        }

        SimpleTexture outputTexture = CombineTexturesInternal(textures);

        for (int i = 0; i < light.texture.Length; i++)
        {
            if (light.texture[i].a == 1)
            {
                outputTexture.texture[i] = light.texture[i];
                continue;
            }
        }

        return outputTexture.ToSprite();
    }

    private static SimpleTexture CombineTexturesInternal(SimpleTexture[] textures)
    {
        SimpleTexture outputTexture = new SimpleTexture(textures[0].width, textures[0].height);


        foreach (SimpleTexture nextLayerTexture in textures)
        {
            for (int i = 0; i < nextLayerTexture.texture.Length; i++)
            {
                if (nextLayerTexture.texture[i].a == 1 && !outputTexture.texture[i].CloseEquals(TaxiwayNodeGold))
                {
                    outputTexture.texture[i] = nextLayerTexture.texture[i];
                    continue;
                }
                if (nextLayerTexture.texture[i].a > 0 && !outputTexture.texture[i].CloseEquals(TaxiwayNodeGold))
                {
                    Color newColor = nextLayerTexture.texture[i];
                    newColor.a = Mathf.Min(1, newColor.a + outputTexture.texture[i].a);
                    outputTexture.texture[i] = newColor;
                    continue;
                }
            }
        }

        return outputTexture;
    }

    internal static Sprite CreateSingleTexture(Texture2D baseTex)
    {
        baseTex.Apply(true, true);
        baseTex.filterMode = FilterMode.Bilinear;
        baseTex.wrapMode = TextureWrapMode.Clamp;
        Sprite sprite = Sprite.Create(baseTex, new Rect(0, 0, baseTex.width, baseTex.height), Vector2.one / 2f, 256, 0u, SpriteMeshType.FullRect);
        return sprite;
    }


    // Extensions down here:
    public static bool CloseEquals(this Color color1, Color color2)
    {
        if (color1.r > color2.r + 0.01f || color1.r < color2.r - 0.01f)
        {
            return false;
        }
        if (color1.g > color2.g + 0.01f || color1.g < color2.g - 0.01f)
        {
            return false;
        }
        if (color1.b > color2.b + 0.01f || color1.b < color2.b - 0.01f)
        {
            return false;
        }
        return true;
    }

    public static SimpleTexture ConvertFlipHorizontally(this Texture2D texture2D)
    {
        SimpleTexture simpleTexture = texture2D;
        return simpleTexture.FlipHorizontally();
    }
    public static SimpleTexture RotateTextureCounterclockwise(this Texture2D texture2D, int times)
    {
        SimpleTexture simpleTexture = texture2D;
        return simpleTexture.RotateTextureCounterclockwise(times);
    }
}
