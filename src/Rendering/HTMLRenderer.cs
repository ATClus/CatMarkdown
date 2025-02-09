using CatMarkdown.src.Rendering.Interfaces;

namespace CatMarkdown.src.Rendering
{
    public class HTMLRenderer : IRenderer
    {
        public string Render(string parsedContent)
        {
            return string.IsNullOrWhiteSpace(parsedContent) ? string.Empty : parsedContent;
        }
    }
}
