using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AetherLib.Util.Config;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using R2API;
using RoR2;
using UnityEngine;
using Path = System.IO.Path;

namespace CharacterCustomizer.CustomSurvivors
{
    public abstract class CustomSurvivor
    {
        public readonly List<IMarkdownString> MarkdownConfigEntries = new List<IMarkdownString>();

        protected ConfigFile Config { get; private set; }

        protected ManualLogSource Logger { get; private set; }

        public string CharacterName { get; }

        public string PrimarySkillName { get; }

        public string SecondarySkillName { get; }

        public string UtilitySkillName { get; }

        public string SpecialSkillName { get; }
        
        public string PrimarySkillReplaceOld { get; private set; }

        public string SecondarySkillReplaceOld { get; private set;}

        public string UtilitySkillReplaceOld { get; private set;}

        public string SpecialSkillReplaceOld { get; private set;}
        

        public SurvivorIndex SurvivorIndex { get; }

        public CustomSkillDefinition PrimarySkill { get; private set; }

        public CustomSkillDefinition SecondarySkill { get; private set; }

        public CustomSkillDefinition UtilitySkill { get; private set; }

        public CustomSkillDefinition SpecialSkill { get; private set; }

        public List<CustomSkillDefinition> ExtraSkills = new List<CustomSkillDefinition>();

        public List<string> ExtraSkillNames = new List<string>();

        public CustomBodyDefinition BodyDefinition { get; private set; }

        private bool _updateVanillaValues;

        protected CustomSurvivor(SurvivorIndex index, string characterName, string primarySkillName,
            string secondarySkillName,
            string utilitySkillName, string specialSkillName, bool updateVanillaValues)
        {
            _updateVanillaValues = updateVanillaValues;
            SurvivorIndex = index;

            CharacterName = characterName;
            PrimarySkillName = primarySkillName;
            SecondarySkillName = secondarySkillName;
            UtilitySkillName = utilitySkillName;
            SpecialSkillName = specialSkillName;
        }

        protected void SetPrimarySkillReplaceOldName(string old)
        {
            PrimarySkillReplaceOld = old;
        }
        
        
        protected void SetSecondarySkillReplaceOldName(string old)
        {
            SecondarySkillReplaceOld = old;
        }
        
        
        protected void SetUtilitySkillReplaceOldName(string old)
        {
            UtilitySkillReplaceOld = old;
        }
        
        
        protected void SetSpecialSkillReplaceOldName(string old)
        {
            SpecialSkillReplaceOld = old;
        }

        public void InitVariables(ConfigFile file, ManualLogSource logger)
        {
            Config = file;
            // Used for creating a file per survivor, though that breaks compatibility with BepInEx.ConfigurationManager, which is the better solution to editing anyway
            // Config = new ConfigFile(Path.Combine(Paths.ConfigPath, "CustomSurvivors", CharacterName + ".cfg"), true);
            Logger = logger;
        }

        public void InitBaseConfigValues()
        {
            PrimarySkill = new CustomSkillDefinition(this, PrimarySkillName, PrimarySkillReplaceOld);
            SecondarySkill = new CustomSkillDefinition(this, SecondarySkillName, SecondarySkillReplaceOld);
            UtilitySkill = new CustomSkillDefinition(this, UtilitySkillName, UtilitySkillReplaceOld);
            SpecialSkill = new CustomSkillDefinition(this, SpecialSkillName, SpecialSkillReplaceOld);
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
                    if (skillDef.ReplaceOld != null && genericSkill.skillName == skillDef.ReplaceOld)
                    {
                        genericSkill.skillName = skillDef.SkillName;
                    }
                    if (genericSkill.skillName == skillDef.SkillName)
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

        public ConfigEntryDescriptionWrapper<T> BindConfig<T>(string key, T defaultVal,
            string description)
        {
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