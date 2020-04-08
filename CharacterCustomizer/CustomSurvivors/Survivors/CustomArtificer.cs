using System;
using System.Collections.Generic;
using System.Reflection;
using R2API.Utils;
using BepInEx.Configuration;
using BepInEx.Logging;
using EntityStates;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using CharacterCustomizer.Util.Config;
using UnityEngine;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomArtificer : CustomSurvivor
    {
        public CustomArtificer(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(SurvivorIndex.Mage,
            "Artificer", "MAGE", updateVanilla, file, logger)
        {
            AddSkill("FlameBolt", 123);
            AddSkill("PlasmaBolt", 125);

            AddSkill("NanoBomb", 129);
            AddSkill("NanoSpear", 128);

            AddSkill("Snapfreeze", 130);

            AddSkill("Flamethrower", 126);
            AddSkill("IonSurge", 127);
        }
    }
}