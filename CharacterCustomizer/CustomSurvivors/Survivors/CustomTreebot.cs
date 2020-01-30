using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomTreebot : CustomSurvivor
    {
        public CustomTreebot(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(SurvivorIndex.Treebot, "REX", "TREEBOT",
             updateVanilla, file, logger)
        {
            AddPrimarySkill("Inject");
            AddSecondarySkill("Drill");
            AddSecondarySkill("SeedBarrage", "ALT1");
            AddUtilitySkill("Disperse");
            AddUtilitySkill("BrambleVolley", "ALT1");
            AddSpecialSkill("TanglingGrowth");
        }
    }
}