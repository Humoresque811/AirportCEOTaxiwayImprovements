using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

internal struct SimpleTexture
{
    internal int width;
    internal int height;
    internal Color[] texture;

    public static implicit operator SimpleTexture(Texture2D texture2D)
    {
        return new SimpleTexture()
        {
            width = texture2D.width,
            height = texture2D.height,
            texture = texture2D.GetPixels(),
        };
    }

    public static implicit operator SimpleTexture(TextureConfiguration textureConfig)
    {
		SimpleTexture finalTexture = textureConfig.textureReference;
        finalTexture = finalTexture.RotateTextureCounterclockwise(textureConfig.rotationIndex);
		if (textureConfig.flipHorizontally)
		{
            return finalTexture.FlipHorizontally();
		}
        return finalTexture;
    }

    internal SimpleTexture(int inputWidth, int inputHeight)
    {
        width = inputWidth;
        height = inputHeight;
        texture = new Color[inputWidth * inputHeight];
    }

    internal SimpleTexture FlipHorizontally()
    {
        SimpleTexture output = new(width, height);
        for (int j = 0; j < height; j++)
        {
            int rowStart = 0;
            int rowEnd = width - 1;
    
            while (rowStart < rowEnd)
            {
                Color hold = texture[(j * width) + (rowStart)];
                output.texture[(j * width) + (rowStart)] = texture[(j * width) + (rowEnd)];
                output.texture[(j * width) + (rowEnd)] = hold;
                rowStart++;
                rowEnd--;
            }
        }
                  
        return output;
    }

    internal SimpleTexture RotateTextureCounterclockwise(int times)
    {
        if (times == 0)
        {
            return this;
        }

        SimpleTexture output = new(width, height);
        int w = width;
        int h = height;

        times = times.Clamp(1, 3);

        if (times == 1 || times == 3)
        {
            int iRotated, iOriginal;

            for (int j = 0; j < h; ++j)
            {
                for (int i = 0; i < w; i++)
                {
                    iRotated = (i + 1) * h - j - 1;
                    iOriginal = times == 3 ? texture.Length - 1 - (j * w + i) : j * w + i;
                    output.texture[iRotated] = texture[iOriginal];
                }
            }
        }
        if (times == 2)
        {
            int iRotated, iOriginal;

            for (int i = 0; i < texture.Length; ++i)
            {
                iRotated = (int)(i - (i - ((float)(texture.Length - 2) / 2f) - 0.5f) * 2);
                iOriginal = i;
                output.texture[iRotated] = texture[iOriginal];
            }
        }

        return output;
    }

    internal Sprite ToSprite()
    {
        Texture2D emptyTex = new Texture2D(width, height);
        emptyTex.SetPixels(texture);
        emptyTex.Apply(true, true);
        emptyTex.filterMode = FilterMode.Bilinear;
        emptyTex.wrapMode = TextureWrapMode.Clamp;
        return Sprite.Create(emptyTex, new Rect(0, 0, emptyTex.width, emptyTex.height), Vector2.one / 2f, 256, 0u, SpriteMeshType.FullRect);
    }
}

internal readonly struct TextureConfiguration
{
	internal TextureConfiguration(Texture2D tex, int rotation, bool flip)
	{
		textureReference = tex;
		rotationIndex = rotation;
		flipHorizontally = flip;
	}

	internal readonly Texture2D textureReference;
	internal readonly int rotationIndex;
	internal readonly bool flipHorizontally;
}

internal struct NodeConfiguration
{
	internal NodeConfiguration(bool[] configuration)
	{
		Connections = configuration;
	}

    readonly internal bool Equals(NodeConfiguration other)
    {
        if (Connections.Length != other.Connections.Length)
        {
            return false;
        }

        for (int i = 0; i < other.Connections.Length; i++)
        {
            if (other.Connections[i] != Connections[i])
            {
                return false;
            }
        }

        return true;
    }

	internal bool[] Connections { get; private set; }
}
