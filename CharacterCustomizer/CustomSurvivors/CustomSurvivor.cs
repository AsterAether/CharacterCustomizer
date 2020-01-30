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
using UnityEngine;
using Path = System.IO.Path;

namespace CharacterCustomizer.CustomSurvivors
{
    public abstract class CustomSurvivor
    {
        public readonly List<IMarkdownString> MarkdownConfigEntries = new List<IMarkdownString>();

        private ConfigFile Config { get; set; }

        private ManualLogSource Logger { get; set; }

        public readonly List<CustomSkillDefinition> PrimarySkills = new List<CustomSkillDefinition>();

        public readonly List<CustomSkillDefinition> SecondarySkills = new List<CustomSkillDefinition>();

        public readonly List<CustomSkillDefinition> UtilitySkills = new List<CustomSkillDefinition>();

        public readonly List<CustomSkillDefinition> SpecialSkills = new List<CustomSkillDefinition>();

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
            BodyDefinition.OnFieldChanged += OnBodyChanged;
        }

        protected void AddPrimarySkill(string commonName, string alt = null)
        {
            CustomSkillDefinition skill = new CustomSkillDefinition(this,
                BodyDefinition.SurvivorNameToken + "_PRIMARY_" + (alt != null ? alt + "_" : "") + "NAME", commonName);
            skill.OnFieldChanged += OnSkillChanged;
            PrimarySkills.Add(skill);
        }

        protected void AddSecondarySkill(string commonName, string alt = null)
        {
            CustomSkillDefinition skill = new CustomSkillDefinition(this,
                BodyDefinition.SurvivorNameToken + "_SECONDARY_" + (alt != null ? alt + "_" : "") + "NAME", commonName);
            skill.OnFieldChanged += OnSkillChanged;
            SecondarySkills.Add(skill);
        }

        protected void AddUtilitySkill(string commonName, string alt = null)
        {
            CustomSkillDefinition skill = new CustomSkillDefinition(this,
                BodyDefinition.SurvivorNameToken + "_UTILITY_" + (alt != null ? alt + "_" : "") + "NAME", commonName);
            skill.OnFieldChanged += OnSkillChanged;
            UtilitySkills.Add(skill);
        }

        protected void AddSpecialSkill(string commonName, string alt = null)
        {
            CustomSkillDefinition skill = new CustomSkillDefinition(this,
                BodyDefinition.SurvivorNameToken + "_SPECIAL_" + (alt != null ? alt + "_" : "") + "NAME", commonName);
            skill.OnFieldChanged += OnSkillChanged;
            SpecialSkills.Add(skill);
        }


        private void OnBodyChanged(CustomBodyDefinition skillDefinition, IFieldChanger changed)
        {
            CharacterBody body = SurvivorDef.bodyPrefab.GetComponent<CharacterBody>();
            BodyDefinition.AllFields.ForEach(changer => { changer.Apply(body); });
        }

        private void OnSkillChanged(CustomSkillDefinition skillDef, IFieldChanger changed)
        {
            GenericSkill[] skills = SurvivorDef.bodyPrefab.GetComponents<GenericSkill>();
            foreach (GenericSkill genericSkill in skills)
            {
                if (genericSkill.skillFamily.defaultSkillDef.skillNameToken == skillDef.SkillNameToken)
                {
                    skillDef.AllFields.ForEach(changer => { changer.Apply(genericSkill.skillFamily.defaultSkillDef); });
                }
            }
        }

        public void OverrideSurvivorBase(SurvivorDef survivor)
        {
            SurvivorDef = survivor;
            List<CustomSkillDefinition> skillDefs = new List<CustomSkillDefinition>();
            skillDefs.AddRange(PrimarySkills);
            skillDefs.AddRange(SecondarySkills);
            skillDefs.AddRange(UtilitySkills);
            skillDefs.AddRange(SpecialSkills);

            GenericSkill[] skills = survivor.bodyPrefab.GetComponents<GenericSkill>();
            foreach (GenericSkill genericSkill in skills)
            {
                foreach (CustomSkillDefinition skillDef in skillDefs)
                {
                    if (genericSkill.skillFamily.defaultSkillDef.skillNameToken == skillDef.SkillNameToken)
                    {
                        skillDef.AllFields.ForEach(changer =>
                        {
                            changer.Apply(genericSkill.skillFamily.defaultSkillDef);
                        });
                    }
                }
            }

            CharacterBody body = survivor.bodyPrefab.GetComponent<CharacterBody>();
            BodyDefinition.AllFields.ForEach(changer => { changer.Apply(body); });
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