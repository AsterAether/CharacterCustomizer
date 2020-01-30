using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using BepInEx.Logging;
using CharacterCustomizer.Util.Config;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    namespace Engineer
    {
        public class CustomEngineer : CustomSurvivor
        {
            public CustomEngineer(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(RoR2.SurvivorIndex.Engi, "Engineer", "ENGI",
                 updateVanilla, file, logger)
            {
                AddPrimarySkill("BouncingGrenades");
                AddSecondarySkill("PressureMine");
                AddSecondarySkill("SpiderMine", "ALT1");
                AddUtilitySkill("BubbleShield");
                AddSpecialSkill("GaussTurrent");
                AddSpecialSkill("CarbonizerTurret", "ALT1");
            }
        }
    }
}