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
            AddSkill("AutoNailgun", 175);
            AddSkill("RebarPuncher", 176);
            AddSkill("ScrapLauncher", 174);
            AddSkill("PowerSaw", 173);
            AddSkill("BlastCanister", 177);
            AddSkill("TransportMode", 179);
            AddSkill("Retool", 178);
        }
    }
}