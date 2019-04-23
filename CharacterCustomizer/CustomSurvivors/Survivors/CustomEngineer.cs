using System;
using System.Collections.Generic;
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
            public CustomEngineer() : base(RoR2.SurvivorIndex.Engineer, "Engineer",
                "FireGrenade",
                "PlaceMine",
                "PlaceBubbleShield",
                "PlaceTurret")
            {
            }

            public ValueConfigWrapper<int> TurretMaxDeployCount;

            public ValueConfigWrapper<int> MineMaxDeployCount;

            public ValueConfigWrapper<int> ShieldMaxDeployCount;

            public FieldConfigWrapper<string> ShieldDuration;

            public ConfigWrapper<bool> ShieldEndlessDuration;

            public List<IFieldChanger> ShieldDeployedFields;

            public FieldConfigWrapper<int> GrenadeMaxFireAmount;
            public FieldConfigWrapper<int> GrenadeMinFireAmount;
            public ConfigWrapper<bool> GrenadeSetChargeCountToFireAmount;
            public FieldConfigWrapper<string> GrenadeMaxChargeTime;
            public FieldConfigWrapper<string> GrenadeTotalChargeDuration;

            public List<IFieldChanger> ChargeGrenadesFields;


            public override void InitConfigValues()
            {
                TurretMaxDeployCount = WrapConfigInt("TurretMaxDeployCount",
                    "The maximum number of turrets the Engineer can place.");


                MineMaxDeployCount = WrapConfigInt("MineMaxDeployCount",
                    "The maximum number of mines the Engineer can place.");


                ShieldMaxDeployCount = WrapConfigInt("ShieldMaxDeployCount",
                    "The maximum number of shields the Engineer can place.");

                ShieldDuration = new FieldConfigWrapper<string>(
                    WrapConfigFloat("ShieldDuration", "The number of seconds the shield is active."), "lifetime", true);

                ShieldDeployedFields = new List<IFieldChanger> {ShieldDuration};

                ShieldEndlessDuration = WrapConfigStandardBool("ShieldEndlessDuration",
                    "If the duration of the shield should be endless.");


                GrenadeMaxFireAmount = new FieldConfigWrapper<int>(WrapConfigInt("GrenadeMaxFireAmount",
                    "The maximum number of grenades the Engineer can fire."), "maxGrenadeCount", true);


                GrenadeMinFireAmount = new FieldConfigWrapper<int>(WrapConfigInt("GrenadeMinFireAmount",
                    "The minimum number of grenades the Engineer fires."), "minGrenadeCount", true);


                GrenadeSetChargeCountToFireAmount = WrapConfigStandardBool("GrenadeSetChargeCountToFireAmount",
                    "Set the number of \"clicks\" you hear in the charging animation to the maximum grenade count.");


                GrenadeMaxChargeTime =
                    new FieldConfigWrapper<string>(WrapConfigFloat("GrenadeMaxChargeTime",
                        "Maximum charge time (animation) for grenades, in seconds."), "baseMaxChargeTime", true);


                GrenadeTotalChargeDuration =
                    new FieldConfigWrapper<string>(WrapConfigFloat("GrenadeTotalChargeDuration",
                        "Maximum charge duration (logic) for grenades, in seconds."), "baseTotalDuration", true);

                ChargeGrenadesFields = new List<IFieldChanger>
                {
                    GrenadeMaxChargeTime, GrenadeMaxFireAmount, GrenadeMinFireAmount, GrenadeTotalChargeDuration
                };
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

                        c.GotoNext(
                            x => x.MatchStloc(1),
                            x => x.MatchLdarg(0)
                        );
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
                if (GrenadeMinFireAmount.ValueConfigWrapper.Value >= 8 ||
                    GrenadeMaxFireAmount.ValueConfigWrapper.Value >= 8)
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

                    Type shieldDeployed = typeof(EntityStates.Engi.EngiBubbleShield.Deployed);

                    ShieldDeployedFields.ForEach(changer => changer.Apply(shieldDeployed));
                    if (ShieldEndlessDuration.Value)
                    {
                        EntityStates.Engi.EngiBubbleShield.Deployed.lifetime = float.PositiveInfinity;
                    }

                    Type chargeGrenades = assembly.GetClass("EntityStates.Engi.EngiWeapon", "ChargeGrenades");

                    ChargeGrenadesFields.ForEach(changer => changer.Apply(chargeGrenades));

                    if (GrenadeSetChargeCountToFireAmount.Value &&
                        GrenadeMaxFireAmount.ValueConfigWrapper.IsNotDefault())
                    {
                        chargeGrenades.SetFieldValue("maxCharges", GrenadeMaxFireAmount.ValueConfigWrapper.Value);
                    }
                };
            }

            public override void WriteNewHooks()
            {
            }
        }
    }
}