using RoR2;

namespace CharacterCustomizer.CustomSurvivors
{
    public class CustomBandit : CustomSurvivor
    {
        public CustomBandit() : base(SurvivorIndex.Bandit,"Bandit", 
            "FireShotgun", 
            "LightsOut", 
            "Cloak", 
            "Grenade")
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