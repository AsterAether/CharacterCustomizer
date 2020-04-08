using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    public class CustomCroco : CustomSurvivor
    {
        public CustomCroco(bool updateVanilla, ConfigFile file, ManualLogSource logger) : base(SurvivorIndex.Croco,
            "Acrid", "CROCO", updateVanilla, file, logger)
        {
            AddSkill("Poison", 53);
            AddSkill("Blight", 52);

            AddSkill("ViciousWounds", 54);
            AddSkill("Neurotoxin", 55);
            AddSkill("Bite", 48);
            AddSkill("CausticLeap", 51);
            AddSkill("FrenziedLeap", 49);

            AddSkill("Epidemic", 50);
        }
    }
}