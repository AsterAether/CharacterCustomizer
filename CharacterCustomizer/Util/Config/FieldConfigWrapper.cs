using System;
using System.Reflection;
using BepInEx.Configuration;
using R2API.Utils;

namespace CharacterCustomizer.Util.Config
{
    public class FieldConfigWrapper<T> : IFieldChanger
    {
        public ConfigEntryDescriptionWrapper<T> ConfigEntryDescriptionWrapper { get; }

        public bool StaticField { get; }

        public string FieldName { get; }

        public FieldConfigWrapper(ConfigEntryDescriptionWrapper<T> configEntryDescriptionWrapper, string fieldName, bool staticField = false)
        {
            ConfigEntryDescriptionWrapper = configEntryDescriptionWrapper;
            FieldName = fieldName;
            StaticField = staticField;
        }

        public G GetValue<G>(Type type)
        {
            if (!StaticField)
            {
                throw new ArgumentException("Not a static FieldConfigWrapper");
            }

            return type.GetFieldValue<G>(FieldName);
        }

        public G GetValue<G>(object obj)
        {
            if (StaticField)
            {
                throw new ArgumentException("Not a instance FieldConfigWrapper");
            }
            
            return obj.GetFieldValue<G>(FieldName);
        }

        public void Apply(Type type)
        {
            if (!StaticField)
            {
                throw new ArgumentException("Not a static FieldConfigWrapper");
            }

            ConfigEntryDescriptionWrapper.UpdateDescription(GetValue<T>(type));

            if (ConfigEntryDescriptionWrapper.IsNotDefault())
            {
                type.SetFieldValue(FieldName, ConfigEntryDescriptionWrapper.Value);
            }
        }

        public void Apply(object obj)
        {
            if (StaticField)
            {
                throw new ArgumentException("Not a instance FieldConfigWrapper");
            }

            ConfigEntryDescriptionWrapper.UpdateDescription(GetValue<T>(obj));

            if (ConfigEntryDescriptionWrapper.IsNotDefault())
            {
                obj.SetFieldValue(FieldName, ConfigEntryDescriptionWrapper.Value);
            }
        }

        public void AddFieldChangedListener(Action<IFieldChanger> func)
        {
            ConfigEntryDescriptionWrapper.AddFieldChangedListener(this, func);
        }
    }
}