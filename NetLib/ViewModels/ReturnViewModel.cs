using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetLib.Models;
using System.ComponentModel.DataAnnotations;

namespace NetLib.ViewModels
{
    public class ReturnViewModel
    {
        [Required]
        public int BorrowRecord_Id { get; set; }
        [BindNever]
        public string? BookTitle { get; set; }
        [BindNever]
        public string? BorrowerName { get; set; }
        [BindNever]
        public DateTime? BorrowDate { get; set; }
    }
}
