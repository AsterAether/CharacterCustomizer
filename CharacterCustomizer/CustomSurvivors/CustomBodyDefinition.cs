using System.Collections.Generic;
using AetherLib.Util.Config;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomBodyDefinition
    {
        public string SurvivorName { get; }
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

        public CustomBodyDefinition(CustomSurvivor survivor, string survivorName)
        {
            SurvivorName = survivorName;

            BaseMaxHealth = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "BaseMaxHealth",
                    0f,
                    survivorName + ": The base health of your survivor"),
                "baseMaxHealth");

            BaseRegen = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "BaseRegen",
                    0f,
                    survivorName + ": The base regen of your survivor"),
                "baseRegen");

            BaseMaxShield = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "BaseMaxShield",
                    0f,
                    survivorName + ": the base max shield of you survivor"),
                "baseMaxShield");

            BaseMoveSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "BaseMoveSpeed",
                    0f,
                    survivorName + ": The base move speed of your survivor"),
                "baseMoveSpeed");

            BaseAcceleration = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "BaseAcceleration",
                    0f,
                    survivorName + ": The base acceleration of your survivor"),
                "baseAcceleration");

            BaseJumpPower = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "BaseJumpPower",
                    0f,
                    survivorName + ": The base jump power of your survivor"),
                "baseJumpPower");

            BaseDamage = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "BaseDamage",
                    0f,
                    survivorName + ": The base damage of your survivor"),
                "baseDamage");

            BaseAttackSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "BaseAttackSpeed",
                    0f,
                    survivorName + ": The base attack speed of your survivor"),
                "baseAttackSpeed");

            BaseCrit = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "BaseCrit",
                    0f,
                    survivorName + ": The base crit chance of your survivor"),
                "baseCrit");

            BaseArmor = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "BaseArmor",
                    0f,
                    survivorName + ": The base armor of your survivor"),
                "baseArmor");

            BaseJumpCount = new FieldConfigWrapper<int>(survivor.BindConfig(
                    survivorName + "BaseJumpCount",
                    0,
                    survivorName + ": The base jump count of your survivor"),
                "baseJumpCount");

            LevelMaxHealth = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "LevelMaxHealth",
                    0f,
                    survivorName + ": The max health per level your survivor gets."),
                "levelMaxHealth");

            LevelRegen = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "LevelRegen",
                    0f,
                    survivorName + ": The regen per level your survivor gets."),
                "levelRegen");

            LevelMaxShield = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "LevelMaxShield",
                    0f,
                    survivorName + ": The max shield per level your survivor gets"),
                "levelMaxShield");

            LevelMoveSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "LevelMoveSpeed",
                    0f,
                    survivorName + ": The move speed per level your survivor gets"),
                "levelMoveSpeed");

            LevelJumpPower = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "LevelJumpPower",
                    0f,
                    survivorName + ": The jump power per level your survivor gets"),
                "levelJumpPower");

            LevelDamage = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "LevelDamage",
                    0f,
                    survivorName + ": The damage per level your survivor gets"),
                "levelDamage");

            LevelAttackSpeed = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "LevelAttackSpeed",
                    0f,
                    survivorName + ": The attack speed per level your survivor gets"),
                "levelAttackSpeed");

            LevelCrit = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "LevelCrit",
                    0f,
                    survivorName + ": The crit chance per level your survivor gets"),
                "levelCrit");

            LevelArmor = new FieldConfigWrapper<float>(survivor.BindConfig(
                    survivorName + "LevelArmor",
                    0f,
                    survivorName + ": The armor per level your survivor gets"),
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
        }
    }
}