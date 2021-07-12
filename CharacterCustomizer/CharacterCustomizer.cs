using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using CharacterCustomizer.CustomSurvivors;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.ContentManagement;
using UnityEngine;

namespace CharacterCustomizer
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("AsterAether.CharacterCustomizer", "CharacterCustomizer", "<version>")]
    [R2APISubmoduleDependency(nameof(SurvivorAPI))]
    public class CharacterCustomizer : BaseUnityPlugin
    {
        private readonly List<CustomSurvivor> _survivors = new List<CustomSurvivor>();
        private ConfigEntry<KeyCode> ReloadConfigButton { get; set; }

        public void Awake()
        {
            ReloadConfigButton = Config.Bind(
                "General",
                "ReloadConfigButton",
                KeyCode.F8,
                "Loads the config from disk and applies all changes.");
            
            On.RoR2.RoR2Application.OnLoad += AfterLoad;
        }

        private IEnumerator AfterLoad(On.RoR2.RoR2Application.orig_OnLoad orig, RoR2Application self)
        {
            yield return orig(self);

            ApplyGeneralSettings();

            foreach (var survivorDef in ContentManager.survivorDefs)
            {
                var customSurvivor = new CustomSurvivor(survivorDef, Config, Logger);
                if (customSurvivor.Enabled.Value)
                    customSurvivor.OverrideSurvivorBase();
                _survivors.Add(customSurvivor);
            }
        }

        private void ApplyGeneralSettings()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(ReloadConfigButton.Value))
            {
                Config.Reload();
            }
        }

        private void OnDestroy()
        {
            _survivors.ForEach(survivor => survivor.OnStop());
            Config.Save();
        }

        private void OnApplicationQuit()
        {
            _survivors.ForEach(survivor => survivor.OnStop());
            Config.Save();
        }
    }
}