using I18nMigrationTool.Services.Models;
using System.Collections.Generic;
using System.IO;

namespace I18nMigrationTool.Services
{
    public class ReportService
    {
        public static void CreateReport(List<Translation> translations, string filepath, List<I18NFile> i18NTranslations)
        {
            using (StreamWriter writer = new StreamWriter(new FileStream(filepath, FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine("i18NKey;jsonKey;IsEligibleToMigrate;MigrationDone;Language;Tag");
                foreach (var translation in translations)
                {
                    foreach (var translationSingle in translation.TranslationSingles)
                    {
                        writer.WriteLine($"{translation.I18NKey};{translation.JsonKey};{translation.IsEligibleToMigrate};{translation.MigrationDone};{translationSingle.CultureInfo.Name};{translationSingle.Tag}");
                    }

                }

            }
        }
    }
}