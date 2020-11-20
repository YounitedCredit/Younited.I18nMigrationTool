using I18nMigrationTool.Services.Models;
using System.Threading.Tasks;

namespace I18nMigrationTool.Services.Interfaces
{
    public interface IMigrateToJsonService
    {
        Task<FileMigrationResult> ReplaceI18N(string templateHtml);
    }
}