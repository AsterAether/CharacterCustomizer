using System.Collections.Generic;
using BepInEx.Configuration;
using BepInEx.Logging;
using CharacterCustomizer.Util.Config;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomHuntress : CustomSurvivor
    {
        public CustomHuntress(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(
            SurvivorIndex.Huntress, "Huntress", "HUNTRESS",
            updateVanilla, file, logger)
        {
            AddPrimarySkill("Strafe");
            AddSecondarySkill("LaserGlaive");
            AddUtilitySkill("Blink");
            AddUtilitySkill("PhaseBlink", "ALT1");
            AddSpecialSkill("ArrowRain");
            AddSpecialSkill("Ballista", "ALT1");
        }
    }
}