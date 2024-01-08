using BepInEx;
using HarmonyLib;
using LethalAPI.LibTerminal.Models;
using Anubis.LC.ExtraDays.Commands;
using Anubis.LC.ExtraDays.Helpers;
using LethalAPI.LibTerminal;

namespace Anubis.LC.ExtraDays
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("ainavt.lc.lethalconfig")]
    public class ExtraDaysToDeadlinePlugin : BaseUnityPlugin
    {
        private const string modGUID = ExtraDaysToDeadlineStaticHelper.modGUID;
        private const string modName = ExtraDaysToDeadlineStaticHelper.modName;
        private const string modVersion = ExtraDaysToDeadlineStaticHelper.modVersion;

        private Harmony m_Harmony = new Harmony(modGUID);

        private TerminalModRegistry m_Registry;

        private void Awake()
        {
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"{modGUID} is loading...");

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Installing patches");
            m_Harmony.PatchAll(typeof(ExtraDaysToDeadlinePlugin).Assembly);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Registering built-in Commands");

            // Create registry for the Terminals API
            m_Registry = TerminalRegistry.CreateTerminalRegistry();

            // Register commands, don't care about the instance
            m_Registry.RegisterFrom<BuyExtraDaysCommands>();

            LethalConfigHelper.SetLehalConfig(Config);

            DontDestroyOnLoad(this);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Plugin {modGUID} is loaded!");
        }
    }
}
