using System;
using System.Collections.Generic;
using AetherLib.Util;
using AetherLib.Util.Config;
using BepInEx;
using BepInEx.Configuration;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors
{
    namespace Mercenary
    {
        public class CustomMercenary : CustomSurvivor
        {
            public FieldConfigWrapper<int> DashMaxCount;

            public FieldConfigWrapper<string> DashTimeoutDuration;

            public List<IFieldChanger> DashFields;

            public override void InitConfigValues()
            {
                DashMaxCount = new FieldConfigWrapper<int>(WrapConfigInt("DashMaxCount",
                    "Maximum amount of dashes Mercenary can perform."), "maxDashes");

                DashTimeoutDuration = new FieldConfigWrapper<string>(WrapConfigFloat("DashTimeoutDuration",
                    "Maximum timeout between dashes, in seconds"), "timeoutDuration");

                DashFields = new List<IFieldChanger>
                {
                    DashMaxCount, DashTimeoutDuration
                };
            }

            public CustomMercenary() : base(SurvivorIndex.Merc, "Mercenary",
                "GroundLight",
                "Whirlwind",
                "Dash",
                "Evis")
            {
            }


            public override void OverrideGameValues()
            {
                On.RoR2.MercDashSkill.OnExecute += (orig, self) =>
                {
                    DashFields.ForEach(changer => changer.Apply(self));

                    orig(self);
                };
            }

            public override void WriteNewHooks()
            {
            }
        }
    }
}