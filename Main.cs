using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;
using XLShredLib;
using System;

namespace NoFlip
{
    [Serializable]
    public class Settings : UnityModManager.ModSettings
    {

        public float timeoutDelay = 2f;
        public bool timeoutEnable = true;
        public bool disableAfterPop = true;
        public bool disableAfterRespawn = true;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }
    }

    static class Main
    {
        public static Settings settings;
        public static bool enabled;
        public static bool suppressionActive;
        public static bool buttonsPressed;
        public static float debounce;
        public static float delayTimer;

        private static Harmony harmonyInstance;
        
        static bool Load(ModEntry modEntry)
        {
            settings = Settings.Load<Settings>(modEntry);
            harmonyInstance = new Harmony(modEntry.Info.Id);
            
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            modEntry.OnSaveGUI = OnSaveGUI;
            return true;
        }

        static void OnSaveGUI(ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        static void OnUpdate(ModEntry modEntry, float dt)
        {
            if (settings.timeoutEnable)
            {
                if (suppressionActive)
                {
                    delayTimer += dt;
                    if (delayTimer >= settings.timeoutDelay)
                    {
                        suppressionActive = false;
                        delayTimer = 0f;
                        ShowNoFlipMessage("Timeout!");
                    }
                }
            }
            if ((PlayerController.Instance.inputController.player.GetButtonDown("LB") && PlayerController.Instance.inputController.player.GetButton("RB")) || (PlayerController.Instance.inputController.player.GetButton("LB") && PlayerController.Instance.inputController.player.GetButtonDown("RB")))
            {
                if (buttonsPressed && debounce >= 0.3f)
                {
                    suppressionActive = !suppressionActive;
                    ShowNoFlipMessage(suppressionActive ? "Enabled!" : "Disabled!");
                    debounce = 0f;
                }
                else if(buttonsPressed)
                {
                    debounce += dt;
                }
                else
                {
                    suppressionActive = !suppressionActive;
                    ShowNoFlipMessage(suppressionActive ? "Enabled!" : "Disabled!");
                    buttonsPressed = true;
                }
            }
            else if (buttonsPressed)
            {
                buttonsPressed = false;
                debounce = 0f;
            }

        }

        static bool OnToggle(ModEntry modEntry, bool value)
        {
            if (enabled == value) return true;

            enabled = value;

            if (enabled)
            {
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                ModMenu.Instance.gameObject.AddComponent<NoFlipMenu>();
            }
            else
            {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
            }
            return true;
        }

        public static void ShowNoFlipMessage(string message)
        {
            ModMenu.Instance.ShowMessage("NoFlip: " + message);
        }
    }
}
