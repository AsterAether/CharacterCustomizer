using System;
using System.Linq;
using System.Reflection;
using AetherLib.Util;
using AetherLib.Util.Config;
using AetherLib.Util.Reflection;
using BepInEx;
using BepInEx.Configuration;
using CharacterCustomizer;
using EntityStates;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using UnityEngine;

namespace CharacterCustomizer.CustomSurvivors
{
    namespace Engineer
    {
        public class CustomEngineer : CustomSurvivor
        {
            public CustomEngineer() : base("Engineer")
            {
            }

            public ValueConfigWrapper<int> TurretMaxDeployCount;
            public ValueConfigWrapper<string> TurretCooldown;


            public ValueConfigWrapper<int> MineMaxDeployCount;
            public ValueConfigWrapper<string> MineCooldown;


            public ValueConfigWrapper<int> ShieldMaxDeployCount;
            public ValueConfigWrapper<string> ShieldCooldown;

            public ValueConfigWrapper<string> ShieldDuration;
            public ConfigWrapper<bool> ShieldEndlessDuration;

            public ValueConfigWrapper<int> GrenadeMaxFireAmount;
            public ValueConfigWrapper<int> GrenadeMinFireAmount;
            public ConfigWrapper<bool> GrenadeSetChargeCountToFireAmount;
            public ValueConfigWrapper<string> GrenadeMaxChargeTime;

            public ValueConfigWrapper<string> GrenadeTotalChargeDuration;


            public override void InitConfigValues()
            {
                TurretMaxDeployCount = WrapConfigInt("TurretMaxDeployCount",
                    "The maximum number of turrets the Engineer can place.");

                TurretCooldown = WrapConfigFloat("TurretCooldown", "Cooldown of the Turret skill, in seconds");


                MineMaxDeployCount = WrapConfigInt("MineMaxDeployCount",
                    "The maximum number of mines the Engineer can place.");


                MineCooldown = WrapConfigFloat("MineCooldown", "Cooldown of the Mine skill, in seconds");

                ShieldMaxDeployCount = WrapConfigInt("ShieldMaxDeployCount",
                    "The maximum number of shields the Engineer can place.");

                ShieldDuration = WrapConfigFloat("ShieldDuration", "The number of seconds the shield is active.");

                ShieldEndlessDuration = WrapConfigBool("ShieldEndlessDuration",
                    "If the duration of the shield should be endless.");


                ShieldCooldown = WrapConfigFloat("ShieldCooldown", "Cooldown of the Shield skill, in seconds");


                GrenadeMaxFireAmount = WrapConfigInt("GrenadeMaxFireAmount",
                    "The maximum number of grenades the Engineer can fire.");


                GrenadeMinFireAmount = WrapConfigInt("GrenadeMinFireAmount",
                    "The minimum number of grenades the Engineer fires.");


                GrenadeSetChargeCountToFireAmount = WrapConfigBool("GrenadeSetChargeCountToFireAmount",
                    "Set the steps when charging (animation) to the same amount as FireAmount.");


                GrenadeMaxChargeTime =
                    WrapConfigFloat("GrenadeMaxChargeTime",
                        "Maximum charge time (animation) for grenades, in seconds.");


                GrenadeTotalChargeDuration =
                    WrapConfigFloat("GrenadeTotalChargeDuration",
                        "Maximum charge duration (logic) for grenades, in seconds.");
            }

