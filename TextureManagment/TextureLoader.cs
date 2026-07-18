using AirportCEOModLoader.SaveLoadUtils;
using Epic.OnlineServices.AntiCheatClient;
using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements.TextureManagment;

internal class TextureLoader
{
    // New Textures

    internal static Texture2D A_VerticalCurveInto;
    internal static Texture2D A_HorizontalCurveInto;

    internal static Texture2D A_VerticalCurveOutOf;
    internal static Texture2D A_HorizontalCurveOutOf;

    internal static Texture2D A_DiagonalHalf;
    internal static Texture2D A_DiagonalFull;

    internal static Texture2D A_EndCap;
    // . . . . . . . . . . Concrete . . . . . . . . . . 
    internal static Texture2D C_VerticalCurveInto;
    internal static Texture2D C_HorizontalCurveInto;

    internal static Texture2D C_VerticalCurveOutOf;
    internal static Texture2D C_HorizontalCurveOutOf;

    internal static Texture2D C_DiagonalHalf;
    internal static Texture2D C_DiagonalFull;

    internal static Texture2D C_EndCap;

    // Road Markings
    internal static Texture2D PlaneCrossing;

    // Old Textures ----------------------------------
    // Runway Exits

    internal static Texture2D AsphaltEntranceFast;
    internal static Texture2D AsphaltEntranceFastLarge;

    internal static Texture2D ConcreteEntranceFast;
    internal static Texture2D ConcreteEntranceFastLarge;


    internal static IEnumerator LoadTextures(string directoryPath)
    {
        CoroutineEventDispatcher.GetTextUpdater()($"{AirportCEOTaxiwayImprovements.MODNAME}: Loading Textures...", 40);
        yield return null;

        AirportCEOTaxiwayImprovements.debugStopwatch.Start();
        if (!string.IsNullOrEmpty(AirportCEOTaxiwayImprovementConfig.AlternateLoadingPath.Value))
        {
            directoryPath = AirportCEOTaxiwayImprovementConfig.AlternateLoadingPath.Value.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar).Trim(' ');
        }

        AirportCEOTaxiwayImprovements.TILogger.LogInfo($"Path to load textures {directoryPath}");

        string taxiwayEdgePath = Path.Combine(directoryPath, "TaxiwayEdges");
        string runwayExitPath = Path.Combine(directoryPath, "RunwayExits");
        string roadMarkingsPath = Path.Combine(directoryPath, "RoadMarkings");

        yield return null;

        A_VerticalCurveInto = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_VerticalCurveIntoF.dds"));
        A_HorizontalCurveInto = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_HorizontalCurveIntoF.dds"));

        A_VerticalCurveOutOf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_VerticalCurveOutOfF.dds"));
        A_HorizontalCurveOutOf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_HorizontalCurveOutOfF.dds"));

        A_DiagonalHalf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_DiagonalHalfF.dds"));
        A_DiagonalFull = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_DiagonalFullF.dds"));

        A_EndCap = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_EndCapF.dds"));

        yield return null;

        C_VerticalCurveInto = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_VerticalCurveIntoF.dds"));
        C_HorizontalCurveInto = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_HorizontalCurveIntoF.dds"));

        C_VerticalCurveOutOf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_VerticalCurveOutOfF.dds"));
        C_HorizontalCurveOutOf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_HorizontalCurveOutOfF.dds"));

        C_DiagonalHalf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_DiagonalHalfF.dds"));
        C_DiagonalFull = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_DiagonalFullF.dds"));

        C_EndCap = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_EndCapF.dds"));

        yield return null;

        AsphaltEntranceFast = LoadTexture(Path.Combine(runwayExitPath, "FastAsphalt.png"));
        AsphaltEntranceFastLarge = LoadTexture(Path.Combine(runwayExitPath, "FastAsphaltLarge.png"));
        ConcreteEntranceFast = LoadTexture(Path.Combine(runwayExitPath, "FastConcrete.png"));
        ConcreteEntranceFastLarge = LoadTexture(Path.Combine(runwayExitPath, "FastConcreteLarge.png"));

        yield return null;
        PlaneCrossing = LoadTextureDDS(Path.Combine(roadMarkingsPath, "PlaneCrossing.dds"));

        CoroutineEventDispatcher.GetTextUpdater()($"{AirportCEOTaxiwayImprovements.MODNAME}: Processing Textures...", 80);
        yield return null;

        TextureRegistry.Init();
        AirportCEOTaxiwayImprovements.debugStopwatch.Stop();

        AirportCEOTaxiwayImprovements.TILogger.LogMessage($"Completed texture loading successfully in {AirportCEOTaxiwayImprovements.debugStopwatch.ElapsedMilliseconds}ms!");
        CoroutineEventDispatcher.GetTextUpdater()($"{AirportCEOTaxiwayImprovements.MODNAME}: Loading Complete", 100);
        yield return new WaitForSecondsRealtime(0.2f);
    }

    private static Texture2D LoadTextureDDS(string filePath)
    {
        if (!File.Exists(filePath))
        {
            AirportCEOTaxiwayImprovements.TILogger.LogError($"Texture file {filePath} does not exist! Errors will occur.");
            return null;
        }

        byte[] file = File.ReadAllBytes(filePath);
        const int headerStart = 4;

        int height = BitConverter.ToInt32(file, headerStart + 8);
        int width = BitConverter.ToInt32(file, headerStart + 12);
    
        int mipCount = BitConverter.ToInt32(file, headerStart + 24);
        if (mipCount < 1)
        {
            mipCount = 1;
        }

        string fourCC = Encoding.ASCII.GetString(file, 84, 4);
        
        TextureFormat format = fourCC switch
        {
            "DXT1" => TextureFormat.DXT1,
            "DXT5" => TextureFormat.DXT5,
            _ => throw new Exception($"Unsupported DDS format '{fourCC}'.")
        };

        Texture2D tex = new Texture2D(width, height, format, mipCount > 1)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Trilinear,
        };

        const int dataOffset = 128;

        byte[] textureData = new byte[file.Length - dataOffset];
        Buffer.BlockCopy(file, dataOffset, textureData, 0, textureData.Length);
        tex.LoadRawTextureData(textureData);

        tex.Apply(true, true);

        return tex;    
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