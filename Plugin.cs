using BepInEx;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AutomaticLaunchCodes
{
    [BepInPlugin("MegaPiggy.AutomaticLaunchCodes", "AutomaticLaunchCodes", "1.0.0")]
    [BepInProcess("OuterWilds_Alpha_1_2.exe")]
    [BepInProcess("OuterWilds_AlphaDemo_PC.exe")]
    public class AutomaticLaunchCodes : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogDebug($"{nameof(AutomaticLaunchCodes)} was started");
            Logger.LogDebug($"The mod script has been placed in the '{this.gameObject.name}'");
            new Harmony("MegaPiggy.AutomaticLaunchCodes").PatchAll();
            Logger.LogDebug($"Harmony patching complete");
        }
    }

    [HarmonyPatch(typeof(LaunchTerminal), nameof(LaunchTerminal.Start))]
    public static class LaunchTerminalPatch
    {
        public static void Postfix(LaunchTerminal __instance)
        {
            Initialize(__instance);
        }

        public static void Initialize(LaunchTerminal launchTerminal)
        {
            PlayerData.LearnLaunchCodes();
            GlobalMessenger.FireEvent("ActivateLaunchTower");
            launchTerminal._interactVolume.Disable();
        }
    }
}
