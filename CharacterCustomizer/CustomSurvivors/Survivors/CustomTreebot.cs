using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomTreebot : CustomSurvivor
    {
        public CustomTreebot(bool updateVanilla) : base(SurvivorIndex.Treebot, "REX",
            "FireSyringe",
            "AimMortar2",
            "SonicBoom",
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