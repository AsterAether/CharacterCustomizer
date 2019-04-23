using System;
using System.Collections.Generic;
using System.Reflection;
using AetherLib.Util;
using AetherLib.Util.Config;
using BepInEx;
using BepInEx.Configuration;
using EntityStates;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors
{
    namespace MulT
    {
        public class CustomMulT : CustomSurvivor
        {
            public FieldConfigWrapper<string> NailgunSpreadYaw;

            public FieldConfigWrapper<string> NailgunSpreadPitch;

            public List<IFieldChanger> NailgunFields;

            public CustomMulT() : base(SurvivorIndex.Toolbot, "MulT",
                "FireNailgun",
                "StunDrone",
                "ToolbotDash",
                "Swap")
            {
                ExtraSkillNames.Add("FireSpear");
            }

            public override void InitConfigValues()
            {
                NailgunSpreadYaw =
                    new FieldConfigWrapper<string>(
                        WrapConfigFloat("NailgunSpreadYaw", "Yaw spread of the nailgun, in percent"), "spreadYawScale",
                        true);

                NailgunSpreadPitch =
                    new FieldConfigWrapper<string>(
                        WrapConfigFloat("NailgunSpreadPitch", "Pitch spread of the nailgun, in percent"),
                        "spreadPitchScale", true);

                NailgunFields = new List<IFieldChanger>
                {
                    NailgunSpreadYaw, NailgunSpreadPitch
                };
            }

            public override void OverrideGameValues()
            {
                On.RoR2.Run.Awake += (orig, self) =>
                {
                    orig(self);

                    Type fireNailgun = typeof(FireNailgun);

                    NailgunFields.ForEach(changer => changer.Apply(fireNailgun));
                };
            }

            public override void WriteNewHooks()
            {
            }
        }
    }
}