using System.Linq;

namespace I18nMigrationTool.Services
{
    public static class ExtensionMethods
    {
        public static string ToUnderscoreCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
        }
    }
}