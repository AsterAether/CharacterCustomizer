using System.Collections.Generic;
using BepInEx.Configuration;
using BepInEx.Logging;
using CharacterCustomizer.Util.Config;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomMercenary : CustomSurvivor
    {
        public CustomMercenary(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(SurvivorIndex.Merc,
            "Mercenary", "MERC",
            updateVanilla, file, logger)
        {
            AddSkill("LaserSword", 138);
            AddSkill("Whirlwind", 140);
            AddSkill("RisingThunder", 139);
            AddSkill("BlindingAssault", 135);
            AddSkill("Eviscerate", 136);
            AddSkill("SlicingWinds", 137);
        }
    }
}