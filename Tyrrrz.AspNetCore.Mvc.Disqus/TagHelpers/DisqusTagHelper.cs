using System;
using System.IO;
using System.Reflection;
using System.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Tyrrrz.AspNetCore.Mvc.Disqus.TagHelpers
{
    /// <summary>
    /// Tag helper used to render Disqus threads
    /// </summary>
    public partial class DisqusTagHelper : TagHelper
    {
        /// <summary>
        /// View context
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Whether the tag helper is enabled
        /// </summary>
        [HtmlAttributeName("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Site short name
        /// </summary>
        [HtmlAttributeName("site")]
        public string Site { get; set; }

        /// <summary>
        /// Page URL
        /// </summary>
        [HtmlAttributeName("page-url")]
        public string PageUrl { get; set; }

        /// <summary>
        /// Page ID
        /// </summary>
        [HtmlAttributeName("page-id")]
        public string PageId { get; set; }

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            // Validate attributes
            if (string.IsNullOrWhiteSpace(Site))
                throw new ArgumentException("Site attribute must be set");

            // Return if not enabled
            if (!Enabled)
            {
                output.TagName = null;
                return;
            }

            // Get page URL and page ID if not set
            var requestUrl = ViewContext.HttpContext.Request.Host + ViewContext.HttpContext.Request.Path;
            var pageUrl = PageUrl ?? requestUrl;
            var pageId = PageId ?? pageUrl;

            // Format the content
            var content = TemplateHtml;
            content = content.Replace("__PageUrl__", pageUrl);
            content = content.Replace("__PageId__", pageId);
            content = content.Replace("__Site__", Site);

            // Output
            output.TagName = null;
            output.Content.SetHtmlContent(content);

            base.Process(context, output);
        }
    }

    public partial class DisqusTagHelper
    {
        private static readonly string TemplateHtml;

        static DisqusTagHelper()
        {
            TemplateHtml = GetTemplateHtml();
        }

        private static string GetTemplateHtml()
        {
            const string resourcePath = "Tyrrrz.AspNetCore.Mvc.Disqus.Resources.Template.html";

            var assembly = typeof(DisqusTagHelper).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
                throw new MissingManifestResourceException("Could not find template resource");

            using (stream)
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}