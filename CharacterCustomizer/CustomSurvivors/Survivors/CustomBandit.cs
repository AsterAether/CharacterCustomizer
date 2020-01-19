using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomBandit : CustomSurvivor
    {
        public CustomBandit(bool updateVanilla) : base(SurvivorIndex.Bandit,
            "Bandit",
            "BANDIT_PRIMARY_FIRE_NAME",
            "FireShotgun",
            "BANDIT_SECONDARY_LIGHTNING_NAME",
            "LightsOut",
            "BANDIT_UTILITY_ICE_NAME",
            "Cloak",
            "BANDIT_SPECIAL_FIRE_NAME",
            "Grenade",
            updateVanilla)
        {
        }

        public override void InitConfigValues()
        {
        }

        public override void OverrideGameValues()
        {
        }

        public override void WriteNewHooks()
        {
        }
    }
}