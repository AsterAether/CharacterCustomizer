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
using UnityEngine;
using Path = System.IO.Path;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomSurvivor
    {
        public ConfigEntry<bool> Enabled { get; private set; }
        public ConfigEntry<bool> UpdateVanillaValues { get; private set; }
        private ConfigFile Config { get; set; }

        private ManualLogSource Logger { get; set; }

        public readonly Dictionary<int, CustomSkillDefinition> Skills = new Dictionary<int, CustomSkillDefinition>();


        public CustomBodyDefinition BodyDefinition { get; private set; }

        public SurvivorDef SurvivorDef { get; private set; }

        public string CommonName { get; private set; }

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

            if (!Enabled.Value) return;

            UpdateVanillaValues = Config.Bind(
                CommonName,
                CommonName + " UpdateVanillaValues",
                true,
                "Write default values in descriptions of settings. Will flip to false after doing it once.");

            BodyDefinition = new CustomBodyDefinition(this);
            BodyDefinition.OnFieldChanged += OnBodyChangedPlaceholder;


            var skills = survivorDef.bodyPrefab.GetComponents<GenericSkill>();
            foreach (var genericSkill in skills)
            {
                foreach (var variant in genericSkill.skillFamily.variants)
                {
                    if (Skills.ContainsKey(variant.skillDef.skillIndex)) continue;

                    var skill = new CustomSkillDefinition(this, variant.skillDef.skillIndex,
                        Regex.Replace(Language.english.GetLocalizedStringByToken(variant.skillDef.skillNameToken),
                            @"[^A-Za-z]+", string.Empty)
                    );
                    skill.OnFieldChanged += OnSkillChangedPlaceholder;
                    Skills.Add(skill.SkillIndex, skill);
                }
            }
        }


        private void OnBodyChangedPlaceholder(CustomBodyDefinition skillDefinition, IFieldChanger changed)
        {
        }

        private void OnSkillChangedPlaceholder(CustomSkillDefinition skillDef, IFieldChanger changed)
        {
        }

        private void OnBodyChanged(CustomBodyDefinition skillDefinition, IFieldChanger changed)
        {
            var body = SurvivorDef.bodyPrefab.GetComponent<CharacterBody>();
            changed.Apply(body);

            var liveBodies = GameObject.FindGameObjectsWithTag("Player");
            foreach (var liveBody in liveBodies)
            {
                if (liveBody.name != SurvivorDef.bodyPrefab.name + "(Clone)") continue;
                var liveChar = liveBody.GetComponent<CharacterBody>();
                changed.Apply(liveChar);
                liveChar.RecalculateStats();

                Logger.LogInfo("Recalculated live body stats of " + SurvivorDef.cachedName);
            }
        }

        private void OnSkillChanged(CustomSkillDefinition skillDef, IFieldChanger changed)
        {
            var def = SkillCatalog.GetSkillDef(skillDef.SkillIndex);
            changed.Apply(def);
            Logger.LogInfo("Skill '" + skillDef.CommonName + "' of Survivor '" + SurvivorDef.cachedName +
                           "' overwritten");

            GameObject[] liveBodies = GameObject.FindGameObjectsWithTag("Player");
            foreach (var liveBody in liveBodies)
            {
                if (liveBody.name == SurvivorDef.bodyPrefab.name + "(Clone)")
                {
                    foreach (GenericSkill genericSkill in liveBody.GetComponents<GenericSkill>())
                    {
                        genericSkill.RecalculateValues();
                    }

                    Logger.LogInfo("Recalculated live skills of " + SurvivorDef.cachedName);
                }
            }
        }

        public void OverrideSurvivorBase()
        {
            Logger.LogInfo("Overriding " + CommonName);

            foreach (var customSkillDefinition in Skills.Values)
            {
                var def = SkillCatalog.GetSkillDef(customSkillDefinition.SkillIndex);
                customSkillDefinition.AllFields.ForEach(changer => { changer.Apply(def); });
                Logger.LogInfo("Skill: " + customSkillDefinition.CommonName +
                               " overwritten");
                // customSkillDefinition.OnFieldChanged -= OnSkillChangedPlaceholder;
                // customSkillDefinition.OnFieldChanged += OnSkillChanged;
            }

            var body = SurvivorDef.bodyPrefab.GetComponent<CharacterBody>();
            BodyDefinition.AllFields.ForEach(changer => { changer.Apply(body); });

            // BodyDefinition.OnFieldChanged -= OnBodyChangedPlaceholder;
            // BodyDefinition.OnFieldChanged += OnBodyChanged;
        }

        public void OnStop()
        {
            if (UpdateVanillaValues != null)
                UpdateVanillaValues.Value = false;
        }


        public ConfigEntryDescriptionWrapper<T> BindConfig<T>(string key, T defaultVal,
            string description)
        {
            // Vanilla values need to be updated every time in the description when the game starts, only the boolean values don't need to be overwritten anymore, so tell that the config wrapper with the boolean
            return new ConfigEntryDescriptionWrapper<T>(
                Config.Bind(CommonName, key, defaultVal, description),
                UpdateVanillaValues.Value);
        }
    }
}