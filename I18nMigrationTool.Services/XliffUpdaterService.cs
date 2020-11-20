using I18nMigrationTool.Services.Models;
using System.Collections.Generic;
using System.IO;

namespace I18nMigrationTool.Services
{
    public class XliffUpdaterService
    {
        public static void FlagAsObsoleteMigratedKey(List<string> migratedKeys, XliffFile file,
            string xlfFilesRootPath)
        {
            var filePtah = Path.Combine(xlfFilesRootPath, file.Name);
            var raw = File.ReadAllText(filePtah);

            foreach (var key in migratedKeys)
            {
                raw = raw.Replace($"id=\"{key}\"", $"id=\"obsolete_{key}\"");
            }

            File.WriteAllText(filePtah, raw);
        }
    }
}