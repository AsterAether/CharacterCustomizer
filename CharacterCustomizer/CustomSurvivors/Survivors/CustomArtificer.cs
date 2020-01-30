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
    namespace Artificer
    {
        public class CustomArtificer : CustomSurvivor
        {
            public CustomArtificer(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(SurvivorIndex.Mage, "Artificer","MAGE", updateVanilla, file, logger)
            {
                AddPrimarySkill("FlameBolt", "FIRE");
                AddPrimarySkill("PlasmaBolt", "LIGHTNING");
                
                AddSecondarySkill("NanoBomb", "LIGHTNING");
                AddSecondarySkill("NanoSpear", "ICE");
                
                AddUtilitySkill("Snapfreeze", "ICE");

                AddSpecialSkill("Flamethrower", "FIRE");
                AddSpecialSkill("IonSurge", "LIGHTNING");
            }

        }
    }
}