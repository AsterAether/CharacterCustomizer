using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomLoader : CustomSurvivor
    {
        public CustomLoader(bool updateVanilla) : base(SurvivorIndex.Loader, "Loader",
            "LOADER_PRIMARY_NAME",
            "SwingFist",
            "LOADER_SECONDARY_NAME",
            "FireHook",
            "LOADER_UTILITY_NAME",
            "ChargeFist",
            "LOADER_SPECIAL_NAME",
            "ThrowPylon", updateVanilla)
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