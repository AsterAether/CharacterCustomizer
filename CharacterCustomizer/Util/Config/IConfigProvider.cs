namespace CharacterCustomizer.Util.Config
{
    public interface IConfigProvider
    {
        string GetSectionName();

        ConfigEntryDescriptionWrapper<T> BindConfig<T>(string key, T defaultVal,
            string description);

        ConfigEntryDescriptionWrapper<T> BindConfig<T>(string key,
            string description);
    }
}