using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using Web_153502_Logvinovich.Domain.Entities;

namespace Web_153502_Logvinovich.TagHelpers
{
    public class PagerTagHelper : TagHelper
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly HttpContext _context;

        [HtmlAttributeName("current-page")]
        public int CurrentPage { get; set; }

        [HtmlAttributeName("total-pages")]
        public int TotalPages { get; set; }

        [HtmlAttributeName("author")]
        public string Author { get; set; }

        [HtmlAttributeName("admin")]
        public bool Admin { get; set; } = false;

        public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor context)
        {
            _linkGenerator = linkGenerator;
            _context = context.HttpContext;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var prev = CurrentPage == 1 ? 1 : CurrentPage - 1;
            var next = CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;

            output.TagName = "nav";
            output.Attributes.SetAttribute("aria-label", "Page navigation example");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            var prevLi = CreatePageLink(prev, "Previous");
            ul.InnerHtml.AppendHtml(prevLi);

            for (var num = 1; num <= TotalPages; num++)
            {
                var li = CreatePageLink(num);
                ul.InnerHtml.AppendHtml(li);
            }

            var nextLi = CreatePageLink(next, "Next");
            ul.InnerHtml.AppendHtml(nextLi);

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreatePageLink(int pageNo, string? text = null)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");

            var uri = "";

            if (!Admin)
            {
                uri = _linkGenerator.GetUriByAction(_context, action: "Index", controller: "Book", values: new { pageNo, Author });
                a.Attributes.Add("ajax", uri);
            }
            else
            {
                uri = _linkGenerator.GetPathByPage(_context, page: "Index", values: new { area="Admin", pageNo });
                a.Attributes.Add("href", uri);
            }
            
            if (text == null)
            {
                a.InnerHtml.Append(pageNo.ToString());
                if (pageNo == CurrentPage)
                {
                    li.AddCssClass("active");
                }
            }
            else
            {
                a.InnerHtml.Append(text);
            }

            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}