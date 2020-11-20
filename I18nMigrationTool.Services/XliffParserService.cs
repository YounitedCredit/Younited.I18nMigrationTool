using I18nMigrationTool.Services.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace I18nMigrationTool.Services
{
    public class XliffParserService
    {
        private readonly string _xlfRootPath;

        public XliffParserService(string xlfRootPath)
        {
            _xlfRootPath = xlfRootPath;
        }

        public I18NFile GetTranslations(XliffFile file)
        {
            var translations = new List<I18NTranslation>();
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);

            var rawFileContent = File.ReadAllText(Path.Combine(_xlfRootPath, file.Name));

            var cleanContent = rawFileContent
                .Replace("&nbsp;", "")
                .Replace(" & ", " &amp; ")
                .Replace("&eacute;", "é")
                .Replace("&oacute;", "ó")
                .Replace("&aacute;", "á");

            var cleanFile = Path.GetDirectoryName(_xlfRootPath) + "\\" + nameWithoutExtension + "-work.xlf";
            File.WriteAllText(cleanFile, cleanContent);


            var doc = XDocument.Load(cleanFile);
            if (doc.Root != null)
            {
                var df = doc.Root.Name.Namespace;
                foreach (var transUnitNode in doc.Descendants(df + "trans-unit"))
                {
                    var id = transUnitNode.FirstAttribute.Value;
                    var targetNode = transUnitNode.Element(df + "target");

                    if (targetNode != null)
                    {
                        var translation = targetNode.ToString()
                            .Replace("<target xmlns=\"urn:oasis:names:tc:xliff:document:1.2\">", "").Replace("</target>", "");

                        translations.Add(new I18NTranslation
                        {
                            Key = id,
                            Translation = translation,
                            FormattedTranslation = FormatHtml(translation),
                            IsEligibleToMigrate = IsEligibleToMigrate(translation)
                        });
                    }
                }
            }

            File.Delete(cleanFile);

            return new I18NFile
            {
                CultureInfo = file.CultureInfo,
                Tag = file.Tag,
                Translations = translations
            };
        }

        private bool IsEligibleToMigrate(string translation)
        {
            return !translation.Contains("START_LINK") && !translation.Contains("INTERPOLATION");
        }

        private string FormatHtml(string original)
        {
            return original?
                .Replace("<x id=\"START_TAG_STRONG\" ctype=\"x-strong\" />", "<strong>")
                .Replace("<x id=\"CLOSE_TAG_STRONG\" ctype=\"x-strong\" />", "</strong>")
                .Replace("<x id=\"START_TAG_STRONG_1\" ctype=\"x-strong\" />", "<strong>")
                .Replace("<x id=\"CLOSE_TAG_STRONG_1\" ctype=\"x-strong\" />", "</strong>")
                .Replace("<x id=\"START_PARAGRAPH\" ctype=\"x-p\" />", "<p>")
                .Replace("<x id=\"CLOSE_PARAGRAPH\" ctype=\"x-p\" />", "</p>")
                .Replace("<x id=\"START_UNORDERED_LIST\" ctype=\"x-ul\" />", "<ul>")
                .Replace("<x id=\"CLOSE_UNORDERED_LIST\" ctype=\"x-ul\" />", "</ul>")
                .Replace("<x id=\"START_LIST_ITEM\" ctype=\"x-li\" />", "<li>")
                .Replace("<x id=\"CLOSE_LIST_ITEM\" ctype=\"x-li\" />", "</li>")
                .Replace("<x id=\"START_TAG_SPAN\" ctype=\"x-span\" />", "<span>")
                .Replace("<x id=\"CLOSE_TAG_SPAN\" ctype=\"x-span\" />", "</span>")
                .Replace("<x id=\"LINE_BREAK\" ctype=\"x-lb\" />", "<br/>")
                .Replace("<x id=\"LINE_BREAK\" ctype=\"lb\" />", "<br/>")
                .Replace("<x id=\"START_SMALL_TEXT\" ctype=\"x-small\" />", "<small>")
                .Replace("<x id=\"CLOSE_SMALL_TEXT\" ctype=\"x-small\" />", "</small>")
                ;
        }
    }
}