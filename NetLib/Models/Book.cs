using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetLib.Models
{
    public class Book
    {
        [Key]
        public int Book_Id { get; set; }

        [Required(ErrorMessage = "Title field is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author field is required")]
        [MaxLength(100, ErrorMessage = "Author name cannot exceed 100 characters.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "ISBN field is required")]
        [MaxLength(15, ErrorMessage = "ISBN name cannot exceed 100 characters.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Published Date field is required")]
        public DateTime PublishedDate { get; set; }

        [DisplayName("Available")]
        public bool IsAvailable { get; set; } = true;

        public string? Image { get; set; }

        public string? Description { get; set; }

        [BindNever]
        public ICollection<BorrowRecord>? BorrowRecords { get; set; }
    }
}
