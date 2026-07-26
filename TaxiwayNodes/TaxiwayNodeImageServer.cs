using AirportCEOModLoader.Core;
using AirportCEOTaxiwayImprovements.TextureManagment;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements.TaxiwayNodes;

internal static class TaxiwayNodeImageServer
{
    private static readonly Color TaxiwayNodeGold = new Color(0.906f, 0.718f, 0.247f, 1);

    internal static Sprite GetPotentialSprite(TaxiwayCenterBuilderSupplement builderSupplement, out int rotation)
	{
		try
		{
			// We know this configuration does not exist anywhere!
			List<TextureConfiguration> texturesApplicable = new();
			List<SimpleTexture> adjustedTextures = new();

			// Is this messy? Yes. I like the switch format but to allow it to hit multiple cases I need to loop over it and see how many it matches 
			bool flipHorizontally = false;
			for (int i = 0; i < 4; i++)
			{
				if (GetMatchingTexture(builderSupplement, i, flipHorizontally, out List<Texture2D> outputTexes))
				{
					foreach (Texture2D tex in outputTexes)
					{
						texturesApplicable.Add(new TextureConfiguration(tex, i, flipHorizontally));
					}
				}
			}

			flipHorizontally = true;

			for (int i = 0; i < 4; i++)
			{
				if (GetMatchingTexture(builderSupplement, i, flipHorizontally, out List<Texture2D> outputTexes))
				{
					foreach (Texture2D tex in outputTexes)
					{
						texturesApplicable.Add(new TextureConfiguration(tex, i, flipHorizontally));
					}
				}
			}

			if (texturesApplicable.Count == 0)
			{
				// Nothing matches this layout at all
				rotation = 0;
				return null;
			}

			foreach (TextureConfiguration configTexture in texturesApplicable)
			{
				adjustedTextures.Add(configTexture); // Implicit conversion behind the scenes here - rotation and flip are baked!
			}


			Sprite newSprite = null;
			newSprite = CombineTexturesNew(adjustedTextures.ToArray());

			rotation = 0;
			return newSprite;
		}
		catch (Exception ex)
		{
			AirportCEOTaxiwayImprovements.TILogger.LogError($"Taxiway Node Sprite generation code failed. {ExceptionUtils.ProccessException(ex)}");
			rotation = 0;
			return null;
		}
	}

	private static bool GetMatchingTexture(TaxiwayCenterBuilderSupplement sup, int rot, bool flip, out List<Texture2D> outputTex)
    {
		bool matched = false;
		outputTex = new List<Texture2D>();

		if (ShouldConnectTo(sup, rot, flip, (-1, -1), (1, -1)) && MatchExists(sup, rot, flip, (0, -1)))
		{
			outputTex.Add(TextureLoader.Node_4545_Curve);
            matched = true;
		}
		if (ShouldConnectTo(sup, rot, flip, (-1, 1), (1, -1)))
		{
			outputTex.Add(TextureLoader.Node_4545_Straight);
            matched = true;
		}
		if (ShouldConnectTo(sup, rot, flip, (-1, 0), (1, -1)))
		{
			outputTex.Add(TextureLoader.Node_9045);
            matched = true;
		}
		if (ShouldConnectTo(sup, rot, flip, (-1, 0), (1, 0)))
		{
			outputTex.Add(TextureLoader.Node_9090);
            matched = true;
		}

		return matched;
    }

	private static bool ShouldConnectTo(TaxiwayCenterBuilderSupplement sup, int rotation, bool isFlipped, params (int, int)[] translationsToCheck)
	{
		int[] indicesToCheck = NormalizeTranslationsIntoIndices(translationsToCheck, rotation, isFlipped);

		foreach (int index in indicesToCheck)
		{
			if (!sup.ShouldConnectTo(index))
			{
				return false;
			}
        }

		return true;
    }
	private static bool MatchExists(TaxiwayCenterBuilderSupplement sup, int rotation, bool isFlipped, params (int, int)[] translationsToCheck)
	{
		int[] indicesToCheck = NormalizeTranslationsIntoIndices(translationsToCheck, rotation, isFlipped);

		foreach (int index in indicesToCheck)
		{
			if (sup.HasConnectionAt(index))
			{
				return false;
			}
        }

		return true;
    }

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

	private static int[] NormalizeTranslationsIntoIndices((int, int)[] translations, int rotation, bool isFlipped)
	{
		(int, int)[] normalizedTranslations = new (int, int)[translations.Length];

        for (int i = 0; i < translations.Length; i++)
        {
			normalizedTranslations[i] = translations[i];

			if (isFlipped)
			{
				normalizedTranslations[i] = (-normalizedTranslations[i].Item1, normalizedTranslations[i].Item2);

				if (rotation == 1)
				{
					normalizedTranslations[i] = (normalizedTranslations[i].Item2, -normalizedTranslations[i].Item1);
				}
				else if (rotation == 3)
				{
					normalizedTranslations[i] = (-normalizedTranslations[i].Item2, normalizedTranslations[i].Item1);
				}
			}
			else
			{
				if (rotation == 3)
				{
					normalizedTranslations[i] = (normalizedTranslations[i].Item2, -normalizedTranslations[i].Item1);
				}
				else if (rotation == 1)
				{
					normalizedTranslations[i] = (-normalizedTranslations[i].Item2, normalizedTranslations[i].Item1);
				}
			}


			if (rotation == 2)
			{
				normalizedTranslations[i] = (-normalizedTranslations[i].Item1, -normalizedTranslations[i].Item2);
			}
        }

		// Now we have list of normalized translations, we need to convert them to indices
		int[] result = new int[normalizedTranslations.Length];
        for (int i = 0; i < normalizedTranslations.Length; i++)
		{
            result[i] = GetIndexFromTranslation((Mathf.RoundToInt(normalizedTranslations[i].Item1), Mathf.RoundToInt(normalizedTranslations[i].Item2)));
        }

		return result;
    }

	internal static int GetIndexFromTranslation((int, int) translation)
    {
        switch (translation)
		{
			case (0, 1):
                return 0;
			case (1, 0):
				return 1;
			case (0, -1):
				return 2;
			case (-1, 0):
                return 3;
			case (1, 1):
                return 4;
			case (1, -1):
                return 5;
			case (-1, -1):
                return 6;
			case (-1, 1):
                return 7;
			default:
				return 0;
        }
	}
	internal static (int, int) GetTranslationFromIndex(int index)
    {
        switch (index)
		{
			case 0:
                return (0, 1);
			case 1:
				return (1, 0);
			case 2:
				return (0, -1);
			case 3:
                return (-1, 0);
			case 4:
                return (1, 1);
			case 5:
                return (1, -1);
			case 6:
                return (-1, -1);
			case 7:
                return (-1, 1);
			default:
				return (0, 0);
        }
	}

	internal static int GetOppositeIndex(int index)
    {
        switch (index)
        {
            case 0:
                return 2;
            case 1:
                return 3;
            case 2:
                return 0;
            case 3:
                return 1;
            case 4:
                return 6;
            case 5:
                return 7;
            case 6:
                return 4;
            case 7:
                return 5;
            default:
                return 0;
        }
    }

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
    }}
