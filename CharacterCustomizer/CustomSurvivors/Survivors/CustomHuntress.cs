using System;
using System.Collections.Generic;
using AetherLib.Util;
using AetherLib.Util.Config;
using BepInEx;
using BepInEx.Configuration;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors
{
    namespace Huntress
    {
        public class CustomHuntress : CustomSurvivor
        {
            public FieldConfigWrapper<string> TrackingMaxDistance;

            public FieldConfigWrapper<string> TrackingMaxAngle;

            public List<IFieldChanger> TrackingFields;

            public CustomHuntress() : base(SurvivorIndex.Huntress, "Huntress",
                "FireSeekingArrow",
                "Glaive",
                "Blink",
                "ArrowRain")
            {
            }

            public override void InitConfigValues()
            {
                TrackingMaxDistance = new FieldConfigWrapper<string>(WrapConfigFloat("TrackingMaxDistance",
                    "The maximum distance the tracking of the huntress works."), "maxTrackingDistance");


                TrackingMaxAngle = new FieldConfigWrapper<string>(WrapConfigFloat("TrackingMaxAngle",
                    "The maximum angle the tracking of the huntress works."), "maxTrackingAngle");

                TrackingFields = new List<IFieldChanger>
                {
                    TrackingMaxAngle, TrackingMaxDistance
                };
            }

            public override void OverrideGameValues()
            {
                On.RoR2.HuntressTracker.Awake += (orig, self) =>
                {
                    orig(self);

                    TrackingFields.ForEach(changer => changer.Apply(self));
                };
            }

            public override void WriteNewHooks()
            {
            }
        }
    }
}