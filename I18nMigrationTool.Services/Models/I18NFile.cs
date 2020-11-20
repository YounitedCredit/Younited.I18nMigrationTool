using System.Collections.Generic;
using System.Globalization;

namespace I18nMigrationTool.Services.Models
{
    public class I18NFile
    {
        public List<I18NTranslation> Translations { get; set; }
        public CultureInfo CultureInfo { get; set; }
        public string Tag { get; set; }
    }
}