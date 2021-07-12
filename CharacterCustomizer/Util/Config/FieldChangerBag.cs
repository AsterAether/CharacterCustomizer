using System;
using System.Collections.Generic;
using CharacterCustomizer.Util.Config;

namespace CharacterCustomizer.Util.Config
{
    public class FieldChangerBag
    {
        protected readonly IConfigProvider _configProvider;
        protected readonly Dictionary<string, IFieldChanger> _fieldChangers;

        public FieldChangerBag(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            _fieldChangers = new Dictionary<string, IFieldChanger>();
        }

        public virtual void AddFieldConfig<T>(string key, string description, string fieldName,
            bool staticField = false)
        {
            _fieldChangers.Add(fieldName,
                new FieldConfigWrapper<T>(_configProvider.BindConfig<T>(key, description), fieldName, staticField));
        }

        public void Apply(Type type)
        {
            foreach (var changer in _fieldChangers.Values)
            {
                changer.Apply(type);
            }
        }

        public void Apply(object o)
        {
            foreach (var changer in _fieldChangers.Values)
            {
                changer.Apply(o);
            }
        }

        public ConfigEntryDescriptionWrapper<T> GetWrapperByFieldName<T>(string fieldName)
        {
            if (_fieldChangers[fieldName] is FieldConfigWrapper<T> wrapper && wrapper.FieldName.Equals(fieldName))
            {
                return wrapper.ConfigEntryDescriptionWrapper;
            }
            
            throw new ArgumentException($"No field changer for name {fieldName}");
        }

        public T GetValueByFieldName<T>(string fieldName)
        {
            return GetWrapperByFieldName<T>(fieldName).Value;
        }
    }
}