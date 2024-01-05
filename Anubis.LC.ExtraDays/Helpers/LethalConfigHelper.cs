using BepInEx.Configuration;
using LethalConfig.ConfigItems;
using System.IO;
using System.Reflection;
using UnityEngine;
using LethalConfig;
using Anubis.LC.ExtraDays.Models;

namespace Anubis.LC.ExtraDays.Helpers
{
    public static class LethalConfigHelper
    {
        public static void SetLehalConfig(ConfigFile config)
        {
            ModSettings settings = ModSettingsHelper.ReadSettings();

            var configEntry = config.Bind("General", "Use price correlated calculation?", settings.IsCorrelatedCalculation, "This determines if the price to buy an extra day will be constant value or correlated to the quota");
            configEntry.SettingChanged += (obj, args) =>
            {
                ModSettingsHelper.WriteSettings(configEntry.Value);
            };
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(configEntry, false));

            LethalConfigManager.SetModIcon(LoadNewSprite(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "icon.png")));
            LethalConfigManager.SetModDescription("Allows the player to purchase an extra day via the terminal. This mod's uniqueness is that it tries to be more realistic with the game, as it is not trying to modify the main logic of the deadline. It tries to add functionality that will give you extends for the deadline but with some price on it.");
        }

        private static Sprite LoadNewSprite(string filePath, float pixelsPerUnit = 100.0f)
        {
            try
            {
                Sprite NewSprite = new Sprite();
                Texture2D SpriteTexture = LoadTexture(filePath);
                NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), pixelsPerUnit);

                return NewSprite;
            }
            catch
            {
                return null;
            }
        }

        private static Texture2D LoadTexture(string filePath)
        {
            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(filePath))
            {
                FileData = File.ReadAllBytes(filePath);
                Tex2D = new Texture2D(2, 2);
                if (Tex2D.LoadImage(FileData))
                {
                    return Tex2D;
                }
            }
            return null;
        }
    }
}
