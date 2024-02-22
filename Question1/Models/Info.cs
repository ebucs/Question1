using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Question1.Models
{
    public class Info
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Person")]
        public string PersonId { get; set; }

        public string? TellNo { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? AddressLine3 { get; set; }

        public string? AddressCode { get; set; }

        public string? PostalAddress1 { get; set; }

        public string? PostalAddress2 { get; set; }

        public string? PostalCode { get; set; }

        public virtual Person Person { get; set; }
    }
}
