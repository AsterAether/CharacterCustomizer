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
    public class CustomEngineer : CustomSurvivor
    {
        public CustomEngineer(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(
            RoR2.SurvivorIndex.Engi, "Engineer", "ENGI",
            updateVanilla, file, logger)
        {
            AddSkill("BouncingGrenades", 66);
            AddSkill("PressureMine", 68);
            AddSkill("SpiderMine", 69);
            AddSkill("BubbleShield", 67);
            AddSkill("ThermalHarpoons", 74);
            AddSkill("ThermalHarpoonTargeting", 73);
            AddSkill("GaussTurret", 70);
            AddSkill("CarbonizerTurret", 71);
        }
    }
}