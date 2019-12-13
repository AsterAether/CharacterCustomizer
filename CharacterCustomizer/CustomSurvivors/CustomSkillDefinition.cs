using System.Collections.Generic;
using AetherLib.Util.Config;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomSkillDefinition
    {
        public string SkillName { get; }

        public string ReplaceOld { get; set; }

        public FieldConfigWrapper<float> BaseRechargeInterval { get; }
        public FieldConfigWrapper<int> BaseMaxStock { get; }
        public FieldConfigWrapper<float> ShootDelay { get; }
        public FieldConfigWrapper<int> RechargeStock { get; }
        public FieldConfigWrapper<bool> IsCombatSkill { get; }
        public FieldConfigWrapper<bool> NoSprint { get; }
        public FieldConfigWrapper<int> RequiredStock { get; }
        public FieldConfigWrapper<int> StockToConsume { get; }
        public FieldConfigWrapper<bool> MustKeyPress { get; }
        public FieldConfigWrapper<bool> BeginSkillCooldownOnSkillEnd { get; }
        public FieldConfigWrapper<bool> CanceledFromSprinting { get; }
        public FieldConfigWrapper<bool> IsBullets { get; }

        public List<IFieldChanger> AllFields { get; }

        public CustomSkillDefinition(CustomSurvivor survivor, string skillName, string replaceOld = null)
        {
            SkillName = skillName;
            ReplaceOld = replaceOld;

            BaseRechargeInterval = new FieldConfigWrapper<float>(survivor.BindConfig(
                    skillName + "BaseRechargeInterval",
                    0f,
                    skillName + ": How long it takes for this skill to recharge after being used."),
                "baseRechargeInterval");

            ShootDelay = new FieldConfigWrapper<float>(survivor.BindConfig(
                    skillName + "ShootDelay",
                    0f,
                    skillName + ": Time between bullets for bullet-style weapons"),
                "shootDelay");

            BaseMaxStock = new FieldConfigWrapper<int>(survivor.BindConfig(
                    skillName + "BaseMaxStock",
                    0,
                    skillName + ": Maximum number of charges this skill can carry."),
                "baseMaxStock");

            RechargeStock = new FieldConfigWrapper<int>(survivor.BindConfig(
                    skillName + "RechargeStock",
                    0,
                    skillName + ": How much stock to restore on a recharge."),
                "rechargeStock");

            IsCombatSkill = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    skillName + "IsCombatSkill",
                    false,
                    skillName + ": Whether or not this is considered a combat skill."),
                "isCombatSkill");

            NoSprint = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    skillName + "NoSprint",
                    false,
                    skillName + ": Whether or not the usage of this skill is mutually exclusive with sprinting."),
                "noSprint");

            RequiredStock = new FieldConfigWrapper<int>(survivor.BindConfig(
                    skillName + "RequiredStock",
                    0,
                    skillName + ": How much stock is required to activate this skill."),
                "requiredStock");

            StockToConsume = new FieldConfigWrapper<int>(survivor.BindConfig(
                    skillName + "StockToConsume",
                    0,
                    skillName + ": How much stock to deduct when the skill is activated."),
                "stockToConsume");

            MustKeyPress = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    skillName + "MustKeyPress",
                    false,
                    skillName + ": The skill can't be activated if the key is held."),
                "mustKeyPress");

            BeginSkillCooldownOnSkillEnd = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    skillName + "BeginSkillCooldownOnSkillEnd",
                    false,
                    skillName + ": Whether or not the cooldown waits until it leaves the set state"),
                "beginSkillCooldownOnSkillEnd");

            CanceledFromSprinting = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    skillName + "CanceledFromSprinting",
                    false,
                    skillName + ": Sprinting will actively cancel this ability."),
                "canceledFromSprinting");

            IsBullets = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    skillName + "IsBullets",
                    false,
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