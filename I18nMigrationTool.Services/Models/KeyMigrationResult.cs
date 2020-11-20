using System.Collections.Generic;

namespace I18nMigrationTool.Services.Models
{
    public class KeyMigrationResult
    {
        public string NewKey { get; set; }
        public string I18NKey { get; set; }
    }

    public class FileMigrationResult
    {
        public string NewHtml { get; set; }
        public List<KeyMigrationResult> KeyMigrationResults { get; set; }
    }
}