using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalAPI.LibTerminal.Models;
using Anubis.LC.ExtraDays.Commands;
using Anubis.LC.ExtraDays.Helpers;

namespace Anubis.LC.ExtraDays
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class ExtraDaysToDeadlinePlugin : BaseUnityPlugin
	{
		private Harmony HarmonyInstance = new Harmony(PluginInfo.PLUGIN_GUID);

		private TerminalModRegistry Terminal;

        private void Awake()
		{
			ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is loading...");

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Installing patches");
            HarmonyInstance.PatchAll(typeof(ExtraDaysToDeadlinePlugin).Assembly);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Registering built-in Commands");

			// Create registry for the Terminals API
			Terminal = TerminalRegistry.CreateTerminalRegistry();

			// Register commands, don't care about the instance
			Terminal.RegisterFrom<BuyExtraDaysCommands>();

			DontDestroyOnLoad(this);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
