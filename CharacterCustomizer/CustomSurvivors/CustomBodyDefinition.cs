using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CharacterCustomizer.Util.Config;
using RoR2;
using UnityEngine;
using Logger = BepInEx.Logging.Logger;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomBodyDefinition : FieldChangerBag
    {
        public string CommonName { get; set; }

        public delegate void CustomFieldChanged(CustomBodyDefinition skillDefinition, IFieldChanger changed);

        public CustomFieldChanged OnFieldChanged;

        public CustomBodyDefinition(IConfigProvider configProvider, string commonName) : base(configProvider)
        {
            CommonName = commonName;

            AddFieldConfig<float>(
                "BaseMaxHealth",
                "The base health of your survivor",
                "baseMaxHealth");

            AddFieldConfig<float>(
                "BaseRegen",
                "The base regen of your survivor",
                "baseRegen");

            AddFieldConfig<float>(
                "BaseMaxShield",
                "the base max shield of you survivor",
                "baseMaxShield");

            AddFieldConfig<float>(
                "BaseMoveSpeed",
                "The base move speed of your survivor",
                "baseMoveSpeed");

            AddFieldConfig<float>(
                "BaseAcceleration",
                "The base acceleration of your survivor",
                "baseAcceleration");

            AddFieldConfig<float>(
                "BaseJumpPower",
                "The base jump power of your survivor",
                "baseJumpPower");

            AddFieldConfig<float>(
                "BaseDamage",
                "The base damage of your survivor",
                "baseDamage");

            AddFieldConfig<float>(
                "BaseAttackSpeed",
                "The base attack speed of your survivor",
                "baseAttackSpeed");

            AddFieldConfig<float>(
                "BaseCrit",
                "The base crit chance of your survivor",
                "baseCrit");

            AddFieldConfig<float>(
                "BaseArmor",
                "The base armor of your survivor",
                "baseArmor");

            AddFieldConfig<int>(
                "BaseJumpCount",
                "The base jump count of your survivor",
                "baseJumpCount");

            AddFieldConfig<float>(
                "LevelMaxHealth",
                "The max health per level your survivor gets.",
                "levelMaxHealth");

            AddFieldConfig<float>(
                "LevelRegen",
                "The regen per level your survivor gets.",
                "levelRegen");

            AddFieldConfig<float>(
                "LevelMaxShield",
                "The max shield per level your survivor gets",
                "levelMaxShield");

            AddFieldConfig<float>(
                "LevelMoveSpeed",
                "The move speed per level your survivor gets",
                "levelMoveSpeed");

            AddFieldConfig<float>(
                "LevelJumpPower",
                "The jump power per level your survivor gets",
                "levelJumpPower");

            AddFieldConfig<float>(
                "LevelDamage",
                "The damage per level your survivor gets",
                "levelDamage");

            AddFieldConfig<float>(
                "LevelAttackSpeed",
                "The attack speed per level your survivor gets",
                "levelAttackSpeed");

            AddFieldConfig<float>(
                "LevelCrit",
                "The crit chance per level your survivor gets",
                "levelCrit");

            AddFieldConfig<float>(
                "LevelArmor",
                "The armor per level your survivor gets",
                "levelArmor");

            foreach (var fieldChanger in _fieldChangers.Values)
            {
                fieldChanger.AddFieldChangedListener(InternalFieldChanged);
            }
        }

        public sealed override void AddFieldConfig<T>(string key, string description, string fieldName,
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