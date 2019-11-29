using System.Collections.Generic;
using AetherLib.Util.Config;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    namespace Mercenary
    {
        public class CustomMercenary : CustomSurvivor
        {
            public FieldConfigWrapper<int> DashMaxCount;

            public FieldConfigWrapper<float> DashTimeoutDuration;

            public List<IFieldChanger> DashFields;

            public override void InitConfigValues()
            {
//                DashMaxCount = new FieldConfigWrapper<int>(BindConfigInt("DashMaxCount",
//                    "Maximum amount of dashes Mercenary can perform."), "maxDashes");
//
//                DashTimeoutDuration = new FieldConfigWrapper<float>(BindConfigFloat("DashTimeoutDuration",
//                    "Maximum timeout between dashes, in seconds"), "timeoutDuration");
//
//                DashFields = new List<IFieldChanger>
//                {
//                    DashMaxCount, DashTimeoutDuration
//                };
            }

            public CustomMercenary(bool updateVanilla) : base(SurvivorIndex.Merc, "Mercenary",
                "GroundLight",
                "Whirlwind",
                "Dash",
                "Evis", updateVanilla)
            {
            }


            public override void OverrideGameValues()
            {
//                On.RoR2.MercDashSkill.OnExecute += (orig, self) =>
//                {
//                    DashFields.ForEach(changer => changer.Apply(self));
//
//                    orig(self);
//                };
            }

            public override void WriteNewHooks()
            {
            }
        }
    }
}