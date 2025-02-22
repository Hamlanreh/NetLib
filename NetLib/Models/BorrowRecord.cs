using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetLib.Models
{
    public class BorrowRecord
    {
        [Key]
        public int BorrowRecord_Id { get; set; }

        [Required(ErrorMessage = "Please enter Borrower Name")]
        [MaxLength(100, ErrorMessage = "")]
        public string BorrowerName { get; set; }

        [Required(ErrorMessage = "Please enter Borrower Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a Email Address")]
        public string BorrowerEmail { get; set; }

        [Required(ErrorMessage = "Please enter Borrower Phone Number")]
        [Phone(ErrorMessage = "Please enter a Valid Phone Number")]
        public string Phone { get; set; }

        public DateTime BorrowDate { get; set; } = DateTime.Now;

        public DateTime? ReturnDate { get; set; }


        [ForeignKey("Book")]
        [BindNever]
        public int Book_Id { get; set; }
        public Book Book { get; set; }
    }
}
