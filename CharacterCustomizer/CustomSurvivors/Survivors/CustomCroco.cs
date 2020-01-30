using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomCroco : CustomSurvivor
    {
        public CustomCroco(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(SurvivorIndex.Croco, "Acrid", "CROCO", updateVanilla, file, logger)
        {
            AddPrimarySkill("ViciousWounds");
            AddSecondarySkill("Neurotoxin");
            AddUtilitySkill("CausticLeap");
            AddUtilitySkill("FrenziedLeap", "ALT1");
            
            AddSpecialSkill("Epidemic");
        }
    }
}