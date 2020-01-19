using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomTreebot : CustomSurvivor
    {
        public CustomTreebot(bool updateVanilla) : base(SurvivorIndex.Treebot, "REX",
            "TREEBOT_PRIMARY_NAME",
            "FireSyringe",
            "TREEBOT_SECONDARY_NAME",
            "AimMortar2",
            "TREEBOT_UTILITY_NAME",
            "SonicBoom",
            "TREEBOT_SPECIAL_NAME",
            "FireFlower2", updateVanilla)
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