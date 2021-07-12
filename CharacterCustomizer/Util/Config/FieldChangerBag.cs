using System;
using System.Collections.Generic;
using CharacterCustomizer.Util.Config;

namespace CharacterCustomizer.Util.Config
{
    public class FieldChangerBag
    {
        protected readonly IConfigProvider _configProvider;
        protected readonly List<IFieldChanger> _fieldChangers;

        public FieldChangerBag(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            _fieldChangers = new List<IFieldChanger>();
        }

        public virtual void AddFieldConfig<T>(string key, string description, string fieldName, bool staticField = false)
        {
            _fieldChangers.Add(new FieldConfigWrapper<T>(_configProvider.BindConfig<T>(key, description), fieldName, staticField));
        }

        public void Apply(Type type)
        {
            _fieldChangers.ForEach(changer => changer.Apply(type));
        }

        public void Apply(object o)
        {
            _fieldChangers.ForEach(changer => changer.Apply(o));
        }

        public ConfigEntryDescriptionWrapper<T> GetWrapperByFieldName<T>(string fieldName)
        {
            foreach (var fieldChanger in _fieldChangers)
            {
                if (fieldChanger is FieldConfigWrapper<T> wrapper && wrapper.FieldName.Equals(fieldName))
                {
                    return wrapper.ConfigEntryDescriptionWrapper;
                }
            }

            throw new ArgumentException($"No field changer for name {fieldName}");
        } 

        public T GetValueByFieldName<T>(string fieldName)
        {
            return GetWrapperByFieldName<T>(fieldName).Value;
        }
    }
}