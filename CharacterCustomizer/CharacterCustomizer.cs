using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using CharacterCustomizer.CustomSurvivors;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.ContentManagement;
using RoR2.Skills;
using RoR2.UI;
using UnityEngine;
using SkillCatalog = IL.RoR2.Skills.SkillCatalog;

namespace CharacterCustomizer
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("Aster.CharacterCustomizer", "CharacterCustomizer", "<version>")]
    [R2APISubmoduleDependency(nameof(SurvivorAPI))]
    public class CharacterCustomizer : BaseUnityPlugin
    {

        private readonly List<CustomSurvivor> _survivors = new List<CustomSurvivor>();

        private IEnumerator AfterLoad(On.RoR2.RoR2Application.orig_OnLoad orig, RoR2Application self)
        {
            yield return orig(self);
            foreach (SurvivorDef survivorDef in ContentManager.survivorDefs)
            {
                var customSurvivor = new CustomSurvivor(survivorDef, Config, Logger);
                if (customSurvivor.Enabled.Value)
                    customSurvivor.OverrideSurvivorBase();
                _survivors.Add(customSurvivor);
            }
        }

        public void Awake()
        {
            On.RoR2.RoR2Application.OnLoad += AfterLoad;
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