using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Pagination.Context;
using Pagination.Models;
using Pagination.Pagination;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Pagination.Controllers
{
    [Route("/api/v1")]
    public class EntityController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LinkGenerator _linkGenerator;


        public EntityController(AppDbContext context, LinkGenerator linkGenerator)
        {
            _context = context;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("entities")]
        public async Task<ActionResult<ReturnObjectWrapper<Entity>>> GetAll([FromQuery] string? cursor, [FromQuery] string sort = "name", [FromQuery] int limit = 5)
        {
            var r = new ReturnObjectWrapper<Entity>();


            if (String.IsNullOrEmpty(cursor))
            {
                var databaseItems = await _context.entities.OrderBy(sort).Take(limit + 1).ToListAsync();
                r.Items = databaseItems.Take(limit).ToList();

                var pageRequest = new PageRequestCursor
                {
                    Offset = 0,
                    PageCount = limit,
                    SortBy = sort

                };

                /*var encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(pageRequestCursor)));

                var newCursor = new { cursor = encoded };

                var next = _linkGenerator.GetUriByAction(HttpContext, action: "GetAll", values: newCursor);
                if (next != null)
                {
                    r.Next = next;
                }*/
                r.Next = PaginationHelper.GetLink(HttpContext, pageRequest.Next(), _linkGenerator);
                r.First = PaginationHelper.GetLink(HttpContext, pageRequest.First(), _linkGenerator);
                r.Self = PaginationHelper.GetLink(HttpContext, pageRequest.Self(), _linkGenerator);
                r.Last = PaginationHelper.GetLink(HttpContext, pageRequest.Last(), _linkGenerator);

                return r;
            }
            else
            {
                var pageRequest = PaginationHelper.Decode(cursor);

                var pageRequestTest = PaginationHelper.Decode("eyJPZmZzZXQiOjAsIlBhZ2VDb3VudCI6MywiU29ydEJ5IjoiY291bnRlciJ9");

                var databaseItems = await _context.entities.OrderBy(pageRequest.SortBy).Skip(pageRequest.Offset).Take(pageRequest.PageCount + 1).ToListAsync();
                r.Items = databaseItems.Take(pageRequest.PageCount).ToList();

                var next = pageRequest.Next();
                var nextString = PaginationHelper.GetLink(HttpContext, next, _linkGenerator);
                r.Next = nextString;
                var first = pageRequest.First();
                var firstString = PaginationHelper.GetLink(HttpContext, first, _linkGenerator);
                r.First = firstString;
                r.Self = PaginationHelper.GetLink(HttpContext, pageRequest.Self(), _linkGenerator);
                r.Last = PaginationHelper.GetLink(HttpContext, pageRequest.Last(), _linkGenerator);

                return r;
            }



        }

        [HttpPost("entities")]
        public async Task<int> Create()
        {


            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            int i = alphabet.Length;

            foreach (char c in alphabet)
            {
                var e = new Entity
                {
                    Counter = i,
                    Name = c.ToString(),
                    Description = c.ToString()
                };

                _context.entities.Add(e);
                i--;
            }


            await _context.SaveChangesAsync();

            return 1;

        }
    }
}
