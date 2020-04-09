using System;
using System.Collections.Generic;
using System.Reflection;
using R2API.Utils;
using BepInEx.Configuration;
using BepInEx.Logging;
using CharacterCustomizer.Util.Config;
using EntityStates;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomCommando : CustomSurvivor
    {
        public CustomCommando(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(
            SurvivorIndex.Commando, "Commando", "COMMANDO",
            updateVanilla, file, logger)
        {
            AddSkill("DoubleTap", 38);
            AddSkill("PhaseRound", 37);
            AddSkill("PhaseBlast", 39);
            AddSkill("TacticalDive", 40);
            AddSkill("TacticalSlide", 42);
            AddSkill("SuppressiveFire", 36);
            AddSkill("FragGrenade", 43);
        }
    }
}