            public override void OverrideGameValues()
            {
                // Count for: Turret, Mines, Shield
                IL.RoR2.CharacterMaster.AddDeployable += il =>
                {
                    ILCursor c = new ILCursor(il);

                    // MineCount:
                    // Double .Next because GotoNext sets the pointer to before the ldf.i4.s 10
                    while (c.Next.Next.OpCode != OpCodes.Stloc_1)
                    {
                        c.GotoNext(x => x.MatchLdcI4(10));
                    }

                    // Step over ldf.i4.s 10
                    c.GotoNext();

                    MineMaxDeployCount.SetDefaultValue(10);
                    MineMaxDeployCount.RunIfNotDefault(count =>
                    {
                        // Pop the 10 from stack
                        c.Emit(OpCodes.Stloc_1);

                        // Push custom value on stack
                        c.Emit(OpCodes.Ldc_I4, (int) count);
                    });


                    // TurretCount:
                    // Double .Next because GotoNext sets the pointer to before the ldf.i4.2
                    while (c.Next.Next.OpCode != OpCodes.Stloc_1)
                    {
                        c.GotoNext(x => x.MatchLdcI4(2));
                    }

                    c.GotoNext();

                    TurretMaxDeployCount.SetDefaultValue(2);
                    TurretMaxDeployCount.RunIfNotDefault(count =>
                    {
                        // Pop the 2 from stack
                        c.Emit(OpCodes.Stloc_1);

                        // Push custom value on stack
                        c.Emit(OpCodes.Ldc_I4, (int) count);
                    });

                    // ShieldCount:
                    // Double .Next because GotoNext sets the pointer to before the ldf.i4.1
                    while (c.Next.Next.OpCode != OpCodes.Stloc_1)
                    {
                        c.GotoNext(x => x.MatchLdcI4(1));
                    }

                    //Step over
                    c.GotoNext();

                    ShieldMaxDeployCount.SetDefaultValue(1);
                    ShieldMaxDeployCount.RunIfNotDefault(count =>
                    {
                        // Pop the 1 from stack
                        c.Emit(OpCodes.Stloc_1);

                        // Push custom value on stack
                        c.Emit(OpCodes.Ldc_I4, (int) count);
                    });
                };

                SurvivorAPI.SurvivorCatalogReady += (sender, args) =>
                {
                    SurvivorDef engi =
                        SurvivorAPI.SurvivorDefinitions.First(def => def.survivorIndex == SurvivorIndex.Engineer);
                    GenericSkill[] skills = engi.bodyPrefab.GetComponents<GenericSkill>();
                    foreach (GenericSkill genericSkill in skills)
                    {
                        switch (genericSkill.skillName)
                        {
                            case "PlaceMine":
                                MineCooldown.SetDefaultValue(genericSkill.baseRechargeInterval);
                                if (MineCooldown.IsNotDefault())
                                {
                                    genericSkill.baseRechargeInterval = MineCooldown.FloatValue;
                                }

                                break;
                            case "PlaceTurret":
                                TurretCooldown.SetDefaultValue(genericSkill.baseRechargeInterval);
                                if (TurretCooldown.IsNotDefault())
                                {
                                    genericSkill.baseRechargeInterval = TurretCooldown.FloatValue;
                                }

                                break;
                            case "FireGrenades":
                                break;
                            case "PlaceBubbleShield":
                                ShieldCooldown.SetDefaultValue(genericSkill.baseRechargeInterval);
                                if (ShieldCooldown.IsNotDefault())
                                {
                                    genericSkill.baseRechargeInterval = ShieldCooldown.FloatValue;
                                }

                                break;
                        }
                    }

                    SurvivorAPI.ReconstructSurvivors();
                };


                // typeof(RoR2Application).Assembly doesn't seem to work
                On.RoR2.Run.Awake += (orig, self) =>
                {
                    orig(self);
                    Assembly assembly = self.GetType().Assembly;

                    ShieldDuration.SetDefaultValue(EntityStates.Engi.EngiBubbleShield.Deployed.lifetime);
                    if (ShieldEndlessDuration.Value)
                    {
                        EntityStates.Engi.EngiBubbleShield.Deployed.lifetime = float.PositiveInfinity;
                    }
                    else if (ShieldDuration.IsNotDefault())
                    {
                        EntityStates.Engi.EngiBubbleShield.Deployed.lifetime = ShieldDuration.FloatValue;
                    }

                    Type chargeGrenades = assembly.GetClass("EntityStates.Engi.EngiWeapon", "ChargeGrenades");

                    GrenadeMinFireAmount.SetDefaultValue(chargeGrenades.GetFieldValue<int>("minGrenadeCount"));
                    GrenadeMinFireAmount.RunIfNotDefault(num =>
                    {
                        chargeGrenades.SetFieldValue("minGrenadeCount", num);
                    });


                    GrenadeMaxFireAmount.SetDefaultValue(chargeGrenades.GetFieldValue<int>("maxGrenadeCount"));
                    GrenadeMaxFireAmount.RunIfNotDefault(num =>
                    {
                        chargeGrenades.SetFieldValue("maxGrenadeCount", num);
                    });

                    if (GrenadeSetChargeCountToFireAmount.Value && GrenadeMaxFireAmount.IsNotDefault())
                    {
                        chargeGrenades.SetFieldValue("maxCharges", GrenadeMaxFireAmount.Value);
                    }

                    GrenadeTotalChargeDuration.SetDefaultValue(
                        chargeGrenades.GetFieldValue<float>("baseTotalDuration"));
                    if (GrenadeTotalChargeDuration.IsNotDefault())
                    {
                        chargeGrenades.SetFieldValue("baseTotalDuration", GrenadeTotalChargeDuration.FloatValue);
                    }

                    GrenadeMaxChargeTime.SetDefaultValue(chargeGrenades.GetFieldValue<float>("baseMaxChargeTime"));
                    if (GrenadeMaxChargeTime.IsNotDefault())
                    {
                        chargeGrenades.SetFieldValue("baseMaxChargeTime", GrenadeMaxChargeTime.FloatValue);
                    }
                };
            }

            public override void WriteNewHooks()
            {
            }
        }
    }
}