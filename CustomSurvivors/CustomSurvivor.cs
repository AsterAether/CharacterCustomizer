using System;
using System.Collections.Generic;
using AetherLib.Util.Config;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace CharacterCustomizer.CustomSurvivors
{
    public abstract class CustomSurvivor
    {
        public readonly List<IMarkdownString> MarkdownConfigDefinitions = new List<IMarkdownString>();

        public readonly List<ConfigDefinition> NonMarkDownConfigDefinitions = new List<ConfigDefinition>();

        protected ConfigFile Config { get; private set; }

        protected ManualLogSource Logger { get; private set; }

        public string CharacterName { get; }

        public CustomSurvivor(string characterName)
        {
            CharacterName = characterName;
        }

        public void InitVariables(ConfigFile config, ManualLogSource logger)
        {
            Config = config;
            Logger = logger;
        }

        public abstract void InitConfigValues();

        public abstract void OverrideGameValues();

        public abstract void WriteNewHooks();

        public ValueConfigWrapper<int> WrapConfigInt(string key, string description)
        {
            ValueConfigWrapper<int> conf = Config.ValueWrap(CharacterName, key, description);
            MarkdownConfigDefinitions.Add(conf);
            return conf;
        }

        public ValueConfigWrapper<string> WrapConfigString(string key, string description)
        {
            ValueConfigWrapper<string> conf = Config.ValueWrap(CharacterName, key, false, description);
            MarkdownConfigDefinitions.Add(conf);
            return conf;
        }

        public ValueConfigWrapper<string> WrapConfigFloat(string key, string description)
        {
            ValueConfigWrapper<string> conf = Config.ValueWrap(CharacterName, key, true, description);
            MarkdownConfigDefinitions.Add(conf);
            return conf;
        }

        public ConfigWrapper<bool> WrapConfigBool(string key, string description)
        {
            ConfigWrapper<bool> conf = Config.Wrap(CharacterName, key, description, false);
            NonMarkDownConfigDefinitions.Add(conf.Definition);
            return conf;
        }
    }
}