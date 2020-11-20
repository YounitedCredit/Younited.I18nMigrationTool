namespace I18nMigrationTool.Services.Interfaces
{
    public interface ITranslationFinderService
    {
        bool IsTranslationEligibleToMigrate(string key);
        void SetJsonKey(string key, string newKey);
    }
}