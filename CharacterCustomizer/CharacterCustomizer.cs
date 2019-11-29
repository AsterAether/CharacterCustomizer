using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AetherLib.Util.Config;
using BepInEx;
using BepInEx.Configuration;
using CharacterCustomizer.CustomSurvivors;
using CharacterCustomizer.CustomSurvivors.Survivors;
using CharacterCustomizer.CustomSurvivors.Survivors.Artificer;
using CharacterCustomizer.CustomSurvivors.Survivors.Commando;
using CharacterCustomizer.CustomSurvivors.Survivors.Engineer;
using CharacterCustomizer.CustomSurvivors.Survivors.Huntress;
using CharacterCustomizer.CustomSurvivors.Survivors.Mercenary;
using CharacterCustomizer.CustomSurvivors.Survivors.MulT;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace CharacterCustomizer
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("at.aster.aetherlib")]
    [BepInPlugin("at.aster.charactercustomizer", "CharacterCustomizer", "<version>")]
    public class CharacterCustomizer : BaseUnityPlugin
    {
        private List<CustomSurvivor> CustomSurvivors { get; set; }

        public ConfigEntry<bool> CreateReadme;
        public ConfigEntry<bool> FixSkillIconCooldownScaling;
        public ConfigEntry<bool> UpdateVanillaValues;

        public void InitConfig()
        {
            CreateReadme = Config.Bind(
                "General",
                "PrintReadme",
                false,
                "Outputs a file called \"config_values.md\" to the working directory, containing all config values formatted as Markdown.");

            FixSkillIconCooldownScaling = Config.Bind(
                "Fixes",
                "FixSkillIconCooldownScaling",
                true,
                "Fix the display of cooldowns when cooldown scaling is applied");
            
            UpdateVanillaValues = Config.Bind(
                "General",
                "UpdateVanillaValues",
                true,
                "Write default values in descriptions of settings. Will flip to false after doing it once.");
        }

        public void Awake()
        {
            InitConfig();
            ApplyFixes();

            CustomSurvivors = new List<CustomSurvivor>
            {
                new CustomEngineer(UpdateVanillaValues.Value),
                new CustomCommando(UpdateVanillaValues.Value),
                new CustomArtificer(UpdateVanillaValues.Value),
                new CustomMulT(UpdateVanillaValues.Value),
                new CustomHuntress(UpdateVanillaValues.Value),
                new CustomMercenary(UpdateVanillaValues.Value),
                new CustomBandit(UpdateVanillaValues.Value),
                new CustomTreebot(UpdateVanillaValues.Value),
                new CustomLoader(UpdateVanillaValues.Value)
            };

            StringBuilder markdown = new StringBuilder("# Config Values\n");

            markdown.AppendLine("## General");
            markdown.AppendLine(CreateReadme.ToMarkdownString());
            markdown.AppendLine(UpdateVanillaValues.ToMarkdownString());

            markdown.AppendLine("## Fixes");
            markdown.AppendLine(FixSkillIconCooldownScaling.ToMarkdownString());

            foreach (var customSurvivor in CustomSurvivors)
            {
                customSurvivor.InitVariables(Config, Logger);

                customSurvivor.Patch();

                if (CreateReadme.Value)
                {
                    markdown.AppendLine("# " + customSurvivor.CharacterName);
                    List<string> markdownLines = new List<string>();

                    foreach (IMarkdownString markdownDef in customSurvivor.MarkdownConfigEntries)
                    {
                        markdownLines.Add(markdownDef.ToMarkdownString());
                    }

                    markdownLines.Sort();

                    foreach (var markdownLine in markdownLines)
                    {
                        markdown.AppendLine(markdownLine);
                    }
                }
            }
            
            if (CreateReadme.Value)
            {
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\config_values.md",
                    markdown.ToString());
            }
        }

        private void OnDestroy()
        {
            UpdateVanillaValues.Value = false;
        }

        private void OnApplicationQuit()
        {
            UpdateVanillaValues.Value = false;
        }

        private void Start()
        {
            foreach (SurvivorDef survivorDef in ((SurvivorDef[]) SurvivorCatalog.allSurvivorDefs))
            {
                try
                {
                    CustomSurvivor cSurv = CustomSurvivors.First(s => s.SurvivorIndex == survivorDef.survivorIndex);
                    cSurv.OverrideSurvivorBase(survivorDef);
                }
                catch (InvalidOperationException)
                {
                    Logger.LogError("No custom survivor for " + survivorDef.survivorIndex);
                }
            }
        }

        public void ApplyFixes()
        {
            if (FixSkillIconCooldownScaling.Value)
            {
                IL.RoR2.UI.SkillIcon.Update += il =>
                {
                    ILCursor c = new ILCursor(il);
                    c.GotoNext(MoveType.After, i => i.MatchStloc(1));
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<SkillIcon>(OpCodes.Ldfld, "targetSkill");
                    c.EmitDelegate<Func<GenericSkill, float>>(skill => Mathf.Min(skill.baseRechargeInterval,
                        Mathf.Max(0.5f, skill.baseRechargeInterval * skill.cooldownScale)));
                    c.Emit(OpCodes.Stloc_1);
                };
            }
        }
    }
}