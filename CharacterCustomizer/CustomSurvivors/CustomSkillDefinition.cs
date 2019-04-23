using System.Collections.Generic;
using AetherLib.Util.Config;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomSkillDefinition
    {
        public string SkillName { get; }

        public FieldConfigWrapper<string> BaseRechargeInterval { get; }
        public FieldConfigWrapper<int> BaseMaxStock { get; }
        public FieldConfigWrapper<string> ShootDelay { get; }
        public FieldConfigWrapper<int> RechargeStock { get; }
        public FieldConfigWrapper<string> IsCombatSkill { get; }
        public FieldConfigWrapper<string> NoSprint { get; }
        public FieldConfigWrapper<int> RequiredStock { get; }
        public FieldConfigWrapper<int> StockToConsume { get; }
        public FieldConfigWrapper<string> MustKeyPress { get; }
        public FieldConfigWrapper<string> BeginSkillCooldownOnSkillEnd { get; }
        public FieldConfigWrapper<string> CanceledFromSprinting { get; }
        public FieldConfigWrapper<string> IsBullets { get; }

        public List<IFieldChanger> AllFields { get; }

        public CustomSkillDefinition(CustomSurvivor survivor, string skillName)
        {
            SkillName = skillName;

            BaseRechargeInterval = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    skillName + "BaseRechargeInterval",
                    skillName + ": How long it takes for this skill to recharge after being used."),
                "baseRechargeInterval");

            ShootDelay = new FieldConfigWrapper<string>(survivor.WrapConfigFloat(
                    skillName + "ShootDelay",
                    skillName + ": Time between bullets for bullet-style weapons"),
                "shootDelay");

            BaseMaxStock = new FieldConfigWrapper<int>(survivor.WrapConfigInt(
                    skillName + "BaseMaxStock",
                    skillName + ": Maximum number of charges this skill can carry."),
                "baseMaxStock");

            RechargeStock = new FieldConfigWrapper<int>(survivor.WrapConfigInt(
                    skillName + "RechargeStock",
                    skillName + ": How much stock to restore on a recharge."),
                "rechargeStock");

            IsCombatSkill = new FieldConfigWrapper<string>(survivor.WrapConfigBool(
                    skillName + "IsCombatSkill",
                    skillName + ": Whether or not this is considered a combat skill."),
                "isCombatSkill");

            NoSprint = new FieldConfigWrapper<string>(survivor.WrapConfigBool(
                    skillName + "NoSprint",
                    skillName + ": Whether or not the usage of this skill is mutually exclusive with sprinting."),
                "noSprint");

            RequiredStock = new FieldConfigWrapper<int>(survivor.WrapConfigInt(
                    skillName + "RequiredStock",
                    skillName + ": How much stock is required to activate this skill."),
                "requiredStock");

            StockToConsume = new FieldConfigWrapper<int>(survivor.WrapConfigInt(
                    skillName + "StockToConsume",
                    skillName + ": How much stock to deduct when the skill is activated."),
                "stockToConsume");

            MustKeyPress = new FieldConfigWrapper<string>(survivor.WrapConfigBool(
                    skillName + "MustKeyPress",
                    skillName + ": The skill can't be activated if the key is held."),
                "mustKeyPress");

            BeginSkillCooldownOnSkillEnd = new FieldConfigWrapper<string>(survivor.WrapConfigBool(
                    skillName + "BeginSkillCooldownOnSkillEnd",
                    skillName + ": Whether or not the cooldown waits until it leaves the set state"),
                "beginSkillCooldownOnSkillEnd");

            CanceledFromSprinting = new FieldConfigWrapper<string>(survivor.WrapConfigBool(
                    skillName + "CanceledFromSprinting",
                    skillName + ": Sprinting will actively cancel this ability."),
                "canceledFromSprinting");

            IsBullets = new FieldConfigWrapper<string>(survivor.WrapConfigBool(
                    skillName + "IsBullets",
                    skillName + ": Whether or not it has bullet reload behavior"),
                "isBullets");


            AllFields = new List<IFieldChanger>
            {
                BaseRechargeInterval,
                BaseMaxStock,
                ShootDelay,
                RechargeStock,
                IsCombatSkill,
                NoSprint,
                RequiredStock,
                StockToConsume,
                MustKeyPress,
                BeginSkillCooldownOnSkillEnd,
                CanceledFromSprinting,
                IsBullets
            };
        }
    }
}