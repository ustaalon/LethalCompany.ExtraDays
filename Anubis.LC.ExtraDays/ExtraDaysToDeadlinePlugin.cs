using BepInEx;
using HarmonyLib;
using LethalAPI.LibTerminal.Models;
using Anubis.LC.ExtraDays.Commands;
using Anubis.LC.ExtraDays.Helpers;
using LethalAPI.LibTerminal;
using RuntimeNetcodeRPCValidator;
using Anubis.LC.ExtraDays.ModNetwork;

namespace Anubis.LC.ExtraDays
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.SoftDependency)]
    public class ExtraDaysToDeadlinePlugin : BaseUnityPlugin
    {
        private const string modGUID = ModStaticHelper.modGUID;
        private const string modName = ModStaticHelper.modName;
        private const string modVersion = ModStaticHelper.modVersion;

        private Harmony m_Harmony = new Harmony(modGUID);

        private TerminalModRegistry m_Registry;

        public static ExtraDaysToDeadlinePlugin Instance;

        private NetcodeValidator netcodeValidator;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            netcodeValidator = new NetcodeValidator(ModStaticHelper.modGUID);
            netcodeValidator.PatchAll();

            netcodeValidator.BindToPreExistingObjectByBehaviour<Networking, Terminal>();

            ModStaticHelper.Logger.LogInfo($"{modGUID} is loading...");

            ModStaticHelper.Logger.LogInfo($"Installing patches");
            m_Harmony.PatchAll(typeof(ExtraDaysToDeadlinePlugin).Assembly);

            ModStaticHelper.Logger.LogInfo($"Registering built-in Commands");

            // Create registry for the Terminals API
            m_Registry = TerminalRegistry.CreateTerminalRegistry();

            // Register commands, don't care about the instance
            m_Registry.RegisterFrom<BuyExtraDaysCommands>();

            LethalConfigHelper.SetLehalConfig(Config);

            DontDestroyOnLoad(this);

            ModStaticHelper.Logger.LogInfo($"Plugin {modGUID} is loaded!");
        }
    }
}
