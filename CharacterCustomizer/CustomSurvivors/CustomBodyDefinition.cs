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
    public class CustomBodyDefinition
    {
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

        public CustomBodyDefinition(CustomSurvivor survivor)
        {
            
            BaseMaxHealth = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " BaseMaxHealth",
                    0f,
                    survivor.CommonName + ": The base health of your survivor"),
                "baseMaxHealth");

            BaseRegen = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " BaseRegen",
                    0f,
                    survivor.CommonName + ": The base regen of your survivor"),
                "baseRegen");

            BaseMaxShield = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " BaseMaxShield",
                    0f,
                    survivor.CommonName + ": the base max shield of you survivor"),
                "baseMaxShield");

            BaseMoveSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " BaseMoveSpeed",
                    0f,
                    survivor.CommonName + ": The base move speed of your survivor"),
                "baseMoveSpeed");

            BaseAcceleration = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " BaseAcceleration",
                    0f,
                    survivor.CommonName + ": The base acceleration of your survivor"),
                "baseAcceleration");

            BaseJumpPower = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " BaseJumpPower",
                    0f,
                    survivor.CommonName + ": The base jump power of your survivor"),
                "baseJumpPower");

            BaseDamage = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " BaseDamage",
                    0f,
                    survivor.CommonName + ": The base damage of your survivor"),
                "baseDamage");

            BaseAttackSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " BaseAttackSpeed",
                    0f,
                    survivor.CommonName + ": The base attack speed of your survivor"),
                "baseAttackSpeed");

            BaseCrit = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " BaseCrit",
                    0f,
                    survivor.CommonName + ": The base crit chance of your survivor"),
                "baseCrit");

            BaseArmor = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " BaseArmor",
                    0f,
                    survivor.CommonName + ": The base armor of your survivor"),
                "baseArmor");

            BaseJumpCount = new FieldConfigWrapper<int>(survivor.BindConfig(
                    survivor.CommonName + " BaseJumpCount",
                    0,
                    survivor.CommonName + ": The base jump count of your survivor"),
                "baseJumpCount");

            LevelMaxHealth = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " LevelMaxHealth",
                    0f,
                    survivor.CommonName + ": The max health per level your survivor gets."),
                "levelMaxHealth");

            LevelRegen = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " LevelRegen",
                    0f,
                    survivor.CommonName + ": The regen per level your survivor gets."),
                "levelRegen");

            LevelMaxShield = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " LevelMaxShield",
                    0f,
                    survivor.CommonName + ": The max shield per level your survivor gets"),
                "levelMaxShield");

            LevelMoveSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " LevelMoveSpeed",
                    0f,
                    survivor.CommonName + ": The move speed per level your survivor gets"),
                "levelMoveSpeed");

            LevelJumpPower = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " LevelJumpPower",
                    0f,
                    survivor.CommonName + ": The jump power per level your survivor gets"),
                "levelJumpPower");

            LevelDamage = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " LevelDamage",
                    0f,
                    survivor.CommonName + ": The damage per level your survivor gets"),
                "levelDamage");

            LevelAttackSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " LevelAttackSpeed",
                    0f,
                    survivor.CommonName + ": The attack speed per level your survivor gets"),
                "levelAttackSpeed");

            LevelCrit = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " LevelCrit",
                    0f,
                    survivor.CommonName + ": The crit chance per level your survivor gets"),
                "levelCrit");

            LevelArmor = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivor.CommonName + " LevelArmor",
                    0f,
                    survivor.CommonName + ": The armor per level your survivor gets"),
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