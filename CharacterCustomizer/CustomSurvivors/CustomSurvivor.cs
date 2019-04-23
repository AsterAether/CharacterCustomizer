using System;
using System.Collections.Generic;
using System.Linq;
using AetherLib.Util.Config;
using BepInEx.Configuration;
using BepInEx.Logging;
using R2API;
using RoR2;
using UnityEngine;
using ValueType = AetherLib.Util.Config.ValueType;

namespace CharacterCustomizer.CustomSurvivors
{
    public abstract class CustomSurvivor
    {
        public readonly List<IMarkdownString> MarkdownConfigDefinitions = new List<IMarkdownString>();

        public readonly List<ConfigDefinition> NonMarkDownConfigDefinitions = new List<ConfigDefinition>();

        protected ConfigFile Config { get; private set; }

        protected ManualLogSource Logger { get; private set; }

        public string CharacterName { get; }

        public string PrimarySkillName { get; }

        public string SecondarySkillName { get; }

        public string UtilitySkillName { get; }

        public string SpecialSkillName { get; }

        public SurvivorIndex SurvivorIndex { get; }

        public CustomSkillDefinition PrimarySkill { get; private set; }

        public CustomSkillDefinition SecondarySkill { get; private set; }

        public CustomSkillDefinition UtilitySkill { get; private set; }

        public CustomSkillDefinition SpecialSkill { get; private set; }

        public List<CustomSkillDefinition> ExtraSkills = new List<CustomSkillDefinition>();

        public List<string> ExtraSkillNames = new List<string>();

        public CustomBodyDefinition BodyDefinition { get; private set; }

        protected CustomSurvivor(SurvivorIndex index, string characterName, string primarySkillName,
            string secondarySkillName,
            string utilitySkillName, string specialSkillName)
        {
            SurvivorIndex = index;

            CharacterName = characterName;
            PrimarySkillName = primarySkillName;
            SecondarySkillName = secondarySkillName;
            UtilitySkillName = utilitySkillName;
            SpecialSkillName = specialSkillName;
        }

        public void InitVariables(ConfigFile config, ManualLogSource logger)
        {
            Config = config;
            Logger = logger;
        }

        public void InitBaseConfigValues()
        {
            PrimarySkill = new CustomSkillDefinition(this, PrimarySkillName);
            SecondarySkill = new CustomSkillDefinition(this, SecondarySkillName);
            UtilitySkill = new CustomSkillDefinition(this, UtilitySkillName);
            SpecialSkill = new CustomSkillDefinition(this, SpecialSkillName);
            foreach (string skillName in ExtraSkillNames)
            {
                ExtraSkills.Add(new CustomSkillDefinition(this, skillName));
            }

            BodyDefinition = new CustomBodyDefinition(this, CharacterName);
        }

        public void OverrideSurvivorBase(SurvivorDef survivor)
        {
            List<CustomSkillDefinition> skillDefs = new List<CustomSkillDefinition>(ExtraSkills);
            skillDefs.AddRange(new[] {PrimarySkill, SecondarySkill, UtilitySkill, SpecialSkill});

            GenericSkill[] skills = survivor.bodyPrefab.GetComponents<GenericSkill>();
            foreach (GenericSkill genericSkill in skills)
            {
                foreach (CustomSkillDefinition skillDef in skillDefs)
                {
                    if (genericSkill.skillName == skillDef.SkillName)
                    {
                        skillDef.AllFields.ForEach(changer => { changer.Apply(genericSkill); });
                    }
                }
            }

            CharacterBody body = survivor.bodyPrefab.GetComponent<CharacterBody>();
            BodyDefinition.AllFields.ForEach(changer => { changer.Apply(body); });
        }

        public void Patch()
        {
            InitBaseConfigValues();
            InitConfigValues();

            OverrideGameValues();
            WriteNewHooks();
        }

        public abstract void InitConfigValues();

        public abstract void OverrideGameValues();

        public abstract void WriteNewHooks();

        public ValueConfigWrapper<int> WrapConfigInt(string key, string description)
        {
            ValueConfigWrapper<int> conf = Config.ValueWrap(CharacterName, key, description, 0);
            MarkdownConfigDefinitions.Add(conf);
            return conf;
        }

        public ValueConfigWrapper<string> WrapConfigString(string key, string description)
        {
            ValueConfigWrapper<string> conf = Config.ValueWrap(CharacterName, key, ValueType.String, description);
            MarkdownConfigDefinitions.Add(conf);
            return conf;
        }

        public ValueConfigWrapper<string> WrapConfigBool(string key, string description)
        {
            ValueConfigWrapper<string> conf = Config.ValueWrap(CharacterName, key, ValueType.Bool, description);
            MarkdownConfigDefinitions.Add(conf);
            return conf;
        }

        public ValueConfigWrapper<string> WrapConfigFloat(string key, string description)
        {
            ValueConfigWrapper<string> conf = Config.ValueWrap(CharacterName, key, ValueType.Float, description);
            MarkdownConfigDefinitions.Add(conf);
            return conf;
        }

        public ConfigWrapper<bool> WrapConfigStandardBool(string key, string description)
        {
            ConfigWrapper<bool> conf = Config.Wrap(CharacterName, key, description, false);
            NonMarkDownConfigDefinitions.Add(conf.Definition);
            return conf;
        }
    }
}