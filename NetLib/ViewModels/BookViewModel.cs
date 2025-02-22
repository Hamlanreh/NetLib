using NetLib.Models;

namespace NetLib.ViewModels
{
    public class BookViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public string? Search {  get; set; } 
    }
}
