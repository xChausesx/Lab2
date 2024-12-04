using Lab2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lab2.TagHelpers
{
    namespace MvcApp.TagHelpers
    {
        public class PageLinkTagHelper : TagHelper
        {
            IUrlHelperFactory urlHelperFactory;
            public PageLinkTagHelper(IUrlHelperFactory helperFactory)
            {
                urlHelperFactory = helperFactory;
            }
            [ViewContext]
            [HtmlAttributeNotBound]
            public ViewContext ViewContext { get; set; } = null!;
            public PageViewModel? PageModel { get; set; }
            public string PageAction { get; set; } = "";
            public override void Process(TagHelperContext context, TagHelperOutput output)
            {
                if (PageModel == null) throw new Exception("PageModel is not set");
                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
                output.TagName = "div";
                // Набір посилань буде являти собою список ul
                TagBuilder tag = new("ul");
                tag.AddCssClass("pagination");
                // Сформуйте три посилання - на поточне, попереднє і наступне
                TagBuilder currentItem = CreateTag(urlHelper, PageModel.PageNumber);

                if (PageModel.HasPreviousPage)
                {
                    TagBuilder prevItem = CreateTag(urlHelper, PageModel.PageNumber - 1);
                    tag.InnerHtml.AppendHtml(prevItem);
                }
                tag.InnerHtml.AppendHtml(currentItem);

                if (PageModel.HasNextPage)
                {
                    TagBuilder nextItem = CreateTag(urlHelper, PageModel.PageNumber + 1);
                    tag.InnerHtml.AppendHtml(nextItem);
                }
                output.Content.AppendHtml(tag);
            }
            TagBuilder CreateTag(IUrlHelper urlHelper, int pageNumber = 1)
            {
                TagBuilder item = new TagBuilder("li");
                TagBuilder link = new TagBuilder("a");
                if (pageNumber == PageModel?.PageNumber)
                {
                    item.AddCssClass("active");
                }
                else
                {
                    link.Attributes["href"] = urlHelper.Action(PageAction,
                        new { page = pageNumber, pageSize = PageModel?.PageSize });
                }
                item.AddCssClass("page-item");
                link.AddCssClass("page-link");

                link.InnerHtml.Append(pageNumber.ToString());
                item.InnerHtml.AppendHtml(link);
                return item;
            }
        }
    }
}
