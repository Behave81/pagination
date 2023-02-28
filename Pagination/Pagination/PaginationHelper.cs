using Microsoft.AspNetCore.Routing;
using Pagination.Models;

namespace Pagination.Pagination
{
    public static class PaginationHelper
    {
        public static object EncodePageRequest(PageRequestCursor pageRequest)
        {
            var encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(pageRequest)));

            var newCursor = new { cursor = encoded };


            return newCursor;
        }

        public static string GetLink(HttpContext httpContext, PageRequestCursor pageRequest, LinkGenerator _linkGenerator)
        {

            var next = _linkGenerator.GetUriByAction(httpContext, action: "GetAll", values: PaginationHelper.EncodePageRequest(pageRequest));
            return next ?? "";
        }

        public static PageRequestCursor Decode(string value)
        {
            var base64EncodedBytes = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(value));

            var pageRequest = System.Text.Json.JsonSerializer.Deserialize<PageRequestCursor>(base64EncodedBytes);

            if (pageRequest == null)
            {
                throw new Exception("PageRequest is null");
            }
            return pageRequest;
        }
    }
}
