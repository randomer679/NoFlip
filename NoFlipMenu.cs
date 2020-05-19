using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

namespace NoFlip
{
    class NoFlipMenu : MonoBehaviour
    {
        private ModUIBox modMenuBox;

        public void Start()
        {
            modMenuBox = ModMenu.Instance.RegisterModMaker("randomer679", "randomer679");
            modMenuBox.AddToggle("noflip.disableafterpop", "Disable after pop: ", Side.left, () => Main.enabled, Main.settings.disableAfterPop && Main.enabled, (b) => Main.settings.disableAfterPop = b);
            modMenuBox.AddToggle("noflip.timeoutenable", "Activate timeout: ", Side.left, () => Main.enabled, Main.settings.timeoutEnable && Main.enabled, (b) => Main.settings.timeoutEnable = b);
            modMenuBox.AddToggle("noflip.disableafterrespawn", "Disable after respawn: ", Side.right, () => Main.enabled, Main.settings.disableAfterRespawn && Main.enabled, (b) => Main.settings.disableAfterRespawn = b);
            modMenuBox.AddCustom("noflip.timeoutdelay", () => { DrawInput(); }, () => Main.enabled);
        }
        
        public void DrawInput()
        {
            if (Main.settings.timeoutEnable)
            {
                GUI.Label(new Rect(0, 0, 0, 0), "Timeout: ");
                var value = GUILayout.TextField(Main.settings.timeoutDelay.ToString("0.00"));
                if (value != Main.settings.timeoutDelay.ToString("0.00"))
                {
                    if (float.TryParse(value, out float parsedFloat))
                    {
                        if (parsedFloat > 0.5f)
                        {
                            Main.settings.timeoutDelay = parsedFloat;
                        }
                    }
                }
            }
        }
    }
}