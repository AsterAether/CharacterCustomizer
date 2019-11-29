using System.Collections.Generic;
using AetherLib.Util.Config;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    namespace Huntress
    {
        public class CustomHuntress : CustomSurvivor
        {
            public FieldConfigWrapper<float> TrackingMaxDistance;

            public FieldConfigWrapper<float> TrackingMaxAngle;

            public List<IFieldChanger> TrackingFields;

            public CustomHuntress(bool updateVanilla) : base(SurvivorIndex.Huntress, "Huntress",
                "FireSeekingArrow",
                "Glaive",
                "Blink",
                "ArrowRain", updateVanilla)
            {
            }

            public override void InitConfigValues()
            {
                TrackingMaxDistance = new FieldConfigWrapper<float>(BindConfigFloat("TrackingMaxDistance",
                    "The maximum distance the tracking of the huntress works."), "maxTrackingDistance");


                TrackingMaxAngle = new FieldConfigWrapper<float>(BindConfigFloat("TrackingMaxAngle",
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