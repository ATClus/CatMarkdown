using CatMarkdown.src.Parsing.Interfaces;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CatMarkdown.src.Parsing
{
    public class MarkdownParser : IMarkdownParser
    {
        public string Parse(string markdown)
        {
            if (string.IsNullOrWhiteSpace(markdown))
                return string.Empty;

            // Dividir o conteúdo em linhas
            var lines = markdown.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var sb = new StringBuilder();

            // Estados para blocos
            bool inCodeBlock = false;
            string codeBlockLanguage = "";
            var codeBlockContent = new StringBuilder();

            bool inList = false;
            string listType = ""; // "ul" ou "ol"
            var listItems = new List<string>();

            var paragraphLines = new List<string>();
            var blockquoteLines = new List<string>();

            // Métodos locais para liberar blocos acumulados
            void FlushParagraph()
            {
                if (paragraphLines.Count > 0)
                {
                    string paragraph = string.Join(" ", paragraphLines);
                    sb.AppendFormat("<p>{0}</p>", paragraph);
                    paragraphLines.Clear();
                }
            }

            void FlushList()
            {
                if (inList && listItems.Count > 0)
                {
                    if (listType == "ul")
                        sb.Append("<ul>");
                    else if (listType == "ol")
                        sb.Append("<ol>");

                    foreach (var item in listItems)
                    {
                        sb.AppendFormat("<li>{0}</li>", item);
                    }

                    if (listType == "ul")
                        sb.Append("</ul>");
                    else if (listType == "ol")
                        sb.Append("</ol>");

                    listItems.Clear();
                    inList = false;
                    listType = "";
                }
            }

            void FlushBlockquote()
            {
                if (blockquoteLines.Count > 0)
                {
                    // Junta as linhas do blockquote separando por <br/>
                    string content = string.Join("<br/>", blockquoteLines);
                    sb.AppendFormat("<blockquote>{0}</blockquote>", content);
                    blockquoteLines.Clear();
                }
            }

            // Processamento linha a linha
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                // Verificar início/fim de bloco de código (fenced code block)
                if (line.StartsWith("```"))
                {
                    FlushParagraph();
                    FlushList();
                    FlushBlockquote();

                    if (!inCodeBlock)
                    {
                        inCodeBlock = true;
                        codeBlockLanguage = line.Length > 3 ? line.Substring(3).Trim() : "";
                        codeBlockContent.Clear();
                    }
                    else
                    {
                        // Finaliza bloco de código
                        string languageClass = string.IsNullOrEmpty(codeBlockLanguage) ? "" : $" class=\"language-{codeBlockLanguage}\"";
                        sb.AppendFormat("<pre><code{0}>{1}</code></pre>",
                            languageClass,
                            WebUtility.HtmlEncode(codeBlockContent.ToString()));
                        inCodeBlock = false;
                        codeBlockLanguage = "";
                        codeBlockContent.Clear();
                    }
                    continue;
                }

                if (inCodeBlock)
                {
                    codeBlockContent.AppendLine(line);
                    continue;
                }

                // Linha em branco: libera os blocos pendentes
                if (string.IsNullOrWhiteSpace(line))
                {
                    FlushParagraph();
                    FlushList();
                    FlushBlockquote();
                    continue;
                }

                // Blockquote (linhas iniciadas por '>')
                if (line.TrimStart().StartsWith(">"))
                {
                    FlushParagraph();
                    FlushList();
                    // Remove o '>' e espaço eventual
                    string content = line.TrimStart().TrimStart('>', ' ');
                    blockquoteLines.Add(ProcessInline(content));
                    continue;
                }
                else if (blockquoteLines.Count > 0)
                {
                    // Se a linha não é blockquote, libera o bloco acumulado
                    FlushBlockquote();
                }

                // Itens de lista não ordenada (iniciados por -, + ou *)
                var ulMatch = Regex.Match(line, @"^\s*([-+*])\s+(.*)$");
                if (ulMatch.Success)
                {
                    FlushParagraph();
                    if (!inList || listType != "ul")
                    {
                        FlushList();
                        inList = true;
                        listType = "ul";
                    }
                    listItems.Add(ProcessInline(ulMatch.Groups[2].Value.Trim()));
                    continue;
                }

                // Itens de lista ordenada (iniciados por dígitos e ponto)
                var olMatch = Regex.Match(line, @"^\s*\d+\.\s+(.*)$");
                if (olMatch.Success)
                {
                    FlushParagraph();
                    if (!inList || listType != "ol")
                    {
                        FlushList();
                        inList = true;
                        listType = "ol";
                    }
                    listItems.Add(ProcessInline(olMatch.Groups[1].Value.Trim()));
                    continue;
                }

                // Títulos (Headings) – de 1 a 6 (#)
                var headingMatch = Regex.Match(line, @"^(#{1,6})\s+(.*)$");
                if (headingMatch.Success)
                {
                    FlushParagraph();
                    FlushList();
                    FlushBlockquote();
                    int level = headingMatch.Groups[1].Value.Length;
                    string content = ProcessInline(headingMatch.Groups[2].Value.Trim());
                    sb.AppendFormat("<h{0}>{1}</h{0}>", level, content);
                    continue;
                }

                // Regra horizontal (pelo menos 3 caracteres - ou * ou _)
                if (Regex.IsMatch(line.Trim(), @"^([-*_]\s*){3,}$"))
                {
                    FlushParagraph();
                    FlushList();
                    FlushBlockquote();
                    sb.Append("<hr />");
                    continue;
                }

                // Caso padrão: acumula como conteúdo de parágrafo
                paragraphLines.Add(ProcessInline(line.Trim()));
            }

            // Libera quaisquer blocos que ainda estejam abertos
            FlushParagraph();
            FlushList();
            FlushBlockquote();

            return $"<div id='markdown-note'>{sb.ToString()}</div>";
        }

        /// <summary>
        /// Processa elementos inline: imagens, links, código, negrito e itálico.
        /// </summary>
        private string ProcessInline(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            // Processar imagem: ![alt](url)
            text = Regex.Replace(text, @"!\[([^\]]*)\]\(([^)]+)\)", "<img src=\"$2\" alt=\"$1\" />");

            // Processar link: [texto](url)
            text = Regex.Replace(text, @"\[(.*?)\]\((.*?)\)", "<a href=\"$2\">$1</a>");

            // Processar código inline: `código`
            text = Regex.Replace(text, @"`([^`]+)`", "<code>$1</code>");

            // Processar ênfases (a ordem é importante)
            // Primeiro, ênfase mista: ***texto***
            text = Regex.Replace(text, @"\*\*\*(.+?)\*\*\*", "<strong><em>$1</em></strong>");
            // Negrito: **texto**
            text = Regex.Replace(text, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
            // Itálico: *texto*
            text = Regex.Replace(text, @"\*(.+?)\*", "<em>$1</em>");

            return text;
        }
    }
}
