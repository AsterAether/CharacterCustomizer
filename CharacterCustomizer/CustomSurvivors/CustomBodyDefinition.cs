using System;
using System.Collections.Generic;
using CharacterCustomizer.Util.Config;
using UnityEngine;
using Logger = BepInEx.Logging.Logger;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomBodyDefinition
    {
        public string CommonName { get; }
        
        public string SurvivorNameToken { get; }
        public FieldConfigWrapper<float> BaseMaxHealth { get; }
        public FieldConfigWrapper<float> BaseRegen { get; }
        public FieldConfigWrapper<float> BaseMaxShield { get; }
        public FieldConfigWrapper<float> BaseMoveSpeed { get; }
        public FieldConfigWrapper<float> BaseAcceleration { get; }
        public FieldConfigWrapper<float> BaseJumpPower { get; }
        public FieldConfigWrapper<float> BaseDamage { get; }
        public FieldConfigWrapper<float> BaseAttackSpeed { get; }
        public FieldConfigWrapper<float> BaseCrit { get; }
        public FieldConfigWrapper<float> BaseArmor { get; }
        public FieldConfigWrapper<int> BaseJumpCount { get; }
        public FieldConfigWrapper<float> LevelMaxHealth { get; }

        public FieldConfigWrapper<float> LevelRegen { get; }
        public FieldConfigWrapper<float> LevelMaxShield { get; }
        public FieldConfigWrapper<float> LevelMoveSpeed { get; }
        public FieldConfigWrapper<float> LevelJumpPower { get; }
        public FieldConfigWrapper<float> LevelDamage { get; }
        public FieldConfigWrapper<float> LevelAttackSpeed { get; }
        public FieldConfigWrapper<float> LevelCrit { get; }
        public FieldConfigWrapper<float> LevelArmor { get; }

        public List<IFieldChanger> AllFields { get; }

        public delegate void CustomFieldChanged(CustomBodyDefinition skillDefinition, IFieldChanger changed);

        public CustomFieldChanged OnFieldChanged;

        public CustomBodyDefinition(CustomSurvivor survivor, string commonName, string survivorNameToken)
        {
            CommonName = commonName;
            SurvivorNameToken = survivorNameToken;

            BaseMaxHealth = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseMaxHealth",
                    0f,
                    commonName + ": The base health of your survivor"),
                "baseMaxHealth");

            BaseRegen = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseRegen",
                    0f,
                    commonName + ": The base regen of your survivor"),
                "baseRegen");

            BaseMaxShield = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseMaxShield",
                    0f,
                    commonName + ": the base max shield of you survivor"),
                "baseMaxShield");

            BaseMoveSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseMoveSpeed",
                    0f,
                    commonName + ": The base move speed of your survivor"),
                "baseMoveSpeed");

            BaseAcceleration = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseAcceleration",
                    0f,
                    commonName + ": The base acceleration of your survivor"),
                "baseAcceleration");

            BaseJumpPower = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseJumpPower",
                    0f,
                    commonName + ": The base jump power of your survivor"),
                "baseJumpPower");

            BaseDamage = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseDamage",
                    0f,
                    commonName + ": The base damage of your survivor"),
                "baseDamage");

            BaseAttackSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseAttackSpeed",
                    0f,
                    commonName + ": The base attack speed of your survivor"),
                "baseAttackSpeed");

            BaseCrit = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseCrit",
                    0f,
                    commonName + ": The base crit chance of your survivor"),
                "baseCrit");

            BaseArmor = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseArmor",
                    0f,
                    commonName + ": The base armor of your survivor"),
                "baseArmor");

            BaseJumpCount = new FieldConfigWrapper<int>(survivor.BindConfig(
                    commonName + "BaseJumpCount",
                    0,
                    commonName + ": The base jump count of your survivor"),
                "baseJumpCount");

            LevelMaxHealth = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "LevelMaxHealth",
                    0f,
                    commonName + ": The max health per level your survivor gets."),
                "levelMaxHealth");

            LevelRegen = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "LevelRegen",
                    0f,
                    commonName + ": The regen per level your survivor gets."),
                "levelRegen");

            LevelMaxShield = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "LevelMaxShield",
                    0f,
                    commonName + ": The max shield per level your survivor gets"),
                "levelMaxShield");

            LevelMoveSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "LevelMoveSpeed",
                    0f,
                    commonName + ": The move speed per level your survivor gets"),
                "levelMoveSpeed");

            LevelJumpPower = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "LevelJumpPower",
                    0f,
                    commonName + ": The jump power per level your survivor gets"),
                "levelJumpPower");

            LevelDamage = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "LevelDamage",
                    0f,
                    commonName + ": The damage per level your survivor gets"),
                "levelDamage");

            LevelAttackSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "LevelAttackSpeed",
                    0f,
                    commonName + ": The attack speed per level your survivor gets"),
                "levelAttackSpeed");

            LevelCrit = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "LevelCrit",
                    0f,
                    commonName + ": The crit chance per level your survivor gets"),
                "levelCrit");

            LevelArmor = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "LevelArmor",
                    0f,
                    commonName + ": The armor per level your survivor gets"),
                "levelArmor");

            AllFields = new List<IFieldChanger>
            {
                BaseMaxHealth,
                BaseRegen,
                BaseMaxShield,
                BaseMoveSpeed,
                BaseAcceleration,
                BaseJumpPower,
                BaseDamage,
                BaseAttackSpeed,
                BaseCrit,
                BaseArmor,
                BaseJumpCount,

                LevelMaxHealth,
                LevelRegen,
                LevelMaxShield,
                LevelMoveSpeed,
                LevelJumpPower,
                LevelDamage,
                LevelAttackSpeed,
                LevelCrit,
                LevelArmor
            };

            foreach (var fieldChanger in AllFields)
            {
                fieldChanger.AddFieldChangedListener(InternalFieldChanged);
            }
        }

        private void InternalFieldChanged(IFieldChanger changer)
        {
            OnFieldChanged.Invoke(this, changer);
        }
    }
}