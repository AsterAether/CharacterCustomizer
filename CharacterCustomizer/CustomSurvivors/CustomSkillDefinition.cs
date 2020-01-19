using System.Collections.Generic;
using AetherLib.Util.Config;
using BepInEx.Configuration;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomSkillDefinition
    {
        public string SkillNameToken { get; }

        public string CommonName { get; set; }

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

        public delegate void CustomFieldChanged(CustomSkillDefinition skillDefinition, IFieldChanger changed);

        public CustomFieldChanged OnFieldChanged;


        public CustomSkillDefinition(CustomSurvivor survivor, string skillNameToken, string commonName = null)
        {
            SkillNameToken = skillNameToken;
            CommonName = commonName;

            BaseRechargeInterval = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "BaseRechargeInterval",
                    0f,
                    commonName + ": How long it takes for this skill to recharge after being used."),
                "baseRechargeInterval");

            ShootDelay = new FieldConfigWrapper<float>(survivor.BindConfig(
                    commonName + "ShootDelay",
                    0f,
                    commonName + ": Time between bullets for bullet-style weapons"),
                "shootDelay");

            BaseMaxStock = new FieldConfigWrapper<int>(survivor.BindConfig(
                    commonName + "BaseMaxStock",
                    0,
                    commonName + ": Maximum number of charges this skill can carry."),
                "baseMaxStock");

            RechargeStock = new FieldConfigWrapper<int>(survivor.BindConfig(
                    commonName + "RechargeStock",
                    0,
                    commonName + ": How much stock to restore on a recharge."),
                "rechargeStock");

            IsCombatSkill = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    commonName + "IsCombatSkill",
                    false,
                    commonName + ": Whether or not this is considered a combat skill."),
                "isCombatSkill");

            NoSprint = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    commonName + "NoSprint",
                    false,
                    commonName + ": Whether or not the usage of this skill is mutually exclusive with sprinting."),
                "noSprint");

            RequiredStock = new FieldConfigWrapper<int>(survivor.BindConfig(
                    commonName + "RequiredStock",
                    0,
                    commonName + ": How much stock is required to activate this skill."),
                "requiredStock");

            StockToConsume = new FieldConfigWrapper<int>(survivor.BindConfig(
                    commonName + "StockToConsume",
                    0,
                    commonName + ": How much stock to deduct when the skill is activated."),
                "stockToConsume");

            MustKeyPress = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    commonName + "MustKeyPress",
                    false,
                    commonName + ": The skill can't be activated if the key is held."),
                "mustKeyPress");

            BeginSkillCooldownOnSkillEnd = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    commonName + "BeginSkillCooldownOnSkillEnd",
                    false,
                    commonName + ": Whether or not the cooldown waits until it leaves the set state"),
                "beginSkillCooldownOnSkillEnd");

            CanceledFromSprinting = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    commonName + "CanceledFromSprinting",
                    false,
                    commonName + ": Sprinting will actively cancel this ability."),
                "canceledFromSprinting");

            IsBullets = new FieldConfigWrapper<bool>(survivor.BindConfig(
                    commonName + "IsBullets",
                    false,
                    commonName + ": Whether or not it has bullet reload behavior"),
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