using I18nMigrationTool.Services.Interfaces;
using I18nMigrationTool.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace I18nMigrationTool.Services
{
    public class TranslationFinderService : ITranslationFinderService
    {
        private readonly List<Translation> _translations;

        public TranslationFinderService(List<Translation> translations)
        {
            _translations = translations;
        }
        public bool IsTranslationEligibleToMigrate(string key)
        {
            return _translations.Exists(t => t.I18NKey == key && t.IsEligibleToMigrate);
        }

        public void SetJsonKey(string key, string newKey)
        {
            _translations.Single(t => t.I18NKey == key).JsonKey = newKey;
        }
    }
}