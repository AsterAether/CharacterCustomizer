using System;
using System.Collections.Generic;
using System.Text;
using AetherLib.Util.Config;
using BepInEx;
using BepInEx.Configuration;
using CharacterCustomizer.CustomSurvivors;
using CharacterCustomizer.CustomSurvivors.Artificer;
using CharacterCustomizer.CustomSurvivors.Commando;
using CharacterCustomizer.CustomSurvivors.Engineer;
using CharacterCustomizer.CustomSurvivors.Huntress;
using CharacterCustomizer.CustomSurvivors.Mercenary;
using CharacterCustomizer.CustomSurvivors.MultT;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace CharacterCustomizer
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("at.aster.aetherlib")]
    [BepInPlugin("at.aster.charactercustomiser", "CharacterCustomizer", "<version>")]
    public class CharacterCustomizer : BaseUnityPlugin
    {
        private List<CustomSurvivor> CustomSurvivors { get; set; }

        private ConfigWrapper<bool> CreateReadme;
        private ConfigWrapper<bool> FixSkillIconCooldownScaling;

        public void InitConfig()
        {
            CreateReadme = Config.Wrap(
                "General",
                "PrintReadme",
                "Outputs a file called \"README.md\" to the working directory, containing all config values formatted as Markdown.",
                false);

            FixSkillIconCooldownScaling = Config.Wrap(
                "Fixes",
                "FixSkillIconCooldownScaling",
                "Fix the display of cooldowns when cooldown scaling is applied",
                true);
        }

        public void Awake()
        {
            InitConfig();
            ApplyFixes();
            
            CustomSurvivors = new List<CustomSurvivor>
            {
                new CustomEngineer(),
                new CustomCommando(),
                new CustomArtificer(),
                new CustomMultT(),
                new CustomHuntress(),
                new CustomMercenary(),
                new CustomHandD(),
                new CustomBandit(),
                new CustomSniper()
            };

            StringBuilder markdown = new StringBuilder();

            foreach (var customSurvivor in CustomSurvivors)
            {
                customSurvivor.InitVariables(Config, Logger);
                customSurvivor.InitConfigValues();
                customSurvivor.OverrideGameValues();
                customSurvivor.WriteNewHooks();

                if (CreateReadme.Value)
                {
                    markdown.AppendLine("### " + customSurvivor.CharacterName);
                    List<string> markdownLines = new List<string>();

                    foreach (IMarkdownString markdownDef in customSurvivor.MarkdownConfigDefinitions)
                    {
                        markdownLines.Add(markdownDef.ToMarkdownString());
                    }

                    foreach (ConfigDefinition markdownDef in customSurvivor.NonMarkDownConfigDefinitions)
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

            markdown.AppendLine("### General");
            markdown.AppendLine(CreateReadme.Definition.ToMarkdownString());

            markdown.AppendLine("### Fixes");
            markdown.AppendLine(FixSkillIconCooldownScaling.Definition.ToMarkdownString());

            if (CreateReadme.Value)
            {
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\README.md", markdown.ToString());
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