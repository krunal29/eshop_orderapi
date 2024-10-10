using iText.Html2pdf;
using iText.StyledXmlParser.Css.Media;
using eshop_orderapi.Pdf.Models;
using eshop_orderapi.Pdf.RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System.IO;
using System.Linq;

namespace eshop_orderapi.Pdf
{
    public class PdfGenerator
    {
        private static readonly TemplateServiceConfiguration Config = new TemplateServiceConfiguration
        {
            BaseTemplateType = typeof(PdfTemplateBase<>),
            TemplateManager = new EmbeddedTemplateManager(typeof(PdfGenerator).Namespace + ".Templates"),
            CachingProvider = new DefaultCachingProvider()
        };

        private static readonly IRazorEngineService RazorEngine = InitRazorEngine();

        private static IRazorEngineService InitRazorEngine()
        {
            return RazorEngineService.Create(Config);
        }

        private static void GenerateFromTemplate<T>(string template, string path = "", params T[] models)
        {
            var html1 = models.Select(model => RazorEngine.RunCompile(template, typeof(T), models[0])).ToArray();
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            ConverterProperties converterProperties = new ConverterProperties().SetMediaDeviceDescription(new MediaDeviceDescription(MediaType.PRINT));
            HtmlConverter.ConvertToPdf(html1[0], new FileStream(path, FileMode.Create), converterProperties);
        }

        public static void GenerateSamplePdf(string path = "", params SampleModelPdf[] SamplePdf)
        {
            GenerateFromTemplate("SamplePdf", path, SamplePdf);
        }
    }
}