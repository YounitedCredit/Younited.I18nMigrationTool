using I18nMigrationTool.Services;
using I18nMigrationTool.Services.Interfaces;
using I18nMigrationTool.Services.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace I18nMigrationTool.App
{
    public class Program
    {
        public static void Main()
        {
            // Load configuration file (appSettings.json)
            var configuration = new ConfigurationFacade();

            // instantiate XliffParserService
            var xliffParserService = new XliffParserService(configuration.XlfFilesRootPath);

            // read all the xlf files to get a list of the i18n keys and their texts
            var i18NTranslations = configuration.XlfFiles.Select(f => xliffParserService.GetTranslations(f)).ToList();
            var translations = I18NAggregateService.Aggregate(i18NTranslations);

            // parse the angular html templates to migrate ngx-translate
            MigrateTemplates(translations, configuration);

            // create the json files
            JsonTranslationFileCreatorService.CreateFiles(translations, configuration.JsonTargetPath);

            // flag as obsolete the migrated i18n keys
            foreach (var xliffFile in configuration.XlfFiles)
            {
                XliffUpdaterService.FlagAsObsoleteMigratedKey(translations.Where(t => t.MigrationDone).Select(t => t.I18NKey).ToList(), xliffFile, configuration.XlfFilesRootPath);
            }

            // create a csv report of the migration
            ReportService.CreateReport(translations, configuration.ReportFilePath, i18NTranslations);
        }

        private static void MigrateTemplates(List<Translation> translations, ConfigurationFacade configuration)
        {
            var migrater = new MigrateToJsonService(new TranslationFinderService(translations));

            foreach (var file in configuration.TemplateFilesPath)
            {
                string originalHtml = File.ReadAllText(file);

                var result = (migrater.ReplaceI18N(originalHtml)).Result;

                File.WriteAllText(file, result.NewHtml);
            }
        }
    }
}
