using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    public abstract class CustomSurvivor
    {
        public readonly List<IMarkdownString> MarkdownConfigEntries = new List<IMarkdownString>();

        private ConfigFile Config { get; set; }

        private ManualLogSource Logger { get; set; }

        public readonly List<CustomSkillDefinition> Skills = new List<CustomSkillDefinition>();

        public SurvivorIndex SurvivorIndex { get; }

        public string CharacterName { get; }
        public CustomBodyDefinition BodyDefinition { get; private set; }

        public SurvivorDef SurvivorDef { get; private set; }

        private bool _updateVanillaValues;

        protected CustomSurvivor(SurvivorIndex index, string characterName, string survivorNameToken,
            bool updateVanillaValues, ConfigFile configFile, ManualLogSource logger)
        {
            _updateVanillaValues = updateVanillaValues;
            SurvivorIndex = index;
            Config = configFile;
            Logger = logger;

            CharacterName = characterName;
            BodyDefinition = new CustomBodyDefinition(this, characterName, survivorNameToken);
            BodyDefinition.OnFieldChanged += OnBodyChangedPlaceholder;
        }

        protected void AddSkill(string commonName, int skillIndex)
        {
            CustomSkillDefinition skill = new CustomSkillDefinition(this, skillIndex, commonName);
            skill.OnFieldChanged += OnSkillChangedPlaceholder;
            Skills.Add(skill);
        }

        private void OnBodyChangedPlaceholder(CustomBodyDefinition skillDefinition, IFieldChanger changed)
        {
        }

        private void OnSkillChangedPlaceholder(CustomSkillDefinition skillDef, IFieldChanger changed)
        {
        }

        private void OnBodyChanged(CustomBodyDefinition skillDefinition, IFieldChanger changed)
        {
            CharacterBody body = SurvivorDef.bodyPrefab.GetComponent<CharacterBody>();
            changed.Apply(body);

            GameObject[] liveBodies = GameObject.FindGameObjectsWithTag("Player");
            foreach (var liveBody in liveBodies)
            {
                if (liveBody.name == SurvivorDef.bodyPrefab.name + "(Clone)")
                {
                    var liveChar = liveBody.GetComponent<CharacterBody>();
                    changed.Apply(liveChar);
                    liveChar.RecalculateStats();

                    Logger.LogInfo("Recalculated live body stats of " + SurvivorDef.name);
                }
            }
        }

        private void OnSkillChanged(CustomSkillDefinition skillDef, IFieldChanger changed)
        {
            var def = SkillCatalog.GetSkillDef(skillDef.SkillIndex);
            changed.Apply(def);
            Logger.LogInfo("Skill '" + skillDef.CommonName + "' of Survivor '" + SurvivorDef.name +
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

                    Logger.LogInfo("Recalculated live skills of " + SurvivorDef.name);
                }
            }
        }

        public void OverrideSurvivorBase(SurvivorDef survivor)
        {
            Logger.LogInfo("Overriding " + survivor.name);
            SurvivorDef = survivor;

            GenericSkill[] skills = survivor.bodyPrefab.GetComponents<GenericSkill>();

            foreach (var customSkillDefinition in Skills)
            {
                var def = SkillCatalog.GetSkillDef(customSkillDefinition.SkillIndex);
                customSkillDefinition.AllFields.ForEach(changer => { changer.Apply(def); });
                Logger.LogInfo("Skill: " + customSkillDefinition.CommonName +
                               " overwritten");
                customSkillDefinition.OnFieldChanged -= OnSkillChangedPlaceholder;
                customSkillDefinition.OnFieldChanged += OnSkillChanged;
            }

            CharacterBody body = survivor.bodyPrefab.GetComponent<CharacterBody>();
            BodyDefinition.AllFields.ForEach(changer => { changer.Apply(body); });

            BodyDefinition.OnFieldChanged -= OnBodyChangedPlaceholder;
            BodyDefinition.OnFieldChanged += OnBodyChanged;
        }


        public ConfigEntryDescriptionWrapper<T> BindConfig<T>(string key, T defaultVal,
            string description)
        {
            // Vanilla values need to be updated every time in the description when the game starts, only the boolean values don't need to be overwritten anymore, so tell that the config wrapper with the boolean
            ConfigEntryDescriptionWrapper<T> entry =
                new ConfigEntryDescriptionWrapper<T>(Config.Bind(CharacterName, key, defaultVal, description),
                    _updateVanillaValues);
            MarkdownConfigEntries.Add(entry);
            return entry;
        }

        public ConfigEntryDescriptionWrapper<bool> BindConfigBool(string key, string description, bool defVal = false)
        {
            return BindConfig(key, defVal, description);
        }

        public ConfigEntryDescriptionWrapper<float> BindConfigFloat(string key, string description, float defVal = 0f)
        {
            return BindConfig(key, defVal, description);
        }

        public ConfigEntryDescriptionWrapper<int> BindConfigInt(string key, string description, int defVal = 0)
        {
            return BindConfig(key, defVal, description);
        }
    }
}