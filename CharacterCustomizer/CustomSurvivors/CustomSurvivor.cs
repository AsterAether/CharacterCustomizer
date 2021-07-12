using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CharacterCustomizer.Util.Config;
using R2API;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Path = System.IO.Path;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomSurvivor : IConfigProvider
    {
        public ConfigEntry<bool> Enabled { get; private set; }
        public ConfigEntry<bool> UpdateVanillaValues { get; private set; }
        private ConfigFile Config { get; set; }

        private ManualLogSource Logger { get; set; }

        public readonly Dictionary<int, CustomSkillDefinition> Skills = new Dictionary<int, CustomSkillDefinition>();


        public CustomBodyDefinition BodyDefinition { get; private set; }

        public SurvivorDef SurvivorDef { get; private set; }

        public string CommonName { get; private set; }

        private bool _configsLoaded;

        public CustomSurvivor(SurvivorDef survivorDef, ConfigFile configFile, ManualLogSource logger)
        {
            Config = configFile;
            Logger = logger;

            SurvivorDef = survivorDef;

            CommonName = Regex.Replace(Language.english.GetLocalizedStringByToken(survivorDef.displayNameToken),
                @"[^A-Za-z]+", string.Empty);

            Enabled = Config.Bind(
                CommonName,
                CommonName + " Enabled",
                false,
                "If changes for this character are enabled. Set to true to generate options on next startup!");

            Enabled.SettingChanged += (sender, args) =>
            {
                if (!Enabled.Value) return;
                LoadConfigs();
                OverrideSurvivorBase();
            };

            if (!Enabled.Value) return;

            LoadConfigs();
        }

        private void LoadConfigs()
        {
            if (_configsLoaded) return;
            _configsLoaded = true;
            UpdateVanillaValues = Config.Bind(
                CommonName,
                CommonName + " UpdateVanillaValues",
                true,
                "Write default values in descriptions of settings. Will flip to false after doing it once.");

            BodyDefinition = new CustomBodyDefinition(this, CommonName);


            var skills = SurvivorDef.bodyPrefab.GetComponents<GenericSkill>();
            foreach (var genericSkill in skills)
            {
                foreach (var variant in genericSkill.skillFamily.variants)
                {
                    if (Skills.ContainsKey(variant.skillDef.skillIndex)) continue;

                    var skill = new CustomSkillDefinition(this, variant.skillDef.skillIndex,
                        Regex.Replace(Language.english.GetLocalizedStringByToken(variant.skillDef.skillNameToken),
                            @"[^A-Za-z]+", string.Empty)
                    );
                    Skills.Add(skill.SkillIndex, skill);
                }
            }
        }

        private void OnBodyChanged(CustomBodyDefinition skillDefinition, IFieldChanger changed)
        {
            var prefabBody = SurvivorDef.bodyPrefab.GetComponent<CharacterBody>();
            changed.Apply(prefabBody);
            Logger.LogInfo(SurvivorDef.cachedName + "'s body changed.");

            foreach (var masterController in PlayerCharacterMasterController.instances)
            {
                var body = masterController.master.GetBody();
                if (body.name != SurvivorDef.bodyPrefab.name + "(Clone)") continue;
                changed.Apply(body);
                body.RecalculateStats();

                Logger.LogInfo("Recalculated live body stats of " + SurvivorDef.cachedName);
            }
        }

        private void OnSkillChanged(CustomSkillDefinition skillDef, IFieldChanger changed)
        {
            var def = SkillCatalog.GetSkillDef(skillDef.SkillIndex);
            changed.Apply(def);
            Logger.LogInfo("Skill '" + skillDef.CommonName + "' of Survivor '" + SurvivorDef.cachedName +
                           "' overwritten");

            foreach (var masterController in PlayerCharacterMasterController.instances)
            {
                var body = masterController.master.GetBody();
                foreach (var genericSkill in body.GetComponents<GenericSkill>())
                {
                    genericSkill.RecalculateValues();
                }

                Logger.LogInfo("Recalculated live skills of " + SurvivorDef.cachedName);
            }
        }

        public void OverrideSurvivorBase()
        {
            Logger.LogInfo("Overriding " + CommonName);

            foreach (var customSkillDefinition in Skills.Values)
            {
                var def = SkillCatalog.GetSkillDef(customSkillDefinition.SkillIndex);
                customSkillDefinition.Apply(def);
                Logger.LogInfo("Skill: " + customSkillDefinition.CommonName +
                               " overwritten");
                customSkillDefinition.OnFieldChanged += OnSkillChanged;
            }

            var body = SurvivorDef.bodyPrefab.GetComponent<CharacterBody>();
            BodyDefinition.Apply(body);

            BodyDefinition.OnFieldChanged += OnBodyChanged;
        }

        public void OnStop()
        {
            if (UpdateVanillaValues != null)
                UpdateVanillaValues.Value = false;
        }


        public string GetSectionName()
        {
            return CommonName;
        }

        public ConfigEntryDescriptionWrapper<T> BindConfig<T>(string key, T defaultVal,
            string description)
        {
            // Vanilla values need to be updated every time in the description when the game starts, only the boolean values don't need to be overwritten anymore, so tell that the config wrapper with the boolean
            return new ConfigEntryDescriptionWrapper<T>(
                Config.Bind(CommonName, key, defaultVal, description),
                UpdateVanillaValues.Value);
        }

        public ConfigEntryDescriptionWrapper<T> BindConfig<T>(string key, string description)
        {
            return BindConfig<T>(key, default, description);
        }
    }
}