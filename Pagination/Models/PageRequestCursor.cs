namespace Pagination.Models
{
    public class PageRequestCursor
    {
        public int Offset { get; set; }
        public int PageCount { get; set; }

        public string SortBy { get; set; }

        public PageRequestCursor Next()
        {
            return new PageRequestCursor
            {
                // todo: need to check max values

                Offset = this.Offset + this.PageCount,
                PageCount = this.PageCount,
                SortBy = this.SortBy
            };
        }

        public PageRequestCursor First()
        {
            return new PageRequestCursor
            {
                // todo: need to check max values

                Offset = 0,
                PageCount = this.PageCount,
                SortBy = this.SortBy
            };
        }

        public PageRequestCursor Self()
        {
            return new PageRequestCursor
            {
                // todo: need to check max values

                Offset = this.Offset,
                PageCount = this.PageCount,
                SortBy = this.SortBy
            };
        }

        public PageRequestCursor Last()
        {
            return new PageRequestCursor
            {
                // todo: need to have max values

                Offset = this.Offset,
                PageCount = this.PageCount,
                SortBy = this.SortBy
            };
        }
    }
}
