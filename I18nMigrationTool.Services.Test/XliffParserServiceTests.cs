using I18nMigrationTool.Services.Models;
using NFluent;
using NUnit.Framework;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace I18nMigrationTool.Services.Tests
{
    public class XliffParserServiceTests
    {
        private XliffParserService _parserService;

        [SetUp]
        public void Setup()
        {
            _parserService = new XliffParserService(TestContext.CurrentContext.TestDirectory + "\\Data");
        }


        [Test]
        public async Task Can_parse()
        {
            var i18NFile = new XliffFile
            {
                Tag = "Desktop",
                CultureInfo = new CultureInfo("es-ES"),
                Name = "messages.es.xlf"
            };
            var file = _parserService.GetTranslations(i18NFile);

            Check.That(file.Translations.Count).Equals(552);

            Check.That(file.Translations.Single(t => t.Key == "informationSecurity_header").Translation).Equals("Seguridad");

        }

        [Test]
        public async Task Can_replace_html()
        {
            var i18NFile = new XliffFile
            {
                Tag = "Desktop",
                CultureInfo = new CultureInfo("es-ES"),
                Name = "messages.es.xlf"
            };
            var file = _parserService.GetTranslations(i18NFile);

            var translationWithHtml = file.Translations.Single(t => t.Key == "emailStepSubCaption");

            Check.That(translationWithHtml.FormattedTranslation).Equals(@"
  <strong>
Necesitamos tu email para enviarte la oferta </strong>
(sin compromiso)");

        }

        [Test]
        public async Task Can_compute_IsEligibleToMigrate_false()
        {
            var i18NFile = new XliffFile
            {
                Tag = "Desktop",
                CultureInfo = new CultureInfo("es-ES"),
                Name = "messages.es.xlf"
            };
            var file = _parserService.GetTranslations(i18NFile);

            var translationWithHtml = file.Translations.Single(t => t.Key == "offerDetails");

            Check.That(translationWithHtml.IsEligibleToMigrate).IsFalse();
        }

        [Test]
        public void Can_compute_IsEligibleToMigrate_true()
        {
            var i18NFile = new XliffFile
            {
                Tag = "Desktop",
                CultureInfo = new CultureInfo("es-ES"),
                Name = "messages.es.xlf"
            };
            var file = _parserService.GetTranslations(i18NFile);

            var translationWithHtml = file.Translations.Single(t => t.Key == "creditCost");

            Check.That(translationWithHtml.IsEligibleToMigrate).IsTrue();
        }
    }
}