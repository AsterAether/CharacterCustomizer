using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomLoader : CustomSurvivor
    {
        public CustomLoader(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(SurvivorIndex.Loader,
            "Loader", "LOADER", updateVanilla, file, logger)
        {
            AddSkill("Knuckleboom", 119);
            AddSkill("GrappleFist", 117);
            AddSkill("SpikedFist", 118);
            AddSkill("ChargedGauntlet", 115);
            AddSkill("ThunderGauntlet", 116);
            AddSkill("Pylon", 120);
        }
    }
}