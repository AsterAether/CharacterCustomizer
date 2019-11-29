using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomBandit : CustomSurvivor
    {
        public CustomBandit(bool updateVanilla) : base(SurvivorIndex.Bandit,"Bandit", 
            "FireShotgun", 
            "LightsOut", 
            "Cloak", 
            "Grenade", updateVanilla)
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