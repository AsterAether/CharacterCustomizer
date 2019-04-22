using System;
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
            public ValueConfigWrapper<int> DashMaxCount;

            public ValueConfigWrapper<string> DashTimeoutDuration;

            public override void InitConfigValues()
            {
                DashMaxCount = WrapConfigInt("DashMaxCount", "Maximum amount of dashes Mercenary can perform.");

                DashTimeoutDuration = WrapConfigFloat("DashTimeoutDuration",
                    "Maximum timeout between dashes, in seconds");
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
                    DashMaxCount.SetDefaultValue(self.maxDashes);
                    DashMaxCount.RunIfNotDefault(count => { self.maxDashes = count; });

                    DashTimeoutDuration.SetDefaultValue(self.timeoutDuration);
                    if (DashTimeoutDuration.IsNotDefault())
                    {
                        self.timeoutDuration = DashTimeoutDuration.FloatValue;
                    }

                    orig(self);
                };
            }

            public override void WriteNewHooks()
            {
            }
        }
    }
}