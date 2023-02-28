namespace Pagination.Models
{
    public class ReturnObjectWrapper<T>
    {
        public List<T> Items { get; set; }

        public string Next { get; set; }
        public string Last { get; set; }
        public string Self { get; set; }
        public string First { get; set; }
    }
}
