using BepInEx.Configuration;
using LethalConfig.ConfigItems;
using System.IO;
using System.Reflection;
using UnityEngine;
using LethalConfig;

namespace Anubis.LC.ExtraDays.Helpers
{
    public static class LethalConfigHelper
    {
        public static ConfigEntry<bool> IsCorrelatedPriceCalculationForSaveFile1;
        public static ConfigEntry<bool> IsCorrelatedPriceCalculationForSaveFile2;
        public static ConfigEntry<bool> IsCorrelatedPriceCalculationForSaveFile3;

        public static void SetLehalConfig(ConfigFile config)
        {
            IsCorrelatedPriceCalculationForSaveFile1 = config.Bind("Save File 1", "Use price correlated calculation?", true, "This determines if the price to buy an extra day will be constant value (350 credits) or correlated to the quota (dynamic)");
            IsCorrelatedPriceCalculationForSaveFile2 = config.Bind("Save File 2", "Use price correlated calculation?", true, "This determines if the price to buy an extra day will be constant value (350 credits) or correlated to the quota (dynamic)");
            IsCorrelatedPriceCalculationForSaveFile3 = config.Bind("Save File 3", "Use price correlated calculation?", true, "This determines if the price to buy an extra day will be constant value (350 credits) or correlated to the quota (dynamic)");

            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(IsCorrelatedPriceCalculationForSaveFile1, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(IsCorrelatedPriceCalculationForSaveFile2, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(IsCorrelatedPriceCalculationForSaveFile3, false));

            LethalConfigManager.SetModIcon(LoadNewSprite(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "icon.png")));
            LethalConfigManager.SetModDescription("Allows the player to purchase an extra day via the terminal. This mod's uniqueness is that it tries to be more realistic with the game, as it is not trying to modify the main logic of the deadline. It tries to add functionality that will give you extends for the deadline but with some price on it.");
        }

        public static ConfigEntry<bool> GetConfigForSaveFile()
        {
            switch(GameNetworkManager.Instance.currentSaveFileName)
            {
                default:
                case "LCSaveFile1":
                    return IsCorrelatedPriceCalculationForSaveFile1;
                case "LCSaveFile2":
                    return IsCorrelatedPriceCalculationForSaveFile2;
                case "LCSaveFile3":
                    return IsCorrelatedPriceCalculationForSaveFile3;
            }
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
