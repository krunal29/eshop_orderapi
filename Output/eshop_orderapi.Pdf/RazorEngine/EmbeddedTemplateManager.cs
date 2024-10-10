using RazorEngine.Templating;
using System;
using System.IO;

namespace eshop_orderapi.Pdf.RazorEngine
{
    public class EmbeddedTemplateManager : ITemplateManager
    {
        private readonly string ns;

        public EmbeddedTemplateManager(string @namespace)
        {
            ns = @namespace;
        }

        public ITemplateSource Resolve(ITemplateKey key)
        {
            var resourceName = $"{ns}.{key.Name}.cshtml";
            string content = "";
            var assem = typeof(EmbeddedTemplateManager).Assembly;
            using (var stream = assem.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        content = streamReader.ReadToEnd();
                    }
                }
            }
            return new LoadedTemplateSource(content);
        }

        public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
        {
            return new NameOnlyTemplateKey(name, resolveType, context);
        }

        public void AddDynamic(ITemplateKey key, ITemplateSource source)
        {
            throw new NotImplementedException("dynamic templates are not supported");
        }
    }
}