using System;
using BepInEx.Configuration;

namespace CharacterCustomizer.Util.Config
{
    public interface IFieldChanger
    {
        void Apply(Type type);

        void Apply(object obj);

        void AddFieldChangedListener(Action<IFieldChanger> func);
    }
}