using BepInEx;
using HarmonyLib;
using LethalAPI.LibTerminal.Models;
using Anubis.LC.ExtraDays.Commands;
using Anubis.LC.ExtraDays.Helpers;
using LethalAPI.LibTerminal;

namespace Anubis.LC.ExtraDays
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class ExtraDaysToDeadlinePlugin : BaseUnityPlugin
	{
		private Harmony m_Harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        private TerminalModRegistry m_Registry;

        private void Awake()
		{
			ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is loading...");

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Installing patches");
            m_Harmony.PatchAll(typeof(ExtraDaysToDeadlinePlugin).Assembly);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Registering built-in Commands");

            // Create registry for the Terminals API
            m_Registry = TerminalRegistry.CreateTerminalRegistry();

            // Register commands, don't care about the instance
            m_Registry.RegisterFrom<BuyExtraDaysCommands>();

			DontDestroyOnLoad(this);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
