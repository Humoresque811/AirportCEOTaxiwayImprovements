using AirportCEOModLoader.Core;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

internal static class TaxiwayNodeImageServer
{
	private static readonly Dictionary<List<TextureConfiguration>, Sprite> cachedSprites = new();

	internal static readonly int connectionArrayLength = 56;

    internal static Sprite GetPotentialSprite(Vector2 nodePosition, out int rotation)
	{
		try
		{
			Stopwatch stopper1 = new();
			Stopwatch stopper2 = new();

			stopper1.Start();

			// We know this configuration does not exist anywhere!
			List<TextureConfiguration> texturesApplicable = new();
			List<SimpleTexture> adjustedTextures = new();

			// Is this messy? Yes. I like the switch format but to allow it to hit multiple cases I need to loop over it and see how many it matches 
			bool flipHorizontally = false;
			for (int i = 0; i < 4; i++)
			{
				if (GetMatchingTexture(nodePosition, i, flipHorizontally, out List<Texture2D> outputTexes))
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
				if (GetMatchingTexture(nodePosition, i, flipHorizontally, out List<Texture2D> outputTexes))
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

			if (AirportCEOTaxiwayImprovementConfig.UseTaxiwayNodeCache.Value)
			{
				foreach (List<TextureConfiguration> configurations in cachedSprites.Keys)
				{
					if (!IsListTextureContentEqual(texturesApplicable, configurations))
					{
						continue;
					}

					rotation = 0;
					return cachedSprites[configurations];
				}
			}
			stopper2.Start();
			SimpleTexture? nodeLight = null;
			bool allClear = true;

			foreach (TextureConfiguration configTexture in texturesApplicable)
			{
				if (configTexture.textureReference == TextureManager.Straight_45)
				{
					TextureConfiguration nodeLightConfig = new TextureConfiguration(TextureManager.Node_Light_45, configTexture.rotationIndex, configTexture.flipHorizontally);
					nodeLight = nodeLightConfig;
				}
				else if (configTexture.textureReference == TextureManager.Straight_90)
				{
					TextureConfiguration nodeLightConfig = new TextureConfiguration(TextureManager.Node_Light_90, configTexture.rotationIndex, configTexture.flipHorizontally);
					nodeLight = nodeLightConfig;
				}

				if (configTexture.textureReference != TextureManager.Clear)
				{
					allClear = false;
				}

				adjustedTextures.Add(configTexture); //Implicit conversion behind the scenes here
			}

			SimpleTexture nodeLightConfirmed = nodeLight ?? TextureManager.Node_Light_90;

			Sprite newSprite = null;
			if (allClear)
			{
				newSprite = TextureManager.CombineTexturesNew(adjustedTextures.ToArray());
			}
			else
			{
				newSprite = TextureManager.CombineTexturesWithNodeLight(nodeLightConfirmed, adjustedTextures.ToArray());
			}
			cachedSprites.Add(texturesApplicable, newSprite);

			stopper2.Stop();
			stopper1.Stop();
			AirportCEOTaxiwayImprovements.TILogger.LogInfo($"Time info: stopper 1 {stopper1.ElapsedMilliseconds}, and stopper 2 {stopper2.ElapsedMilliseconds}");

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

	private static bool GetMatchingTexture(Vector2 pos, int rot, bool flip, out List<Texture2D> ouputTex)
    {
		bool matched = false;
		ouputTex = new List<Texture2D>();

		if (IsMatchWith(pos, rot, flip, (-2, 0), (-1, 0), (1, 0), (2, 0)))
		{
			ouputTex.Add(TextureManager.Straight_90);
            matched = true;
		}

		if (IsMatchWith(pos, rot, flip, (-3, -3), (-2, -2), (-1, -1), (1, 1), (2, 2), (3, 3)) ||
			IsMatchWith(pos, rot, flip, (-3, -2), (-1, -2), (-2, -2), (-1, -1), (1, 1), (2, 2), (3, 3)))
		{
			ouputTex.Add(TextureManager.Straight_45);
            matched = true;
		}

		if (IsMatchWith(pos, rot, flip, (-2, 0), (-1, 0), (1, 0), (2, 1), (3, 2), (4, 3)))
		{
			ouputTex.Add(TextureManager.Curve_4590_P1);
            matched = true;
		}

		if ((IsMatchWith(pos, rot, flip, (-3, 0), (-2, 0), (-1, 0), (1, 1), (2, 2), (3, 3)) && IsNotMatchWith(pos, rot, flip, (1, 0), (0, 1), (2, 1), (1, 2), (3, 2), (2, 3))) ||
			 IsMatchWith(pos, rot, flip, (-3, 0), (-2, 0), (-1, 0), (1, 1), (2, 2), (3, 3), (4, 4)))
		{
			ouputTex.Add(TextureManager.Curve_4590_P2);
            matched = true;
		}

		if (IsMatchWith(pos, rot, flip, (-3, -1), (-2, -1), (-1, -1), (1, 1), (2, 2), (3, 3)))
		{
			ouputTex.Add(TextureManager.Curve_4590_P3);
            matched = true;
		}

		if (IsMatchWith(pos, rot, flip, (-2, 0), (-1, 0), (1, 0), (1, 1), (1, 2), (1, 3), (0, 1)))
		{
			ouputTex.Add(TextureManager.Curve_9090_P1);
            matched = true;
		}
		if (IsMatchWith(pos, rot, flip, (-2, -1), (-1, -1), (0, -1), (1, -1), (1, 0), (1, 1), (1, 2), (1, 3)) || 
			IsMatchWith(pos, rot, flip, (-2, 0), (-1, 0), (-1, 1), (0, 1), (0, 2)))
		{
			ouputTex.Add(TextureManager.Clear);
            matched = true;
		}

		return matched;
    }

    private static bool[] RotateConnections(this bool[] newConnectionsArray)
	{
		// See humoresques magic table for why this is this way
		bool[] output = new bool[TaxiwayNodeImageServer.connectionArrayLength];
		output[1] =		newConnectionsArray[0];
		output[2] =		newConnectionsArray[1];
		output[3] =		newConnectionsArray[2];
		output[0] =		newConnectionsArray[3];
		output[5] =		newConnectionsArray[4];
		output[6] =		newConnectionsArray[5];
		output[7] =		newConnectionsArray[6];
		output[4] =		newConnectionsArray[7];
		output[12] =	newConnectionsArray[8];
		output[14] =	newConnectionsArray[9];
		output[16] =	newConnectionsArray[10];
		output[18] =	newConnectionsArray[11];
		output[23] =	newConnectionsArray[12];
		output[11] =	newConnectionsArray[13];
		output[22] =	newConnectionsArray[14];
		output[10] =	newConnectionsArray[15];
		output[21] =	newConnectionsArray[16];
		output[9] =		newConnectionsArray[17];
		output[20] =	newConnectionsArray[18];
		output[8] =		newConnectionsArray[19];
		output[13] =	newConnectionsArray[20];
		output[15] =	newConnectionsArray[21];
		output[17] =	newConnectionsArray[22];
		output[19] =	newConnectionsArray[23];
		output[30] =	newConnectionsArray[24];
		output[32] =	newConnectionsArray[25];
		output[34] =	newConnectionsArray[26];
		output[36] =	newConnectionsArray[27];
		output[38] =	newConnectionsArray[28];
		output[40] =	newConnectionsArray[29];
		output[47] =	newConnectionsArray[30];
		output[29] =	newConnectionsArray[31];
		output[46] =	newConnectionsArray[32];
		output[28] =	newConnectionsArray[33];
		output[45] =	newConnectionsArray[34];
		output[27] =	newConnectionsArray[35];
		output[44] =	newConnectionsArray[36];
		output[26] =	newConnectionsArray[37];
		output[43] =	newConnectionsArray[38];
		output[25] =	newConnectionsArray[39];
		output[42] =	newConnectionsArray[40];
		output[24] =	newConnectionsArray[41];
		output[31] =	newConnectionsArray[42];
		output[33] =	newConnectionsArray[43];
		output[35] =	newConnectionsArray[44];
		output[37] =	newConnectionsArray[45];
		output[39] =	newConnectionsArray[46];
		output[41] =	newConnectionsArray[47];
		
		output[51] =	newConnectionsArray[48];
		output[53] =	newConnectionsArray[49];
		output[49] =	newConnectionsArray[50];
		output[55] =	newConnectionsArray[51];
		output[48] =	newConnectionsArray[52];
		output[54] =	newConnectionsArray[53];
		output[50] =	newConnectionsArray[54];
		output[52] =	newConnectionsArray[55];

		for (int i = 0; i < output.Length; i++)
		{
			newConnectionsArray[i] = output[i];
        }

		return output;
	}
	
	private static bool[] FlipConnections(this bool[] newConnectionsArray)
	{
		// See humoresques magic table for why this is this way
		bool[] output = new bool[TaxiwayNodeImageServer.connectionArrayLength];
		output[0] =		newConnectionsArray[0];
		output[3] =		newConnectionsArray[1];
		output[2] =		newConnectionsArray[2];
		output[1] =		newConnectionsArray[3];
		output[7] =		newConnectionsArray[4];
		output[6] =		newConnectionsArray[5];
		output[5] =		newConnectionsArray[6];
		output[4] =		newConnectionsArray[7];
		output[19] =	newConnectionsArray[8];
		output[20] =	newConnectionsArray[9];
		output[21] =	newConnectionsArray[10];
		output[22] =	newConnectionsArray[11];
		output[23] =	newConnectionsArray[12];
		output[17] =	newConnectionsArray[13];
		output[18] =	newConnectionsArray[14];
		output[15] =	newConnectionsArray[15];
		output[16] =	newConnectionsArray[16];
		output[13] =	newConnectionsArray[17];
		output[14] =	newConnectionsArray[18];
		output[8] =		newConnectionsArray[19];
		output[9] =		newConnectionsArray[20];
		output[10] =	newConnectionsArray[21];
		output[11] =	newConnectionsArray[22];
		output[12] =	newConnectionsArray[23];
		output[41] =	newConnectionsArray[24];
		output[42] =	newConnectionsArray[25];
		output[43] =	newConnectionsArray[26];
		output[44] =	newConnectionsArray[27];
		output[45] =	newConnectionsArray[28];
		output[46] =	newConnectionsArray[29];
		output[47] =	newConnectionsArray[30];
		output[39] =	newConnectionsArray[31];
		output[40] =	newConnectionsArray[32];
		output[37] =	newConnectionsArray[33];
		output[38] =	newConnectionsArray[34];
		output[35] =	newConnectionsArray[35];
		output[36] =	newConnectionsArray[36];
		output[33] =	newConnectionsArray[37];
		output[34] =	newConnectionsArray[38];
		output[31] =	newConnectionsArray[39];
		output[32] =	newConnectionsArray[40];
		output[24] =	newConnectionsArray[41];
		output[25] =	newConnectionsArray[42];
		output[26] =	newConnectionsArray[43];
		output[27] =	newConnectionsArray[44];
		output[28] =	newConnectionsArray[45];
		output[29] =	newConnectionsArray[46];
		output[30] =	newConnectionsArray[47];

		output[54] =	newConnectionsArray[48];
		output[55] =	newConnectionsArray[49];
		output[52] =	newConnectionsArray[50];
		output[53] =	newConnectionsArray[51];
		output[50] =	newConnectionsArray[52];
		output[51] =	newConnectionsArray[53];
		output[48] =	newConnectionsArray[54];
		output[49] =	newConnectionsArray[55];

        for (int i = 0; i < output.Length; i++)
		{
			newConnectionsArray[i] = output[i];
        }

		return output;
	}

	internal static void SetSpecials()
	{

	}

	private static bool IsMatchWith(Vector2 nodePosition, int rotation, bool isFlipped, params (int, int)[] translationsToCheck)
	{
		Vector2[] newTranslations = UpdateTranslationsToInGameUnits(translationsToCheck, rotation, isFlipped);

		foreach (Vector2 translation in newTranslations)
		{
			if (Singleton<TaxiwayController>.Instance.GetNodeAtPosition(nodePosition + translation) == null)
			{
				return false;
			}
		}

		return true;
    }
	private static bool IsNotMatchWith(Vector2 nodePosition, int rotation, bool isFlipped, params (int, int)[] translationsToCheck)
	{
		Vector2[] newTranslations = UpdateTranslationsToInGameUnits(translationsToCheck, rotation, isFlipped);

		foreach (Vector2 translation in newTranslations)
		{
			if (Singleton<TaxiwayController>.Instance.GetNodeAtPosition(nodePosition + translation) != null)
			{
				return false;
			}
		}

		return true;
    }

	private static Vector2[] UpdateTranslationsToInGameUnits((int, int)[] translations, int rotation, bool isFlipped)
	{
		Vector2[] output = new Vector2[translations.Length];

        for (int i = 0; i < translations.Length; i++)
        {
			output[i] = new Vector2(translations[i].Item1, translations[i].Item2);

			output[i] *= 4;

			if (isFlipped)
			{
				output[i] = new Vector2(-output[i].x, output[i].y);

				if (rotation == 1)
				{
					output[i] = new Vector2(output[i].y, -output[i].x);
				}
				else if (rotation == 3)
				{
					output[i] = new Vector2(-output[i].y, output[i].x);
				}
			}
			else
			{
				if (rotation == 3)
				{
					output[i] = new Vector2(output[i].y, -output[i].x);
				}
				else if (rotation == 1)
				{
					output[i] = new Vector2(-output[i].y, output[i].x);
				}
			}


			if (rotation == 2)
			{
				output[i] = new Vector2(-output[i].x, -output[i].y);
			}
        }

		return output;
	}

	private static bool IsListTextureContentEqual(List<TextureConfiguration> list1, List<TextureConfiguration> list2)
	{
		if (list1.Count != list2.Count)
		{
			return false;
		}

        for (int i = 0; i < list1.Count; i++)
        {
			if (!TextureConfigsEqual(list1[i], list2[i]))
			{
				return false;
			}
        }

		return true;
    }

    private static bool TextureConfigsEqual(TextureConfiguration textureConfiguration1, TextureConfiguration textureConfiguration2)
    {
        if (textureConfiguration1.textureReference != textureConfiguration2.textureReference)
        {
            return false;
        }

        if (textureConfiguration1.rotationIndex != textureConfiguration2.rotationIndex)
        {
            return false;
        }

        if (textureConfiguration1.flipHorizontally != textureConfiguration2.flipHorizontally)
        {
            return false;
        }

        return true;
    }
}