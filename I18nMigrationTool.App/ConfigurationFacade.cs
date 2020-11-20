using I18nMigrationTool.Services.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace I18nMigrationTool.App
{
    public class ConfigurationFacade
    {
        private readonly IConfigurationRoot _configuration;

        public ConfigurationFacade()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json").Build();
        }

        public string XlfFilesRootPath => _configuration["xlfRootPath"];


        public List<XliffFile> XlfFiles
        {
            get
            {
                var xlfFiles = new List<XliffFile>();
                var xlfFilesConfiguration = _configuration.GetSection("xlfFiles").GetChildren();
                foreach (var file in xlfFilesConfiguration)
                {
                    xlfFiles.Add(new XliffFile
                    {
                        Name = file["name"],
                        CultureInfo = new CultureInfo(file["culture"]),
                        Tag = file["tag"]

                    });
                };
                return xlfFiles;
            }
        }

        public string AngularProjectRootPath => _configuration["angularProjectRootPath"];
        public string JsonTargetPath => _configuration["jsonTargetPath"];
        public string ReportFilePath => _configuration["reportFilePath"];
        public string[] TemplateFilesPath => Directory.GetFiles(AngularProjectRootPath, "*.html",
            SearchOption.AllDirectories);
    }
}