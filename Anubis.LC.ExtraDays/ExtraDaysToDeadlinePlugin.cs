using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Anubis.LC.ExtraDays.Commands;
using Anubis.LC.ExtraDays.Models;
using Anubis.LC.ExtraDays.Patches;

namespace Anubis.LC.ExtraDays
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class ExtraDaysToDeadlinePlugin : BaseUnityPlugin
	{
		private Harmony HarmonyInstance = new Harmony(PluginInfo.PLUGIN_GUID);

		private TerminalModRegistry Terminal;

		public static ManualLogSource LogSource = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_GUID);

        public static bool IsInProcess = false;

        private void Awake()
		{
			Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is loading...");

			Logger.LogInfo($"Installing patches");
            HarmonyInstance.PatchAll(typeof(ExtraDaysToDeadlinePlugin).Assembly);
            HarmonyInstance.PatchAll(typeof(TimeOfDayPatch).Assembly);

            Logger.LogInfo($"Registering built-in Commands");

			// Create registry for the Terminals API
			Terminal = TerminalRegistry.CreateTerminalRegistry();

			// Register commands, don't care about the instance
			Terminal.RegisterFrom<CommandInfoCommands>();

			DontDestroyOnLoad(this);

			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
