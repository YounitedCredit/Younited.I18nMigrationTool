using I18nMigrationTool.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace I18nMigrationTool.Services.Interfaces
{
    public class I18NAggregateService
    {
        public static List<Translation> Aggregate(List<I18NFile> i18NFiles)
        {
            List<Translation> translations = new List<Translation>();

            foreach (var i18NFile in i18NFiles)
            {
                foreach (var i18NTranslation in i18NFile.Translations)
                {
                    if (translations.Any(t => t.I18NKey == i18NTranslation.Key))
                    {
                        var translation = translations.Single(t => t.I18NKey == i18NTranslation.Key);
                        translation.IsEligibleToMigrate &= i18NTranslation.IsEligibleToMigrate;
                        translation.TranslationSingles.Add(new TranslationSingle
                        {
                            Tag = i18NFile.Tag,
                            CultureInfo = i18NFile.CultureInfo,
                            Text = i18NTranslation.FormattedTranslation
                        });

                    }
                    else
                    {
                        translations.Add(new Translation
                        {
                            I18NKey = i18NTranslation.Key,
                            IsEligibleToMigrate = i18NTranslation.IsEligibleToMigrate,
                            TranslationSingles = new List<TranslationSingle>
                            {
                                new TranslationSingle{
                                    Tag = i18NFile.Tag,
                                    CultureInfo = i18NFile.CultureInfo,
                                    Text = i18NTranslation.FormattedTranslation
                                }
                            }
                        });
                    }
                }

            }

            foreach (var translation in translations)
            {
                foreach (var group in translation.TranslationSingles.GroupBy(d => d.CultureInfo.Name))
                {
                    var differenceBetweenTag = group.Where(t => !string.IsNullOrEmpty(t.Text) && t.Text != $"@@{translation.I18NKey}").Select(t => t.Text).Distinct().Count() > 1;
                    if (differenceBetweenTag)
                        translation.IsEligibleToMigrate = false;
                }
            }

            return translations;
        }
    }
}