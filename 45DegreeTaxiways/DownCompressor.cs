using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

internal class DownCompressor
{
    internal enum DownscaleLevel
    {
        [Description("Full Quality - Recommended")]
        Original,
        [Description("Downscale2X")]
        Downscale2X,
    }

    internal static int GetDownscaleInt()
    {
        int downscaleAmount = 1;
        switch (AirportCEOTaxiwayImprovementConfig.DownscaleLevel.Value)
        {
            case DownscaleLevel.Original:
                downscaleAmount = 1;
                break;
            case DownscaleLevel.Downscale2X:
                downscaleAmount = 2;
                break;
        };
        return downscaleAmount;
    }

    internal static Texture2D DownscaleTextureFastGPU(Texture2D source)
	{
        if (AirportCEOTaxiwayImprovementConfig.DownscaleLevel.Value == DownscaleLevel.Original)
        {
            return source;
        }

        int newWidth = source.width / GetDownscaleInt();
        int newHeight = source.height / GetDownscaleInt();

		RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);

		RenderTexture.active = rt;

		Graphics.Blit(source, rt);
		source.Resize(newWidth, newHeight, TextureFormat.ARGB32, false);
		source.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0,0);
		source.Apply();
		RenderTexture.active = null;
		RenderTexture.ReleaseTemporary(rt);
		return source;
	}
}
