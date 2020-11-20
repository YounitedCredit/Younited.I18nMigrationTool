using AngleSharp;
using AngleSharp.Dom;
using I18nMigrationTool.Services.Interfaces;
using I18nMigrationTool.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace I18nMigrationTool.Services
{
    public class MigrateToJsonService : IMigrateToJsonService
    {
        private readonly ITranslationFinderService _translationFinderService;
        private readonly IBrowsingContext _context;
        private const string I18NAttribute = "i18n";

        public MigrateToJsonService(ITranslationFinderService translationFinderService)
        {
            _translationFinderService = translationFinderService;
            var config = Configuration.Default;
            _context = BrowsingContext.New(config);
        }

        public async Task<FileMigrationResult> ReplaceI18N(string templateHtml)
        {
            var results = new List<KeyMigrationResult>();

            var document = await _context.OpenAsync(req => req.Content(templateHtml));


            var elements = document.All.Where(m => m.HasAttribute(I18NAttribute)).ToList();

            foreach (var element in elements)
            {
                var i18NKey = element.GetAttribute(I18NAttribute).Replace("@@", "");

                var isEligibleToMigrate = _translationFinderService.IsTranslationEligibleToMigrate(i18NKey);

                if (isEligibleToMigrate)
                {
                    var jsonKey = BuildJsonKey(i18NKey);

                    templateHtml = ReplaceI18NAttribute(element, jsonKey, templateHtml);

                    element.InnerHtml = string.Empty;

                    results.Add(new KeyMigrationResult()
                    {
                        NewKey = jsonKey,
                        I18NKey = i18NKey,
                    });

                    _translationFinderService.SetJsonKey(i18NKey, jsonKey);
                }
                else
                {
                    results.Add(new KeyMigrationResult()
                    {
                        I18NKey = i18NKey,
                    });
                }
            }

            return new FileMigrationResult
            {
                KeyMigrationResults = results,
                NewHtml = templateHtml
            };
        }

        private static string BuildJsonKey(string i18NKey)
        {
            return i18NKey.Contains('_') ? i18NKey.ToLower() : i18NKey.ToUnderscoreCase();
        }

        private string ReplaceI18NAttribute(IElement element, string newKey, string originalHtml)
        {
            var oldAttribute = $"{I18NAttribute}=\"{element.GetAttribute(I18NAttribute)}\"";
            var newAttribute = $"[innerHTML]=\"'{newKey}' | translate\"";

            var oldInnerHtml = $">{element.InnerHtml}<";
            var newInnerHtml = $">{string.Empty}<";

            return originalHtml.Replace(oldAttribute, newAttribute).Replace(oldInnerHtml, newInnerHtml);
        }
    }
}
