using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomLoader : CustomSurvivor
    {
        public CustomLoader(bool updateVanilla) : base(SurvivorIndex.Loader, "Loader",
            "GroundLight",
            "Whirlwind",
            "Dash",
            "Evis", updateVanilla)
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