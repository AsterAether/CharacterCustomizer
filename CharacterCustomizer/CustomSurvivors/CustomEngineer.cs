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
            public CustomEngineer() : base(RoR2.SurvivorIndex.Engineer,"Engineer",
                "FireGrenade",
                "PlaceMine",
                "PlaceBubbleShield",
                "PlaceTurret")
            {
            }

            public ValueConfigWrapper<int> TurretMaxDeployCount;


            public ValueConfigWrapper<int> MineMaxDeployCount;


            public ValueConfigWrapper<int> ShieldMaxDeployCount;

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


                MineMaxDeployCount = WrapConfigInt("MineMaxDeployCount",
                    "The maximum number of mines the Engineer can place.");


                ShieldMaxDeployCount = WrapConfigInt("ShieldMaxDeployCount",
                    "The maximum number of shields the Engineer can place.");

                ShieldDuration = WrapConfigFloat("ShieldDuration", "The number of seconds the shield is active.");

                ShieldEndlessDuration = WrapConfigBool("ShieldEndlessDuration",
                    "If the duration of the shield should be endless.");


                GrenadeMaxFireAmount = WrapConfigInt("GrenadeMaxFireAmount",
                    "The maximum number of grenades the Engineer can fire.");


                GrenadeMinFireAmount = WrapConfigInt("GrenadeMinFireAmount",
                    "The minimum number of grenades the Engineer fires.");


                GrenadeSetChargeCountToFireAmount = WrapConfigBool("GrenadeSetChargeCountToFireAmount",
                    "Set the number of \"clicks\" you hear in the charging animation to the maximum grenade count.");


                GrenadeMaxChargeTime =
                    WrapConfigFloat("GrenadeMaxChargeTime",
                        "Maximum charge time (animation) for grenades, in seconds.");


                GrenadeTotalChargeDuration =
                    WrapConfigFloat("GrenadeTotalChargeDuration",
                        "Maximum charge duration (logic) for grenades, in seconds.");
            }

            public override void OverrideGameValues()
            {
                MineMaxDeployCount.SetDefaultValue(10);
                TurretMaxDeployCount.SetDefaultValue(2);
                ShieldMaxDeployCount.SetDefaultValue(1);

                if (MineMaxDeployCount.IsNotDefault() || TurretMaxDeployCount.IsNotDefault() ||
                    ShieldMaxDeployCount.IsNotDefault())
                {
                    IL.RoR2.CharacterMaster.AddDeployable += il =>
                    {
                        var c = new ILCursor(il).Goto(0);

                        c.GotoNext(x => x.MatchStloc(1) && x.Next.MatchLdarg(0));
                        c.Index += 1;
                        c.Next.OpCode = OpCodes.Nop;
                        c.Index += 1;
                        c.Emit(OpCodes.Ldloc_1);
                        c.Emit(OpCodes.Ldarg_0);
                        c.Emit(OpCodes.Ldarg_2);


                        c.EmitDelegate<Func<int, CharacterMaster, DeployableSlot, int>>((maxDeploy, self, slot) =>
                        {
                            switch (slot)
                            {
                                case DeployableSlot.EngiMine:
                                    if (MineMaxDeployCount.IsNotDefault())
                                    {
                                        maxDeploy = MineMaxDeployCount.Value;
                                    }

                                    break;
                                case DeployableSlot.EngiTurret:
                                    if (TurretMaxDeployCount.IsNotDefault())
                                    {
                                        maxDeploy = TurretMaxDeployCount.Value;
                                    }

                                    break;
                                case DeployableSlot.EngiBubbleShield:
                                    if (ShieldMaxDeployCount.IsNotDefault())
                                    {
                                        maxDeploy = ShieldMaxDeployCount.Value;
                                    }

                                    break;
                            }

                            return maxDeploy;
                        });
                        c.Emit(OpCodes.Stloc_1);
                        c.Emit(OpCodes.Ldarg_0);
                    };
                }


                // Workaround for more than 8 max grenades
                if (GrenadeMinFireAmount.Value >= 8 || GrenadeMaxFireAmount.Value >= 8)
                {
                    On.EntityStates.Engi.EngiWeapon.FireGrenades.OnEnter += (orig, self) =>
                    {
                        Assembly assembly = self.GetType().Assembly;
                        Type fireGrenades = assembly.GetClass("EntityStates.Engi.EngiWeapon", "FireGrenades");

                        orig(self);
                        self.SetFieldValue("duration",
                            fireGrenades.GetFieldValue<float>("baseDuration")
                            * self.GetFieldValue<int>("grenadeCountMax") / 8f
                                                                         / self.GetFieldValue<float>("attackSpeedStat")
                        );
                    };
                }

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