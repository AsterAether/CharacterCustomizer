using System;
using System.Globalization;
using System.Reflection;
using BepInEx.Configuration;
using JetBrains.Annotations;

namespace CharacterCustomizer.Util.Config
{
    public class ConfigEntryDescriptionWrapper<T> : IMarkdownString
    {
        private bool _updatedDescription;
        private bool _updateBoolValue;

        public ConfigEntry<T> Entry { get; private set; }

        public Action<IFieldChanger> ChangeAction { get; set; }

        public IFieldChanger Changer { get; set; }

        public T Value => Entry.Value;

        public ConfigEntryDescriptionWrapper(ConfigEntry<T> entry, bool updateBoolValue = false)
        {
            _updateBoolValue = updateBoolValue;
            Entry = entry;
        }

        public bool IsDefault()
        {
            return Entry.BoxedValue.Equals(Entry.DefaultValue);
        }

        public bool IsNotDefault() => !IsDefault();

        public bool RunIfNotDefault(Action<T> function)
        {
            if (IsNotDefault())
            {
                function(Entry.Value);
                return true;
            }

            return false;
        }

        public string ToMarkdownString()
        {
            return "* **" + Entry.Definition.Key + ":** " + Entry.Description.Description;
        }

        public void UpdateDescription(T vanillaValue, bool ignoreAlreadyUpdated = false)
        {
            if (!ignoreAlreadyUpdated && _updatedDescription)
            {
                return;
            }

            _updatedDescription = true;
            string desc = Entry.Description.Description;
            T val = Value;
            ConfigDefinition def = Entry.Definition;
            ConfigFile file = Entry.ConfigFile;

            file.Remove(def);
            Entry = file.Bind(def, (T) (Entry.DefaultValue is bool ? vanillaValue : Entry.DefaultValue),
                new ConfigDescription(desc + (desc.EndsWith(".") ? "" : ".") + " Vanilla value: " + vanillaValue));
            if (Changer != null)
                Entry.SettingChanged += (sender, args) => ChangeAction.Invoke(Changer);
            Entry.Value = val is bool && _updateBoolValue ? vanillaValue : val;
            Entry.ConfigFile.Save();
        }

        public void AddFieldChangedListener(IFieldChanger changer, Action<IFieldChanger> func)
        {
            ChangeAction = func;
            Changer = changer;
            Entry.SettingChanged += (sender, args) => func.Invoke(changer);
        }
    }
}