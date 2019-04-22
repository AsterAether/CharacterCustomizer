using System;
using System.Reflection;
using AetherLib.Util;
using AetherLib.Util.Config;
using BepInEx;
using BepInEx.Configuration;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors
{
    namespace MultT
    {
        public class CustomMultT : CustomSurvivor
        {
            public ValueConfigWrapper<string> NailgunSpreadYaw;

            public ValueConfigWrapper<string> NailgunSpreadPitch;

            public CustomMultT() : base(SurvivorIndex.Toolbot, "MultT",
                "FireNailgun",
                "StunDrone",
                "ToolbotDash",
                "Swap")
            {
                ExtraSkillNames.Add("FireSpear");
            }

            public override void InitConfigValues()
            {
                NailgunSpreadYaw = WrapConfigFloat("NailgunSpreadYaw", "Yaw spread of the nailgun, in percent");

                NailgunSpreadPitch = WrapConfigFloat("NailgunSpreadPitch", "Pitch spread of the nailgun, in percent");
            }

            public override void OverrideGameValues()
            {
                On.RoR2.Run.Awake += (orig, self) =>
                {
                    orig(self);

                    NailgunSpreadPitch.SetDefaultValue(EntityStates.FireNailgun.spreadPitchScale);
                    if (NailgunSpreadPitch.IsNotDefault())
                    {
                        EntityStates.FireNailgun.spreadPitchScale = NailgunSpreadYaw.FloatValue;
                    }

                    NailgunSpreadYaw.SetDefaultValue(EntityStates.FireNailgun.spreadYawScale);
                    if (NailgunSpreadYaw.IsNotDefault())
                    {
                        EntityStates.FireNailgun.spreadYawScale = NailgunSpreadYaw.FloatValue;
                    }
                };
            }

            public override void WriteNewHooks()
            {
            }
        }
    }
}