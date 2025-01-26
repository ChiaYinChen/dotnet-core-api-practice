using HandlebarsDotNet;

namespace WebApiApp.Services
{
    public class TemplateService
    {
        private readonly string _templateDirectory;

        public TemplateService()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            _templateDirectory = Path.Combine(currentDirectory, "Templates");
        }

        public string RenderTemplate(string templateName, object model)
        {
            var templatePath = Path.Combine(_templateDirectory, templateName);
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template {templateName} not found");
            var htmlTemplate = Handlebars.Compile(File.ReadAllText(templatePath));
            return htmlTemplate(model);
        }
    }
}
