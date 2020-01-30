using BepInEx.Configuration;

namespace CharacterCustomizer.Util.Config
{
    public static class ConfigEntryExtension
    {
        public static string ToMarkdownString<T>(this ConfigEntry<T> entry)
        {
            return "* **" + entry.Definition.Key + ":** " + entry.Description.Description;
        }
    }
}