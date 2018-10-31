using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebShop.Healpers
{
    public static class PagingHelpers
    {
        public static string PageLinks(this HtmlHelper html,
            int currentPage, int totalPage, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= totalPage; i++)
            {
                TagBuilder tagLI = new TagBuilder("li");
                TagBuilder tag = new TagBuilder("a");// <a>
                var url = HttpUtility.UrlDecode(pageUrl(i));
                tag.MergeAttribute("href", url);
                tag.InnerHtml = i.ToString();
                if (i == currentPage)
                    tagLI.AddCssClass("active");
                tagLI.InnerHtml = tag.ToString();
                result.AppendLine(tagLI.ToString());
            }
            TagBuilder tagDiv = new TagBuilder("div");


            TagBuilder tagUl = new TagBuilder("ul");
            tagUl.AddCssClass("pagination");
            tagUl.InnerHtml = result.ToString();
            tagDiv.InnerHtml = tagUl.ToString();
            return tagDiv.ToString();
        }
    }
}