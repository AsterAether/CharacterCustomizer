using AetherLib.Util.Config;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomSkillDefinition
    {
        public string SkillName { get; }

        public ValueConfigWrapper<string> BaseRechargeInterval { get; }
        public ValueConfigWrapper<int> BaseMaxStock { get; }
        public ValueConfigWrapper<string> ShootDelay { get; }
        public ValueConfigWrapper<int> RechargeStock { get; }
        public ValueConfigWrapper<string> IsCombatSkill { get; }
        public ValueConfigWrapper<string> NoSprint { get; }
        public ValueConfigWrapper<int> RequiredStock { get; }
        public ValueConfigWrapper<int> StockToConsume { get; }
        public ValueConfigWrapper<string> MustKeyPress { get; }
        public ValueConfigWrapper<string> BeginSkillCooldownOnSkillEnd { get; }
        public ValueConfigWrapper<string> CanceledFromSprinting { get; }
        public ValueConfigWrapper<string> IsBullets { get; }

        public CustomSkillDefinition(CustomSurvivor survivor, string skillName)
        {
            SkillName = skillName;

            BaseRechargeInterval = survivor.WrapConfigFloat(skillName + "BaseRechargeInterval",
                skillName + ": How long it takes for this skill to recharge after being used.");
            ShootDelay = survivor.WrapConfigFloat(skillName + "ShootDelay",
                skillName + ": Time between bullets for bullet-style weapons");
            BaseMaxStock = survivor.WrapConfigInt(skillName + "BaseMaxStock",
                skillName + ": Maximum number of charges this skill can carry.");
            RechargeStock = survivor.WrapConfigInt(skillName + "RechargeStock",
                skillName + ": How much stock to restore on a recharge.");
            IsCombatSkill = survivor.WrapConfigString(skillName + "IsCombatSkill",
                skillName + ": Whether or not this is considered a combat skill.");
            NoSprint = survivor.WrapConfigString(skillName + "NoSprint",
                skillName +": Whether or not the usage of this skill is mutually exclusive with sprinting.");
            RequiredStock = survivor.WrapConfigInt(skillName + "RequiredStock",
                skillName + ": How much stock is required to activate this skill.");
            StockToConsume = survivor.WrapConfigInt(skillName + "StockToConsume",
                skillName + ": How much stock to deduct when the skill is activated.");
            MustKeyPress = survivor.WrapConfigString(skillName + "MustKeyPress",
                skillName + ": The skill can't be activated if the key is held.");
            BeginSkillCooldownOnSkillEnd = survivor.WrapConfigString(skillName + "BeginSkillCooldownOnSkillEnd",
                skillName + ": Whether or not the cooldown waits until it leaves the set state");
            CanceledFromSprinting = survivor.WrapConfigString(skillName + "CanceledFromSprinting",
                skillName + ": Sprinting will actively cancel this ability.");
            IsBullets = survivor.WrapConfigString(skillName + "IsBullets",
                skillName + ": Whether or not it has bullet reload behavior");
        }
    }
}