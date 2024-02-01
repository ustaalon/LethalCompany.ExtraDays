using BepInEx.Configuration;
using LethalConfig.ConfigItems;
using System.IO;
using System.Reflection;
using UnityEngine;
using LethalConfig;
using System.Collections.Generic;
using System;
using LethalConfig.ConfigItems.Options;
using Anubis.LC.ExtraDays.ModNetwork;

namespace Anubis.LC.ExtraDays.Helpers
{
    public static class LethalConfigHelper
    {
        public static Dictionary<string, Dictionary<string, ConfigEntryBase>> SaveFilesConfigurations;

        public static void SetLehalConfig(ConfigFile config)
        {
            SaveFilesConfigurations = new Dictionary<string, Dictionary<string, ConfigEntryBase>>();

            int numOfSaveFiles = 0;
            foreach (string file in ES3.GetFiles())
            {
                if (ES3.FileExists(file) && file.StartsWith("LCSaveFile"))
                {
                    numOfSaveFiles++;
                }
            }

            ModStaticHelper.Logger.LogInfo($"There are {numOfSaveFiles} save files");
            if (numOfSaveFiles > 3)
            {
                int i = 1;
                foreach (string file in ES3.GetFiles())
                {
                    if (ES3.FileExists(file) && file.StartsWith("LCSaveFile"))
                    {
                        var currentIndex = i++;
                        var configurationForSaveFile = new Dictionary<string, ConfigEntryBase>();
                        var correlatedPrice = config.Bind($"Save File {currentIndex}", "Use price correlated calculation?", true, "This determines if the price to buy an extra day will be constant value (350 credits) or correlated to the quota (dynamic)");
                        var buyingRate = config.Bind($"Save File {currentIndex}", "Use reduce buying rate?", false, "This determines if the buying rate will reduce a bit after buying an extra day");
                        var extraDayPrice = config.Bind($"Save File {currentIndex}", "Setup extra day price", ModStaticHelper.CONSTANT_PRICE, "This configure the price of an extra day only if `correlated calculation` field is OFF");
                        extraDayPrice.SettingChanged += (obj, args) =>
                        {
                            if (Networking.Instance && RoundManager.Instance.NetworkManager.IsHost)
                            {
                                Networking.Instance.SetExtraDaysPriceServerRpc(extraDayPrice.Value);
                            }
                        };
                        LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(correlatedPrice, false));
                        LethalConfigManager.AddConfigItem(new IntSliderConfigItem(extraDayPrice, new IntSliderOptions()
                        {
                            Min = 250,
                            Max = 1700,
                            RequiresRestart = false,
                        }));
                        LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(buyingRate, false));
                        configurationForSaveFile.Add("correlatedPrice", correlatedPrice);
                        configurationForSaveFile.Add("buyingRate", buyingRate);
                        configurationForSaveFile.Add("extraDayPrice", extraDayPrice);
                        SaveFilesConfigurations.Add(file, configurationForSaveFile);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    var currentIndex = i + 1;
                    var configurationForSaveFile = new Dictionary<string, ConfigEntryBase>();
                    var correlatedPrice = config.Bind($"Save File {currentIndex}", "Use price correlated calculation?", true, "This determines if the price to buy an extra day will be constant value (350 credits) or correlated to the quota (dynamic)");
                    var buyingRate = config.Bind($"Save File {currentIndex}", "Use reduce buying rate?", false, "This determines if the buying rate will reduce a bit after buying an extra day");
                    var extraDayPrice = config.Bind($"Save File {currentIndex}", "Setup extra day price", ModStaticHelper.CONSTANT_PRICE, "This configure the price of an extra day only if `correlated calculation` field is OFF");
                    extraDayPrice.SettingChanged += (obj, args) =>
                    {
                        if (Networking.Instance && RoundManager.Instance.NetworkManager.IsHost)
                        {
                            Networking.Instance.SetExtraDaysPriceServerRpc(extraDayPrice.Value);
                        }
                    };
                    LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(correlatedPrice, false));
                    LethalConfigManager.AddConfigItem(new IntSliderConfigItem(extraDayPrice, new IntSliderOptions()
                    {
                        Min = 250,
                        Max = 1700,
                        RequiresRestart = false,
                    }));
                    LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(buyingRate, false));
                    configurationForSaveFile.Add("correlatedPrice", correlatedPrice);
                    configurationForSaveFile.Add("buyingRate", buyingRate);
                    configurationForSaveFile.Add("extraDayPrice", extraDayPrice);
                    SaveFilesConfigurations.Add($"LCSaveFile{i}", configurationForSaveFile);
                }
            }

            LethalConfigManager.SetModIcon(LoadNewSprite(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "icon.png")));
            LethalConfigManager.SetModDescription("Allows the player to purchase an extra day via the terminal. This mod's uniqueness is that it tries to be more realistic with the game, as it is not trying to modify the main logic of the deadline. It tries to add functionality that will give you extends for the deadline but with some price on it.");
        }

        public static Dictionary<string, ConfigEntryBase> GetConfigForSaveFile()
        {
            SaveFilesConfigurations.TryGetValue(GameNetworkManager.Instance.currentSaveFileName, out var config);
            if (config == null)
            {
                ModStaticHelper.Logger.LogError("Could not find save file. Using defaults configuration for save files");
                var configurationForSaveFile = new Dictionary<string, ConfigEntryBase>();
                var correlatedPrice = ExtraDaysToDeadlinePlugin.Instance.Config.Bind($"Save File 1", "Use price correlated calculation?", true, "This determines if the price to buy an extra day will be constant value (350 credits) or correlated to the quota (dynamic)");
                var buyingRate = ExtraDaysToDeadlinePlugin.Instance.Config.Bind($"Save File 1", "Use reduce buying rate?", false, "This determines if the buying rate will reduce a bit after buying an extra day");
                var extraDayPrice = ExtraDaysToDeadlinePlugin.Instance.Config.Bind($"Save File 1", "Setup extra day price", ModStaticHelper.CONSTANT_PRICE, "This configure the price of an extra day only if `correlated calculation` field is OFF");
                LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(correlatedPrice, false));
                LethalConfigManager.AddConfigItem(new IntSliderConfigItem(extraDayPrice, new IntSliderOptions()
                {
                    Min = 250,
                    Max = 1700,
                    RequiresRestart = false,
                }));
                LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(buyingRate, false));
                configurationForSaveFile.Add("correlatedPrice", correlatedPrice);
                configurationForSaveFile.Add("buyingRate", buyingRate);
                configurationForSaveFile.Add("extraDayPrice", extraDayPrice);
                return configurationForSaveFile;
            }
            return config;
        }

        private static Sprite LoadNewSprite(string filePath, float pixelsPerUnit = 100.0f)
        {
            try
            {
                Texture2D spriteTexture = LoadTexture(filePath);
                var newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), pixelsPerUnit);

                return newSprite;
            }
            catch
            {
                return null;
            }
        }

        private static Texture2D LoadTexture(string filePath)
        {
            Texture2D tex2D;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex2D = new Texture2D(2, 2);
                if (tex2D.LoadImage(fileData))
                {
                    return tex2D;
                }
            }
            return null;
        }
    }
}
