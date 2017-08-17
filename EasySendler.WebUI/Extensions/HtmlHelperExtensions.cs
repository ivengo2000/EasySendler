using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EasySendler.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string id, string url, string alternateText)
        {
            return Image(helper, id, url, alternateText, null);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string id, string url, string alternateText, object htmlAttributes)
        {
            var builder = new TagBuilder("img");

            builder.GenerateId(id);

            builder.MergeAttribute("src", url);
            builder.MergeAttribute("alt", alternateText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString ImageFromDb(this HtmlHelper helper, byte[] data, string id, string alternateText, object htmlAttributes)
        {
            var imgSrc = "/UploadImages/NoImage.jpg";
            if (data != null && data.Length > 0)
            {
                var base64 = Convert.ToBase64String(data);
                imgSrc = $"data:image/png;base64,{base64}";
            }
            
            return  Image(helper, id, imgSrc, alternateText, htmlAttributes);
        } 
    }
}