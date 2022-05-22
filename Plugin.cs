using BepInEx;
using CAMOWA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AutomaticLaunchCodes
{
    [BepInDependency("locochoco.plugins.CAMOWA", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("MegaPiggy.AutomaticLaunchCodes", "AutomaticLaunchCodes", "1.0.0")]
    [BepInProcess("OuterWilds_Alpha_1_2.exe")]
    public class AutomaticLaunchCodes : BaseUnityPlugin
    {

        private static string gamePath;
        public static string DllExecutablePath
        {
            get
            {
                if (string.IsNullOrEmpty(gamePath))
                    gamePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return gamePath;
            }

            private set { }
        }

        private void Awake()
        {
            Debug.Log($"{nameof(AutomaticLaunchCodes)} was started");
            Debug.Log($"The mod script has been placed in the '{this.gameObject.name}'");
            SceneLoading.OnSceneLoad += SceneLoading_OnSceneLoad;
        }

        private void SceneLoading_OnSceneLoad(int sceneId)
        {
            if (sceneId == 1)
            {
                Initialize();
            }
        }

        private IEnumerator DelayLaunch()
        {
            yield return new WaitForSeconds(1);
            GlobalMessenger.FireEvent("ActivateLaunchTower");
            ((LaunchTerminal)UnityEngine.Object.FindObjectOfType(typeof(LaunchTerminal)))._interactVolume.Disable();
        }

        private void Initialize()
        {
            if (Application.loadedLevel == 1)
            {
                PlayerData.LearnLaunchCodes();
                StartCoroutine(DelayLaunch());
            }
        }
    }
}
