using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BepInEx.Configuration;
using CharacterCustomizer.Util.Config;

namespace CharacterCustomizer.CustomSurvivors
{
    public sealed class CustomSkillDefinition : FieldChangerBag
    {
        public int SkillIndex { get; }

        public string CommonName { get; }

        public delegate void CustomFieldChanged(CustomSkillDefinition skillDefinition, IFieldChanger changed);

        public CustomFieldChanged OnFieldChanged;


        public CustomSkillDefinition(IConfigProvider configProvider, int skillIndex, string commonName): base(configProvider)
        {
            SkillIndex = skillIndex;
            CommonName = commonName;

            AddFieldConfig<float>(
                "BaseRechargeInterval",
                "How long it takes for this skill to recharge after being used.",
                "baseRechargeInterval");

            AddFieldConfig<int>(
                "BaseMaxStock",
                "Maximum number of charges this skill can carry.",
                "baseMaxStock");

            AddFieldConfig<int>(
                "RechargeStock",
                "How much stock to restore on a recharge.",
                "rechargeStock");

            AddFieldConfig<bool>(
                "IsCombatSkill",
                "Whether or not this is considered a combat skill.",
                "isCombatSkill");

            AddFieldConfig<bool>(
                "CancelSprintingOnActivation",
                "Whether or not activating the skill forces off sprinting.",
                "cancelSprintingOnActivation");

            AddFieldConfig<int>(
                "RequiredStock",
                "How much stock is required to activate this skill.",
                "requiredStock");

            AddFieldConfig<int>(
                "StockToConsume",
                "How much stock to deduct when the skill is activated.",
                "stockToConsume");

            AddFieldConfig<bool>(
                "MustKeyPress",
                "The skill can't be activated if the key is held.",
                "mustKeyPress");

            AddFieldConfig<bool>(
                "BeginSkillCooldownOnSkillEnd",
                "Whether or not the cooldown waits until it leaves the set state",
                "beginSkillCooldownOnSkillEnd");

            AddFieldConfig<bool>(
                "CanceledFromSprinting",
                "Sprinting will actively cancel this ability.",
                "canceledFromSprinting");

            AddFieldConfig<bool>(
                "ResetCooldownTimerOnUse",
                "Whether or not it resets any progress on cooldowns.",
                "resetCooldownTimerOnUse");

            AddFieldConfig<bool>(
                "ForceSprintDuringState",
                "Whether or not this skill is considered 'mobility'. Currently just forces sprint.",
                "forceSprintDuringState");

            AddFieldConfig<bool>(
                "DontAllowPastMaxStocks",
                "Whether or not this skill can hold past it's maximum stock.",
                "dontAllowPastMaxStocks");


            foreach (var fieldChanger in _fieldChangers.Values)
            {
                fieldChanger.AddFieldChangedListener(InternalFieldChanged);
            }
        }
        
        public override void AddFieldConfig<T>(string key, string description, string fieldName,
            bool staticField = false)
        {
            base.AddFieldConfig<T>(CommonName + " " + key, CommonName + ": " + description, fieldName, staticField);
        }

        private void InternalFieldChanged(IFieldChanger changer)
        {
            OnFieldChanged?.Invoke(this, changer);
        }
    }
}