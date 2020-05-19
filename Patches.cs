using HarmonyLib;

namespace NoFlip
{
    // Prevents "OnFlipStickUpdate" from running. This prevents any flip tricks from being detected, but the flipstick foot can still control the board.
    [HarmonyPatch(typeof(PlayerController), "OnFlipStickUpdate")]
    [HarmonyPriority(Priority.First)]
    class FlipPatch
    {
        static bool Prefix()
        {
            if (Main.suppressionActive)
            {
                Main.ShowNoFlipMessage("Flip!");
                return false;
            }
            
            return true;
        }
    }

    // Prevents "AugmentedToeAxisVel" from returning anything except for 0f. This prevents any scooped tricks from being detected, but the popstick foot can still control the board.
    [HarmonyPatch(typeof(StickInput), "AugmentedToeAxisVel", MethodType.Getter)]
    [HarmonyPriority(Priority.First)]
    class ShuvPatch
    {
        static bool Prefix(ref float __result)
        {
            if(Main.suppressionActive)
            {
                __result = 0f;
                Main.ShowNoFlipMessage("Shuv!");
                return false;
            }
            return true;
        }
    }

    // This is to disable suppression after any pop
    [HarmonyPatch(typeof(PlayerState_Pop), "Exit")]
    class DisableAfterPop
    {
        static void Prefix()
        {
            if(Main.settings.disableAfterPop && Main.suppressionActive)
            {
                Main.suppressionActive = false;
                Main.delayTimer = 0;
                Main.ShowNoFlipMessage("Pop!");
            }
        }
    }

    // Disable after respawn
    [HarmonyPatch(typeof(Respawn), "DoRespawn")]
    [HarmonyPriority(Priority.Low)]
    class DisableAfterRespawn
    {
        static void Postfix()
        {
            if(Main.settings.disableAfterRespawn && Main.suppressionActive)
            {
                Main.suppressionActive = false;
                Main.delayTimer = 0;
                Main.ShowNoFlipMessage("Respawn!");
            }
        }
    }
}
