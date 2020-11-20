using I18nMigrationTool.Services.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace I18nMigrationTool.Services
{
    public class JsonTranslationFileCreatorService
    {
        public static void CreateFiles(List<Translation> translations, string rootDestinationPath)
        {
            var cultureInfoNames = translations.Where(t => t.MigrationDone).SelectMany(t => t.TranslationSingles)
                .Select(t => t.CultureInfo.Name).Distinct();
            foreach (var language in cultureInfoNames)
            {
                var dictionary = new Dictionary<string, string>();

                foreach (var translation in translations.Where(t => t.MigrationDone))
                {
                    var text = translation.TranslationSingles.FirstOrDefault(t => t.CultureInfo.Name == language)?.Text;
                    if (string.IsNullOrEmpty(text))
                        text = string.Empty;
                    dictionary.Add(translation.JsonKey, text);

                }

                var jsonString = JsonConvert.SerializeObject(dictionary);

                File.WriteAllText($"{rootDestinationPath}{language}.json", jsonString);
            }
        }
    }
}