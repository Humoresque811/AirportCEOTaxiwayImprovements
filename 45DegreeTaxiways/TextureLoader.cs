using Epic.OnlineServices.AntiCheatClient;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

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

    // Old Textures ----------------------------------

    internal static Texture2D VerticalCurveInto;
    internal static Texture2D HorizontalCurveInto;

    internal static Texture2D VerticalCurveOutOf;
    internal static Texture2D HorizontalCurveOutOf;

    internal static Texture2D DiagonalHalf;
    internal static Texture2D DiagonalFull;

    internal static Texture2D EndCap;

    internal static Texture2D ConcreteFull;
    internal static Texture2D ConcreteTri;

    internal static Texture2D AsphaltFull;
    internal static Texture2D AsphaltTri;

    // Runway Exits

    internal static Texture2D AsphaltEntranceFast;
    internal static Texture2D AsphaltEntranceFastLarge;

    internal static Texture2D ConcreteEntranceFast;
    internal static Texture2D ConcreteEntranceFastLarge;


    internal static void LoadTextures(string directoryPath)
    {
        AirportCEOTaxiwayImprovements.debugStopwatch.Start();
        if (!string.IsNullOrEmpty(AirportCEOTaxiwayImprovementConfig.AlternateLoadingPath.Value))
        {
            directoryPath = AirportCEOTaxiwayImprovementConfig.AlternateLoadingPath.Value.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar).Trim(' ');
        }

        AirportCEOTaxiwayImprovements.TILogger.LogInfo($"Path to load textures {directoryPath}");

        string taxiwayEdgePath = Path.Combine(directoryPath, "TaxiwayEdges");
        string runwayExitPath = Path.Combine(directoryPath, "RunwayExits");

        A_VerticalCurveInto = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_VerticalCurveInto.dds"));
        A_HorizontalCurveInto = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_HorizontalCurveInto.dds"));

        A_VerticalCurveOutOf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_VerticalCurveOutOf.dds"));
        A_HorizontalCurveOutOf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_HorizontalCurveOutOf.dds"));

        A_DiagonalHalf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_DiagonalHalf.dds"));
        A_DiagonalFull = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_DiagonalFull.dds"));

        A_EndCap = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "A_EndCap.dds"));

        C_VerticalCurveInto = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_VerticalCurveInto.dds"));
        C_HorizontalCurveInto = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_HorizontalCurveInto.dds"));

        C_VerticalCurveOutOf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_VerticalCurveOutOf.dds"));
        C_HorizontalCurveOutOf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_HorizontalCurveOutOf.dds"));

        C_DiagonalHalf = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_DiagonalHalf.dds"));
        C_DiagonalFull = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_DiagonalFull.dds"));

        C_EndCap = LoadTextureDDS(Path.Combine(taxiwayEdgePath, "C_EndCap.dds"));

        //VerticalCurveInto = LoadTexture(Path.Combine(directoryPath, "VerticalCurveInto.png"));
        //HorizontalCurveInto = LoadTexture(Path.Combine(directoryPath, "HorizontalCurveInto.png"));

        //VerticalCurveOutOf = LoadTexture(Path.Combine(directoryPath, "VerticalCurveOutOf.png"));
        //HorizontalCurveOutOf = LoadTexture(Path.Combine(directoryPath, "HorizontalCurveOutOf.png"));

        //DiagonalHalf = LoadTexture(Path.Combine(directoryPath, "DiagonalHalf.png"));
        //DiagonalFull = LoadTexture(Path.Combine(directoryPath, "DiagonalFull.png"));

        //ConcreteFull = LoadTexture(Path.Combine(directoryPath, "ConcreteFull.png"));
        //ConcreteTri = LoadTexture(Path.Combine(directoryPath, "ConcreteTri.png"));

        //AsphaltFull = LoadTexture(Path.Combine(directoryPath, "AsphaltFull.png"));
        //AsphaltTri = LoadTexture(Path.Combine(directoryPath, "AsphaltTri.png"));

        //EndCap = LoadTexture(Path.Combine(directoryPath, "EndCap.png"));

        AsphaltEntranceFast = LoadTexture(Path.Combine(runwayExitPath, "FastAsphalt.png"));
        AsphaltEntranceFastLarge = LoadTexture(Path.Combine(runwayExitPath, "FastAsphaltLarge.png"));
        ConcreteEntranceFast = LoadTexture(Path.Combine(runwayExitPath, "FastConcrete.png"));
        ConcreteEntranceFastLarge = LoadTexture(Path.Combine(runwayExitPath, "FastConcreteLarge.png"));

        TextureRegistry.Init();
        AirportCEOTaxiwayImprovements.debugStopwatch.Stop();

        AirportCEOTaxiwayImprovements.TILogger.LogMessage($"Completed texture loading successfully in {AirportCEOTaxiwayImprovements.debugStopwatch.ElapsedMilliseconds}ms!");
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
            mipMapBias = AirportCEOTaxiwayImprovementConfig.MipMapBias.Value
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
