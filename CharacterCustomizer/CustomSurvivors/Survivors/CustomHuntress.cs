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
            AddSkill("Strafe", 100);
            AddSkill("Volley", 97);
            AddSkill("LaserGlaive", 101);
            AddSkill("Blink", 99);
            AddSkill("PhaseBlink", 102);
            AddSkill("ArrowRain", 98);
            AddSkill("Ballista", 95);
            AddSkill("FireBallistaShot", 96);
        }
    }
}