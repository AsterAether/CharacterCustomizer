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
            AddPrimarySkill("Knuckleboom");
            AddSecondarySkill("GrappleFist");
            AddSecondarySkill("SpikedFist", "ALT1");
            AddUtilitySkill("ChargedGauntlet");
            AddUtilitySkill("ThunderGauntlet", "ALT1");
            AddSpecialSkill("Pylon");
        }
    }
}