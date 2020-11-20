using I18nMigrationTool.Services.Interfaces;
using NFluent;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace I18nMigrationTool.Services.Tests
{
    public class DummyFinderService : ITranslationFinderService
    {
        public bool IsTranslationEligibleToMigrate(string key)
        {
            return true;
        }

        public void SetJsonKey(string key, string newKey)
        {
        }
    }

    public class MigrateToJsonServiceTests
    {
        private IMigrateToJsonService _migraterService;

        [SetUp]
        public void Setup()
        {
            _migraterService = new MigrateToJsonService(new DummyFinderService());
        }


        [Test]
        public async Task Can_find_0_key()
        {
            var originalHtml = "";

            var migrationResult = await _migraterService.ReplaceI18N(originalHtml);

            Check.That(migrationResult.KeyMigrationResults.Count).Equals(0);

        }

        [Test]
        public async Task Can_find_1_key()
        {
            var originalHtml = "<div i18n=\"@@superKey\">defaultText</div>";

            var migrationResult = await _migraterService.ReplaceI18N(originalHtml);

            Check.That(migrationResult.KeyMigrationResults.Count).Equals(1);

        }

        [Test]
        public async Task Can_find_2_key()
        {
            var originalHtml = "<div i18n=\"@@superKey\">defaultText</div><div i18n=\"@@superKey2\">defaultText</div>";

            var migrationResult = await _migraterService.ReplaceI18N(originalHtml);

            Check.That(migrationResult.KeyMigrationResults.Count).Equals(2);

        }

        [Test]
        public async Task I18NKey_is_correct()
        {
            var originalHtml = "<div i18n=\"@@superKey\">defaultText</div>";

            var migrationResult = await _migraterService.ReplaceI18N(originalHtml);

            var migrationKeyResult = migrationResult.KeyMigrationResults.Single();

            Check.That(migrationKeyResult.I18NKey).Equals("superKey");

        }

        [Test]
        public async Task NewKey_is_correct()
        {
            var originalHtml = "<div i18n=\"@@superKey\">defaultText</div>";

            var migrationResult = await _migraterService.ReplaceI18N(originalHtml);

            var migrationKeyResult = migrationResult.KeyMigrationResults.Single();

            Check.That(migrationKeyResult.NewKey).Equals("super_key");
        }

        [Test]
        public async Task NewKey_is_in_lower_case()
        {
            var originalHtml = "<div i18n=\"@@EMAIL_LBL\">defaultText</div>";

            var migrationResult = await _migraterService.ReplaceI18N(originalHtml);

            var migrationKeyResult = migrationResult.KeyMigrationResults.Single();

            Check.That(migrationKeyResult.NewKey).Equals("email_lbl");
        }

        [Test]
        public async Task NewHtml_is_correct()
        {
            var originalHtml = "<div i18n=\"@@superKey\">defaultText</div><span>test</span>";

            var migrationResult = await _migraterService.ReplaceI18N(originalHtml);

            Check.That(migrationResult.NewHtml).Equals("<div [innerHTML]=\"'super_key' | translate\"></div><span>test</span>");
        }

        [Test]
        public async Task NgFor_is_preserved()
        {
            var originalHtml = @"<li class=""item d-flex"" *ngFor=""let review of (customerReviews$ | async).reviews; index as i""><div class=""img--rounded--small mr-3""><img src=""/assets/img/avatars/perso-{{ i + 1 }}.png"" *ngIf=""ok"" alt="" {{ review.customerName }}""></div><div><p class=""mt-0 font-italic small-text"">{{ review.comment }}</p><div class=""title-4 mt-1 mb-0"">{{ review.customerName }}</div></div></li>";

            var migrationResult = await _migraterService.ReplaceI18N(originalHtml);

            Check.That(migrationResult.NewHtml).Contains(originalHtml);
        }

        [Test]
        public async Task Template_variable_is_preserved()
        {
            var originalHtml = @"<dyn-textbox #agentFiscalCode id=""agentFiscalCode"" [params]=""stepParams.agentFiscalCode.params"" [value]=""stepParams.agentFiscalCode.value$ | async"" place-holder=""Please enter agent fiscal code"" i18n-place-holder=""@@ENTER_AGENT_FISCAL_CODE_LBL"" max-length=""16""></dyn-textbox>";

            var migrationResult = await _migraterService.ReplaceI18N(originalHtml);

            Check.That(migrationResult.NewHtml).Contains(originalHtml);
        }
    }

}