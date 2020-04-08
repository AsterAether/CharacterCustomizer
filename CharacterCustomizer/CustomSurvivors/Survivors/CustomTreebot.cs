using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomTreebot : CustomSurvivor
    {
        public CustomTreebot(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(SurvivorIndex.Treebot,
            "REX", "TREEBOT",
            updateVanilla, file, logger)
        {
            AddSkill("Inject", 183);
            AddSkill("Drill", 181);
            AddSkill("SeedBarrage", 180);
            AddSkill("Disperse", 185);
            AddSkill("BrambleVolley", 184);
            AddSkill("TanglingGrowth", 182);
        }
    }
}