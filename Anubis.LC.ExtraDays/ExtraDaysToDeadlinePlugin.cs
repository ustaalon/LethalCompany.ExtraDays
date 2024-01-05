using BepInEx;
using HarmonyLib;
using LethalAPI.TerminalCommands.Models;
using Anubis.LC.ExtraDays.Commands;
using Anubis.LC.ExtraDays.Helpers;

namespace Anubis.LC.ExtraDays
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("ainavt.lc.lethalconfig")]
    public class ExtraDaysToDeadlinePlugin : BaseUnityPlugin
    {
        private const string modGUID = ExtraDaysToDeadlineStaticHelper.modGUID;
        private const string modName = ExtraDaysToDeadlineStaticHelper.modName;
        private const string modVersion = ExtraDaysToDeadlineStaticHelper.modVersion;

        private Harmony HarmonyInstance = new Harmony(modGUID);

        private TerminalModRegistry Terminal;

        private void Awake()
        {
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"{modGUID} is loading...");

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Installing patches");
            HarmonyInstance.PatchAll(typeof(ExtraDaysToDeadlinePlugin).Assembly);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Registering built-in Commands");

            // Create registry for the Terminals API
            Terminal = TerminalRegistry.CreateTerminalRegistry();

            // Register commands, don't care about the instance
            Terminal.RegisterFrom<BuyExtraDaysCommands>();

            LethalConfigHelper.SetLehalConfig(Config);

            DontDestroyOnLoad(this);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Plugin {modGUID} is loaded!");
        }
    }
}
