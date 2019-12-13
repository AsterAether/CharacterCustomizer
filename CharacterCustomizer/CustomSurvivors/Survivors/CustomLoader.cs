using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomLoader : CustomSurvivor
    {
        public CustomLoader(bool updateVanilla) : base(SurvivorIndex.Loader, "Loader",
            "SwingFist",
            "FireHook",
            "ChargeFist",
            "ThrowPylon", updateVanilla)
        {     
            SetPrimarySkillReplaceOldName("GroundLight");
            SetSecondarySkillReplaceOldName("Whirlwind");
            SetUtilitySkillReplaceOldName("Dash");
            SetSpecialSkillReplaceOldName("Evis");
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