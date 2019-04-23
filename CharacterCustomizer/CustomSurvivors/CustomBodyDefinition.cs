using System.Collections.Generic;
using AetherLib.Util.Config;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomBodyDefinition
    {
        public string SurvivorName { get; }
        public FieldConfigWrapper<string> BaseMaxHealth { get; }
        public FieldConfigWrapper<string> BaseRegen { get; }
        public FieldConfigWrapper<string> BaseMaxShield { get; }
        public FieldConfigWrapper<string> BaseMoveSpeed { get; }
        public FieldConfigWrapper<string> BaseAcceleration { get; }
        public FieldConfigWrapper<string> BaseJumpPower { get; }
        public FieldConfigWrapper<string> BaseDamage { get; }
        public FieldConfigWrapper<string> BaseAttackSpeed { get; }
        public FieldConfigWrapper<string> BaseCrit { get; }
        public FieldConfigWrapper<string> BaseArmor { get; }
        public FieldConfigWrapper<int> BaseJumpCount { get; }
        public FieldConfigWrapper<string> LevelMaxHealth { get; }

        public FieldConfigWrapper<string> LevelRegen { get; }
        public FieldConfigWrapper<string> LevelMaxShield { get; }
        public FieldConfigWrapper<string> LevelMoveSpeed { get; }
        public FieldConfigWrapper<string> LevelJumpPower { get; }
        public FieldConfigWrapper<string> LevelDamage { get; }
        public FieldConfigWrapper<string> LevelAttackSpeed { get; }
        public FieldConfigWrapper<string> LevelCrit { get; }
        public FieldConfigWrapper<string> LevelArmor { get; }

        public List<IFieldChanger> AllFields { get; }

        public CustomBodyDefinition(CustomSurvivor survivor, string survivorName)
        {
            SurvivorName = survivorName;

            BaseMaxHealth = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "BaseMaxHealth",
                    survivorName + ": The base health of your survivor"),
                "baseMaxHealth");

            BaseRegen = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "BaseRegen",
                    survivorName + ": The base regen of your survivor"),
                "baseRegen");

            BaseMaxShield = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "BaseMaxShield",
                    survivorName + ": the base max shield of you survivor"),
                "baseMaxShield");

            BaseMoveSpeed = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "BaseMoveSpeed",
                    survivorName + ": The base move speed of your survivor"),
                "baseMoveSpeed");

            BaseAcceleration = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "BaseAcceleration",
                    survivorName + ": The base acceleration of your survivor"),
                "baseAcceleration");

            BaseJumpPower = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "BaseJumpPower",
                    survivorName + ": The base jump power of your survivor"),
                "baseJumpPower");

            BaseDamage = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "BaseDamage",
                    survivorName + ": The base damage of your survivor"),
                "baseDamage");

            BaseAttackSpeed = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "BaseAttackSpeed",
                    survivorName + ": The base attack speed of your survivor"),
                "baseAttackSpeed");

            BaseCrit = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "BaseCrit",
                    survivorName + ": The base crit chance of your survivor"),
                "baseCrit");

            BaseArmor = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "BaseArmor",
                    survivorName + ": The base armor of your survivor"),
                "baseArmor");

            BaseJumpCount = new FieldConfigWrapper<int>(survivor.WrapConfigInt(
                    survivorName + "BaseJumpCount",
                    survivorName + ": The base jump count of your survivor"),
                "baseJumpCount");

            LevelMaxHealth = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "LevelMaxHealth",
                    survivorName + ": The max health per level your survivor gets."),
                "levelMaxHealth");

            LevelRegen = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "LevelRegen",
                    survivorName + ": The regen per level your survivor gets."),
                "levelRegen");

            LevelMaxShield = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "LevelMaxShield",
                    survivorName + ": The max shield per level your survivor gets"),
                "levelMaxShield");

            LevelMoveSpeed = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "LevelMoveSpeed",
                    survivorName + ": The move speed per level your survivor gets"),
                "levelMoveSpeed");

            LevelJumpPower = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "LevelJumpPower",
                    survivorName + ": The jump power per level your survivor gets"),
                "levelJumpPower");

            LevelDamage = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "LevelDamage",
                    survivorName + ": The damage per level your survivor gets"),
                "levelDamage");

            LevelAttackSpeed = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "LevelAttackSpeed",
                    survivorName + ": The attack speed per level your survivor gets"),
                "levelAttackSpeed");

            LevelCrit = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "LevelCrit",
                    survivorName + ": The crit chance per level your survivor gets"),
                "levelCrit");

            LevelArmor = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    survivorName + "LevelArmor",
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