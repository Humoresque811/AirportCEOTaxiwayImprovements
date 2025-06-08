using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportCEOTaxiwayImprovements._45DegreeTaxiways;

internal class UIEdits
{
	internal static void ShowNodeWarningMessage(SaveLoadGameDataController _)
	{
		if (AirportCEOTaxiwayImprovementConfig.SmoothTaxiwayNodes.Value && AirportCEOTaxiwayImprovementConfig.ShowTaxiwayNodeWarning.Value)
		{
			AirportCEOModLoader.Core.DialogUtils.QueueDialog("You have enabled taxiway node smoothing from the mod AirportCEO Taxiway Improvements - This is just a " +
				"brief note that this setting may cause your game to hang while building taxiways and taxiway nodes, and getting taxiway nodes to look perfect might " +
				"take a bit of trial an error. This is normal, and just a side effect of how complicated such a system is. If you would like to disable the setting, " +
				"or stop seeing this message go to config (F1) and disable the respective settings, then reload the game.");
		}
	}
	internal static void ShowReloadWarningMessage(object _, System.EventArgs __)
	{
		AirportCEOModLoader.Core.DialogUtils.QueueDialog("You have just changed a config value that requires a game reload. Please reload the game completely " +
			"(quit to desktop) to avoid bugs (but remember to save your game first if you are in game)");
	}
}
