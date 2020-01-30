using System.Collections.Generic;
using BepInEx.Configuration;
using BepInEx.Logging;
using CharacterCustomizer.Util.Config;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    namespace Mercenary
    {
        public class CustomMercenary : CustomSurvivor
        {
           
            public CustomMercenary(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(SurvivorIndex.Merc, "Mercenary", "MERC",
                updateVanilla, file, logger)
            {
                AddPrimarySkill("LaserSword");
                AddSecondarySkill("Whirlwind");
                AddSecondarySkill("RisingThunder", "ALT1");
                AddUtilitySkill("BlindingAssault");
                AddSpecialSkill("Eviscerate");
                AddSpecialSkill("SlicingWinds", "ALT1");
            }
            
        }
    }
}