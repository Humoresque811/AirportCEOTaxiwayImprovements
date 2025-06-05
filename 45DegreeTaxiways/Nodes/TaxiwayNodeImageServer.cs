using AirportCEOModLoader.Core;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

internal static class TaxiwayNodeImageServer
{
	private static readonly Dictionary<NodeConfiguration, Sprite> cachedSprites = new();
	private static readonly List<Sprite> notSpecials = new();

	internal static readonly int connectionArrayLength = 48;

    internal static Sprite GetPotentialSprite(bool[] newConnectionsArray, out int rotation, out bool isSpecial)
	{
		try
		{
			NodeConfiguration nodeConfiguration = new NodeConfiguration(newConnectionsArray);
			if (AirportCEOTaxiwayImprovementConfig.UseTaxiwayNodeCache.Value)
			{
				for (int i = 0; i < 360; i += 90)
				{
					foreach (NodeConfiguration key in cachedSprites.Keys)
					{
						if (key.Equals(nodeConfiguration))
						{
							rotation = i;
							isSpecial = notSpecials.Contains(cachedSprites[key]) ? false : true;
							return cachedSprites[key];
						}
					}
					nodeConfiguration = new NodeConfiguration(newConnectionsArray.RotateConnections());
				}
			}

			// We know this configuration does not exist anywhere!
			List<TextureConfiguration> texturesApplicable = new();
			List<SimpleTexture> adjustedTextures = new();

			// Is this messy? Yes. I like the switch format but to allow it to hit multiple cases I need to loop over it and see how many it matches 
			bool flipHorizontally = false;
			for (int i = 0; i < 4; i++)
			{
				if (GetMatchingTexture(newConnectionsArray, out List<Texture2D> outputTexes))
				{
					foreach (Texture2D tex in outputTexes)
					{
						texturesApplicable.Add(new TextureConfiguration(tex, i, flipHorizontally));
					}
				}
				newConnectionsArray.RotateConnections();
			}

			flipHorizontally = true;
			//newConnectionsArray.RotateConnections();    // Back to default
			newConnectionsArray.FlipConnections();      // Flip them so we can catch all possible combinations

			for (int i = 0; i < 4; i++)
			{
				if (GetMatchingTexture(newConnectionsArray, out List<Texture2D> outputTexes))
				{
					foreach (Texture2D tex in outputTexes)
					{
						texturesApplicable.Add(new TextureConfiguration(tex, i, flipHorizontally));
					}
				}
				newConnectionsArray.RotateConnections();
			}

			//newConnectionsArray.RotateConnections();
			newConnectionsArray.FlipConnections();

			if (texturesApplicable.Count == 0)
			{
				// Nothing matches this layout at all
				rotation = 0;
				isSpecial = false;
				return null;
			}

			foreach (TextureConfiguration configTexture in texturesApplicable)
			{
				adjustedTextures.Add(configTexture); //Implicit conversion behind the scenes here
			}

			Sprite newSprite = TextureManager.CombineTexturesNew(adjustedTextures.ToArray());
			cachedSprites.Add(nodeConfiguration, newSprite);

			if (texturesApplicable.Count == 1 && (texturesApplicable[0].textureReference == TextureManager.Straight_90 
				|| texturesApplicable[0].textureReference == TextureManager.Straight_45) && !notSpecials.Contains(newSprite))
			{
				notSpecials.Add(newSprite);
			}

			rotation = 0;
			isSpecial = notSpecials.Contains(newSprite) ? false : true;
			return newSprite;
		}
		catch (Exception ex)
		{
			AirportCEOTaxiwayImprovements.TILogger.LogError($"Taxiway Node Sprite generation code failed. {ExceptionUtils.ProccessException(ex)}");
			rotation = 0;
			isSpecial = false;
			return null;
		}
	}

	private static bool GetMatchingTexture(bool[] newConnectionsArray, out List<Texture2D> ouputTex)
    {
		var infoSet = (newConnectionsArray[0], newConnectionsArray[1], newConnectionsArray[2], newConnectionsArray[3],
		newConnectionsArray[4], newConnectionsArray[5], newConnectionsArray[6], newConnectionsArray[7],
		newConnectionsArray[8], newConnectionsArray[9], newConnectionsArray[10], newConnectionsArray[11], newConnectionsArray[12],
		newConnectionsArray[13], newConnectionsArray[14], newConnectionsArray[15], newConnectionsArray[16], newConnectionsArray[17],
		newConnectionsArray[18], newConnectionsArray[19], newConnectionsArray[20], newConnectionsArray[21], newConnectionsArray[22],
		newConnectionsArray[23], newConnectionsArray[24], newConnectionsArray[25], newConnectionsArray[26], newConnectionsArray[27],
		newConnectionsArray[28], newConnectionsArray[29], newConnectionsArray[30], newConnectionsArray[31], newConnectionsArray[32],
		newConnectionsArray[33], newConnectionsArray[34], newConnectionsArray[35], newConnectionsArray[36], newConnectionsArray[37],
		newConnectionsArray[38], newConnectionsArray[39], newConnectionsArray[40], newConnectionsArray[41], newConnectionsArray[42],
		newConnectionsArray[43], newConnectionsArray[44], newConnectionsArray[45], newConnectionsArray[46], newConnectionsArray[47]);

		bool matched = false;
		ouputTex = new List<Texture2D>();

		switch (infoSet) {
			case (_, true, _, true, _, _, _, _, _, _, true, _, _, _, _, _, _, _, _, _, _, true, _, _, _, _, _, true, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, true, _, _, _):
				ouputTex.Add(TextureManager.Straight_90);
                matched = true;
				break;
		}
		switch (infoSet) {
			case (_, _, _, _, true, _, true, _, true, _, _, _, _, _, _, _, _, _, _, _, _, _, _, true, true, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, true):
				ouputTex.Add(TextureManager.Straight_45);
                matched = true;
				break;
		}
		switch (infoSet) {
			case (_, true, _, true, _, _, _, _, _, _, true, _, _, _, _, _, _, _, _, _, _, true, _, false, _, _, _, true, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, true, false, _):
				ouputTex.Add(TextureManager.Curve_4590_P1);
                matched = true;
				break;
		}
		switch (infoSet) {
			case (_, true, _, true, _, _, _, _, _, _, true, _, _, _, _, _, _, _, false, _, _, _, true, false, _, _, _, true, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, true, _):
				ouputTex.Add(TextureManager.Curve_4590_P2);
                matched = true;
				break;
		}
		switch (infoSet) {
			case (_, _, _, true, true, _, _, _, _, _, true, _, _, _, _, _, false, _, false, _, _, _, _, true, _, _, _, true, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, true):
				ouputTex.Add(TextureManager.Curve_4590_P3);
                matched = true;
				break;
		}	
		switch (infoSet) {
			case (false, _, _, _, true, _, true, false, _, true, _, _, _, _, _, _, _, _, _, _, _, _, _, true, _, _, true, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, true):
				ouputTex.Add(TextureManager.Curve_4590_P4);
                matched = true;
				break;
		}
		switch (infoSet) {
			case (_, _, _, false, true, _, true, _, true, _, false, _, _, _, _, _, _, _, _, _, _, _, _, true, _, true, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, true): // hits when flipped and i=1
				ouputTex.Add(TextureManager.Curve_4590_P5);
                matched = true;
				break;
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

        for (int i = 0; i < output.Length; i++)
		{
			newConnectionsArray[i] = output[i];
        }

		return output;
	}

	internal static void SetSpecials()
	{
		//cachedSprites.Clear();
	}

}