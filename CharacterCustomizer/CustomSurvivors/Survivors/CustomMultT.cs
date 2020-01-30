using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using BepInEx.Logging;
using CharacterCustomizer.Util.Config;
using EntityStates;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomMulT : CustomSurvivor
    {
        public CustomMulT(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(SurvivorIndex.Toolbot,
            "MultT", "TOOLBOT",
            updateVanilla, file, logger)
        {
            AddPrimarySkill("AutoNailgun");
            AddPrimarySkill("RebarPuncher", "ALT1");
            AddPrimarySkill("ScrapLauncher", "ALT2");
            AddPrimarySkill("PowerSaw", "ALT3");
            AddSecondarySkill("BlastCanister");
            AddUtilitySkill("TransportMode");
            AddSpecialSkill("Retool");
        }
    }
}