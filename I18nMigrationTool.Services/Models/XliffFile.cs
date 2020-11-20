using System.Globalization;

namespace I18nMigrationTool.Services.Models
{
    public class XliffFile
    {
        public string Name { get; set; }
        public CultureInfo CultureInfo { get; set; }
        public string Tag { get; set; }
    }
}