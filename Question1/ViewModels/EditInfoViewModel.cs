using Question1.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Question1.ViewModels
{
    public class EditInfoViewModel
    {
        public string PersonId { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? TellNo { get; set; }

        public string? CellNo { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? AddressLine3 { get; set; }

        public string? AddressCode { get; set; }

        public string? PostalAddress1 { get; set; }

        public string? PostalAddress2 { get; set; }

        public string? PostalCode { get; set; }

        public Person? Person { get; set; }
    }
}
