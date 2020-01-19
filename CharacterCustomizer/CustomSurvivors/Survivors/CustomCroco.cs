using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomCroco : CustomSurvivor
    {
        public CustomCroco(bool updateVanilla) : base(SurvivorIndex.Croco, "Croco",
            "CROCO_PRIMARY_NAME",
            "Slash",
            "CROCO_SECONDARY_NAME",
            "Neurotoxin",
            "CROCO_UTILITY_NAME",
            "CausticLeap",
            "CROCO_SPECIAL_NAME",
            "Epidemic",
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