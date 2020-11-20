namespace I18nMigrationTool.Services.Models
{
    public class I18NTranslation
    {
        public string Key { get; set; }
        public string Translation { get; set; }
        public string FormattedTranslation { get; set; }
        public bool IsEligibleToMigrate { get; set; }
    }
}