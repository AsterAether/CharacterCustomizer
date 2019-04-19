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
using CharacterCustomizer.CustomSurvivors.MultT;

namespace CharacterCustomizer
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("at.aster.aetherlib")]
    [BepInPlugin("at.aster.charactercustomiser", "CharacterCustomizer", "0.0.1")]
    public class CharacterCustomizer : BaseUnityPlugin
    {
        private List<CustomSurvivor> CustomSurvivors { get; set; }

        public void Awake()
        {
            ConfigWrapper<bool> createReadme = Config.Wrap(
                "General",
                "PrintReadme",
                "Outputs a file called \"README.md\" to the working directory, containing all config values formated as Markdown.",
                false);


            CustomSurvivors = new List<CustomSurvivor>
            {
                new CustomEngineer(),
                new CustomCommando(),
                new CustomArtificer(),
                new CustomMultT()
            };

            StringBuilder markdown = new StringBuilder();

            foreach (var customSurvivor in CustomSurvivors)
            {
                customSurvivor.InitVariables(Config, Logger);
                customSurvivor.InitConfigValues();
                customSurvivor.OverrideGameValues();
                customSurvivor.WriteNewHooks();

                if (createReadme.Value)
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

            if (createReadme.Value)
            {
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\README.md", markdown.ToString());
            }
        }
    }
}