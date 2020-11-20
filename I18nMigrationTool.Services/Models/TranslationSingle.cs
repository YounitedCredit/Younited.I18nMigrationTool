using System.Globalization;

namespace I18nMigrationTool.Services.Models
{
    public class TranslationSingle
    {
        public CultureInfo CultureInfo { get; set; }
        public string Tag { get; set; }
        public string Text { get; set; }
    }
}