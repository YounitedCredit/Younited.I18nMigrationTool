using System.Collections.Generic;

namespace I18nMigrationTool.Services.Models
{
    public class Translation
    {
        public string I18NKey { get; set; }
        public bool IsEligibleToMigrate { get; set; }
        public List<TranslationSingle> TranslationSingles { get; set; }
        public bool MigrationDone => !string.IsNullOrEmpty(JsonKey);
        public string JsonKey { get; set; }
    }
}