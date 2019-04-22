using System;
using System.Collections.Generic;
using System.Linq;
using AetherLib.Util.Config;
using BepInEx.Configuration;
using BepInEx.Logging;
using R2API;
using RoR2;
using UnityEngine;

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
        }

        public void OverrideSkills(SurvivorDef survivor)
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
                        skillDef.BaseRechargeInterval.SetDefaultValue(genericSkill.baseRechargeInterval);
                        if (skillDef.BaseRechargeInterval.IsNotDefault())
                        {
                            genericSkill.baseRechargeInterval = skillDef.BaseRechargeInterval.FloatValue;
                        }

                        skillDef.ShootDelay.SetDefaultValue(genericSkill.shootDelay);
                        if (skillDef.ShootDelay.IsNotDefault())
                        {
                            genericSkill.shootDelay = skillDef.ShootDelay.FloatValue;
                        }

                        skillDef.BaseMaxStock.SetDefaultValue(genericSkill.baseMaxStock);
                        if (skillDef.BaseMaxStock.IsNotDefault())
                        {
                            genericSkill.baseMaxStock = skillDef.BaseMaxStock.Value;
                        }

                        skillDef.RechargeStock.SetDefaultValue(genericSkill.rechargeStock);
                        if (skillDef.RechargeStock.IsNotDefault())
                        {
                            genericSkill.rechargeStock = skillDef.RechargeStock.Value;
                        }

                        skillDef.IsCombatSkill.SetDefaultValue(genericSkill.isCombatSkill);
                        if (skillDef.IsCombatSkill.IsNotDefault())
                        {
                            genericSkill.isCombatSkill = skillDef.IsCombatSkill.BoolValue;
                        }

                        skillDef.NoSprint.SetDefaultValue(genericSkill.noSprint);
                        if (skillDef.NoSprint.IsNotDefault())
                        {
                            genericSkill.noSprint = skillDef.NoSprint.BoolValue;
                        }

                        skillDef.RequiredStock.SetDefaultValue(genericSkill.requiredStock);
                        if (skillDef.RequiredStock.IsNotDefault())
                        {
                            genericSkill.requiredStock = skillDef.RequiredStock.Value;
                        }

                        skillDef.StockToConsume.SetDefaultValue(genericSkill.stockToConsume);
                        if (skillDef.StockToConsume.IsNotDefault())
                        {
                            genericSkill.stockToConsume = skillDef.StockToConsume.Value;
                        }

                        skillDef.BaseRechargeInterval.SetDefaultValue(genericSkill.baseRechargeInterval);
                        if (skillDef.BaseRechargeInterval.IsNotDefault())
                        {
                            genericSkill.baseRechargeInterval = skillDef.BaseRechargeInterval.FloatValue;
                        }

                        skillDef.MustKeyPress.SetDefaultValue(genericSkill.mustKeyPress);
                        if (skillDef.MustKeyPress.IsNotDefault())
                        {
                            genericSkill.mustKeyPress = skillDef.MustKeyPress.BoolValue;
                        }

                        skillDef.BeginSkillCooldownOnSkillEnd.SetDefaultValue(genericSkill
                            .beginSkillCooldownOnSkillEnd);
                        if (skillDef.BeginSkillCooldownOnSkillEnd.IsNotDefault())
                        {
                            genericSkill.beginSkillCooldownOnSkillEnd =
                                skillDef.BeginSkillCooldownOnSkillEnd.BoolValue;
                        }

                        skillDef.CanceledFromSprinting.SetDefaultValue(genericSkill.canceledFromSprinting);
                        if (skillDef.CanceledFromSprinting.IsNotDefault())
                        {
                            genericSkill.canceledFromSprinting = skillDef.CanceledFromSprinting.BoolValue;
                        }

                        skillDef.IsBullets.SetDefaultValue(genericSkill.isBullets);
                        if (skillDef.IsBullets.IsNotDefault())
                        {
                            genericSkill.isBullets = skillDef.IsBullets.BoolValue;
                        }
                    }
                }
            }
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
            ValueConfigWrapper<string> conf = Config.ValueWrap(CharacterName, key, false, description);
            MarkdownConfigDefinitions.Add(conf);
            return conf;
        }

        public ValueConfigWrapper<string> WrapConfigFloat(string key, string description)
        {
            ValueConfigWrapper<string> conf = Config.ValueWrap(CharacterName, key, true, description);
            MarkdownConfigDefinitions.Add(conf);
            return conf;
        }

        public ConfigWrapper<bool> WrapConfigBool(string key, string description)
        {
            ConfigWrapper<bool> conf = Config.Wrap(CharacterName, key, description, false);
            NonMarkDownConfigDefinitions.Add(conf.Definition);
            return conf;
        }
    }
}