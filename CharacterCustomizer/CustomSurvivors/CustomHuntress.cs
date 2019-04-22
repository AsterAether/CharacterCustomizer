using System;
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
            public ValueConfigWrapper<string> TrackingMaxDistance;

            public ValueConfigWrapper<string> TrackingMaxAngle;

            public CustomHuntress() : base(SurvivorIndex.Huntress,"Huntress",
                "FireSeekingArrow",
                "Glaive",
                "Blink",
                "ArrowRain")
            {
            }

            public override void InitConfigValues()
            {
                TrackingMaxDistance = WrapConfigFloat("TrackingMaxDistance",
                    "The maximum distance the tracking of the huntress works.");


                TrackingMaxAngle = WrapConfigFloat("TrackingMaxAngle",
                    "The maximum angle the tracking of the huntress works.");
            }

            public override void OverrideGameValues()
            {
                On.RoR2.HuntressTracker.Awake += (orig, self) =>
                {
                    orig(self);

                    TrackingMaxDistance.SetDefaultValue(self.maxTrackingDistance);
                    if (TrackingMaxDistance.IsNotDefault())
                    {
                        self.maxTrackingDistance = TrackingMaxDistance.FloatValue;
                    }

                    
                    TrackingMaxAngle.SetDefaultValue(self.maxTrackingAngle);
                    if (TrackingMaxAngle.IsNotDefault())
                    {
                        self.maxTrackingAngle = TrackingMaxAngle.FloatValue;
                    }
                };
            }

            public override void WriteNewHooks()
            {
            }
        }
    }
}