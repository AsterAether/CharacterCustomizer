using System;
using System.Collections.Generic;
using AetherLib.Util.Config;
using EntityStates;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    namespace MulT
    {
        public class CustomMulT : CustomSurvivor
        {
            public FieldConfigWrapper<float> NailgunSpreadYaw;

            public FieldConfigWrapper<float> NailgunSpreadPitch;

            public List<IFieldChanger> NailgunFields;

            public CustomMulT(bool updateVanilla) : base(SurvivorIndex.Toolbot, "MultT",
                "TOOLBOT_PRIMARY_NAME",
                "FireNailgun",
                "TOOLBOT_SECONDARY_NAME",
                "StunDrone",
                "TOOLBOT_UTILITY_NAME",
                "ToolbotDash",
                "TOOLBOT_SPECIAL_NAME",
                "Swap", updateVanilla)
            {
                ExtraSkillNames.Add("TOOLBOT_PRIMARY_ALT1_NAME");
            }

            public override void InitConfigValues()
            {
                NailgunSpreadYaw =
                    new FieldConfigWrapper<float>(
                        BindConfigFloat("NailgunSpreadYaw", "Yaw spread of the nailgun, in percent"), "spreadYawScale",
                        true);

                NailgunSpreadPitch =
                    new FieldConfigWrapper<float>(
                        BindConfigFloat("NailgunSpreadPitch", "Pitch spread of the nailgun, in percent"),
                        "spreadPitchScale", true);

                NailgunFields = new List<IFieldChanger>
                {
                    NailgunSpreadYaw, NailgunSpreadPitch
                };
            }

            public override void OverrideGameValues()
            {
                On.RoR2.RoR2Application.Start += (orig, self) =>
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